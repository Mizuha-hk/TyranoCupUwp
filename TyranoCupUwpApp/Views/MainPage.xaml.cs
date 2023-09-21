using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.System;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using TyranoCupUwpApp.Shared;
using TyranoCupUwpApp.Shared.api;
using TyranoCupUwpApp.Shared.Models;
using Windows.System.UserProfile;
using Windows.ApplicationModel.Appointments;
using System.Threading.Tasks;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace TyranoCupUwpApp
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            LoadAppointment();
            this.InitializeComponent();
            Loaded += MainPage_LoadedAsync;
        }

        #region parameters
        private IList<Appointment> appointments = new List<Appointment>();
        private IRecord recordEngine = new Record();
        private IVoiceRecognition voiceRecognitionEngine = new VoiceRecognition();
        private ApiKeyManagement apiKeyManagement = ApiKeyManagement.GetInstance();
        private IOpenAIFormation openAIFormationEngine = new OpenAIFormation();
        private ISchedule scheduleEngine = new Schedule();
        private INotification notificationEngine = new Notification();
        private IAccessDb db = new AccessDb();

        private List<EventOverView> Events { get; set; } = new List<EventOverView>();

        #endregion

        private async void LoadAppointment()
        {
            var appointmentStore =
                await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AllCalendarsReadOnly);

            var rangeStart = DateTimeOffset.Now.AddMonths(-6);
            var rangeEnd = TimeSpan.FromDays((rangeStart.AddMonths(12) - rangeStart).Days);

            var appointments = await appointmentStore.FindAppointmentsAsync(rangeStart, rangeEnd);

            this.appointments = appointments.ToList();

            foreach (var appointment in appointments)
            {
                Events.Add(new EventOverView() { Date = appointment.StartTime.Date, Subject = appointment.Subject });
            }
        }

        private async void MainPage_LoadedAsync(object sender, RoutedEventArgs e)
        {
            await apiKeyManagement.Initialize();
            await db.InitializeDatabase();
        }

        private async void SpeechButton_Click(object sender, RoutedEventArgs e)
        {
            if(recordEngine.IsRecording)
            {
                SpeechButtonIcon.Glyph = "\uE720";
                var fileName = await recordEngine.StopRecording();
                var text = await voiceRecognitionEngine.VoiceRecognitionFromWavFile(fileName + ".wav", "ja-JP", apiKeyManagement.SpeechApiKey );
                await openAIFormationEngine.FormatTextToJson(text, apiKeyManagement.OpenAIApiKey);
                var id = await scheduleEngine.Add(ScheduleModel.GetInstance());
                notificationEngine.Schedule(
                    id,
                    ScheduleModel
                        .GetInstance()
                        .Subject,
                    fileName,
                    ScheduleModel
                        .GetInstance()
                        .StartTime
                        .AddMinutes(-30)
                        .Date);
                db.Create(new SaveAudioModel() { AppointmentId = id, AudioId = fileName });

                ContentDialog noWifiDialog = new ContentDialog
                {
                    Title = "Schedule Completed !",
                    Content = $"Title : {ScheduleModel.GetInstance().Subject} \r\n StartTime : {ScheduleModel.GetInstance().StartTime}",
                    CloseButtonText = "finish"
                };
            }
            else
            {
                SpeechButtonIcon.Glyph = "\uE71A";
                await recordEngine.StartRecording();               
            }
        }

        private void MainCalendar_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs args)
        {
            args.Item.DataContext = Events
                .Where(x => x.Date == args.Item.Date.Date)
                .ToList();

            if(args.Phase == 0)
            {
                args.RegisterUpdateCallback(MainCalendar_CalendarViewDayItemChanging);
            }
            
        }

        private void SideCalendar_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            if(args.AddedDates.Count > 0)
            {
                MainCalendar.SetDisplayDate(args.AddedDates.First());
            }
        }

        private void MainCalendar_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            if (args.AddedDates.Count > 0)
            {
                SideCalendar.SetDisplayDate(args.AddedDates.First());

                var date = args.AddedDates.First();
                DescriptionView.SetScheduleDetailsList(appointments, date);
            }
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(SpeechInputBox.Text))
            {
                await openAIFormationEngine.FormatTextToJson(SpeechInputBox.Text, apiKeyManagement.OpenAIApiKey);
                var id = await scheduleEngine.Add(ScheduleModel.GetInstance());
                notificationEngine.Schedule(
                    id,
                    ScheduleModel
                        .GetInstance()
                        .Subject,
                    "",
                    ScheduleModel
                        .GetInstance()
                        .StartTime
                        .AddMinutes(-30)
                        .Date);
            }
        }
    }

    public class EventOverView
    {
        public DateTime Date { get; set; }
        public string Subject { get; set; }
    }
}
