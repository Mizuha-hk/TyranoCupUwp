using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TyranoCupUwpApp.Shared;
using TyranoCupUwpApp.Shared.api;
using TyranoCupUwpApp.Shared.Models;
using Windows.ApplicationModel.Appointments;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
        private IList<Appointment> appointments = new ObservableCollection<Appointment>();
        private IList<EventOverView> Events { get; set; } = new ObservableCollection<EventOverView>();

        private IRecord recordEngine = new Record();
        private IVoiceRecognition voiceRecognitionEngine = new VoiceRecognition();
        private ApiKeyManagement apiKeyManagement = ApiKeyManagement.GetInstance();
        private IOpenAIFormation openAIFormationEngine = new OpenAIFormation();
        private ISchedule scheduleEngine = new Schedule();
        private INotification notificationEngine = new Notification();
        private IAccessDb db = new AccessDb();

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
            if (recordEngine.IsRecording)
            {
                SpeechButtonIcon.Glyph = "\uE720";

                var fileName = await recordEngine.StopRecording();
                var text = await voiceRecognitionEngine.VoiceRecognitionFromWavFile(fileName + ".wav", "ja-JP", apiKeyManagement.SpeechApiKey);
                var data = await openAIFormationEngine.FormatTextToJson(text, apiKeyManagement.OpenAIApiKey);
                if (data == null || data.Subject == string.Empty)
                {
                    ContentDialog failedDialog = new ContentDialog
                    {
                        Title = "Schedule Failed !",
                        CloseButtonText = "Ok"
                    };

                    await failedDialog.ShowAsync();
                    return;
                }

                var id = await scheduleEngine.Add(data);

                if (id == null)
                {
                    ContentDialog failedDialog = new ContentDialog
                    {
                        Title = "Schedule Canceled !",
                        Content = $"Subject : {data.Subject} \r\n StartTime : {data.StartTime}\r\n Duration : {data.Duration}",
                        CloseButtonText = "Ok"
                    };

                    await failedDialog.ShowAsync();
                    return;
                }

                notificationEngine.Schedule(
                    id,
                    data.Subject,
                    fileName,
                    data.StartTime.AddMinutes(-30).Date < DateTime.Now
                        ? DateTime.Now.AddSeconds(5)
                        : data.StartTime.AddMinutes(-30).Date);

                appointments.Add(new Appointment()
                {
                    Subject = data.Subject,
                    StartTime = data.StartTime,
                    Duration = data.Duration,
                    Location = data.Location,
                    Reminder = data.Reminder
                });

                Events.Add(new EventOverView() { Date = data.StartTime.Date, Subject = data.Subject });

                db.Create(new SaveAudioModel() { AppointmentId = id, AudioId = fileName });

                ContentDialog Dialog = new ContentDialog
                {
                    Title = "Schedule Completed !",
                    Content = $"Title : {data.Subject} \r\n StartTime : {data.StartTime}\r\n Duration : {data.Duration}",
                    CloseButtonText = "Ok"
                };

                await Dialog.ShowAsync();
            }
            else
            {
                await recordEngine.StartRecording();
                SpeechButtonIcon.Glyph = "\uE71A";
            }
        }

        private void MainCalendar_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs args)
        {
            args.Item.DataContext = Events
                .Where(x => x.Date == args.Item.Date.Date)
                .ToList();

            if (args.Phase == 0)
            {
                args.RegisterUpdateCallback(MainCalendar_CalendarViewDayItemChanging);
            }

        }

        private void SideCalendar_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            if (args.AddedDates.Count > 0)
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
            if (!string.IsNullOrEmpty(SpeechInputBox.Text))
            {
                var data = await openAIFormationEngine.FormatTextToJson(SpeechInputBox.Text, apiKeyManagement.OpenAIApiKey);
                if (data == null || data.Subject == string.Empty)
                {
                    ContentDialog faledDialog = new ContentDialog
                    {
                        Title = "Schedule Failed !",
                        CloseButtonText = "Ok"
                    };
                    await faledDialog.ShowAsync();
                    return;
                }
                var id = await scheduleEngine.Add(data);

                if (id == null)
                {
                    ContentDialog canceledDialog = new ContentDialog
                    {
                        Title = "Schedule Canceled !",
                        Content = $"Subject : {data.Subject} \r\n StartTime : {data.StartTime}\r\n Duration : {data.Duration}",
                        CloseButtonText = "Ok"
                    };
                    await canceledDialog.ShowAsync();
                }
                appointments.Add(new Appointment()
                {
                    Subject = data.Subject,
                    StartTime = data.StartTime,
                    Duration = data.Duration,
                    Location = data.Location,
                    Reminder = data.Reminder
                });

                Events.Add(new EventOverView() { Date = data.StartTime.Date, Subject = data.Subject });


                ContentDialog contentDialog = new ContentDialog
                {
                    Title = "Schedule Completed !",
                    Content = $"Title : {data.Subject} \r\n StartTime : {data.StartTime} \r\n Duration : {data.Duration}",
                    CloseButtonText = "Finish"
                };


                await contentDialog.ShowAsync();
            }
        }

        private async void SubmitTextScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(
                string.IsNullOrWhiteSpace(SubjectBox.Text)
                || StartDatePicker.SelectedDate == null
                || StartTimePicker.SelectedTime == null
                || EndDatePicker.SelectedDate == null
                || EndTimePicker.SelectedTime == null))
            {
                var data = ScheduleModel.GetInstance();
                data.Subject = SubjectBox.Text;
                data.Location = LocationBox.Text;
                data.StartTime = StartDatePicker.SelectedDate.Value.Date + StartTimePicker.SelectedTime.Value;
                data.Duration = EndDatePicker.SelectedDate.Value.Date + EndTimePicker.SelectedTime.Value - StartDatePicker.SelectedDate.Value.Date - StartTimePicker.Time;

                appointments.Add(new Appointment()
                {
                    Subject = data.Subject,
                    StartTime = data.StartTime,
                    Duration = data.Duration,
                    Location = data.Location,
                    Reminder = data.Reminder
                });

                Events.Add(new EventOverView() { Date = data.StartTime.Date, Subject = data.Subject });

                await scheduleEngine.Add(data);
            }

        }
    }

    public class EventOverView
    {
        public DateTime Date { get; set; }
        public string Subject { get; set; }
    }
}
