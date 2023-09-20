using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
            this.InitializeComponent();
            Loaded += MainPage_LoadedAsync;
        }

        #region parameters

        private IRecord recordEngine = new Record();
        private IVoiceRecognition voiceRecognitionEngine = new VoiceRecognition();

        private List<EventOverView> Events { get; set; } = new List<EventOverView>() { new EventOverView() { Date = DateTime.Today, Subject = "Test" } };

        #endregion

        private async void MainPage_LoadedAsync(object sender, RoutedEventArgs e)
        {
            await voiceRecognitionEngine.GetAzureSpeechApiKey();
        }

        private async void SpeechButton_Click(object sender, RoutedEventArgs e)
        {
            if(recordEngine.IsRecording)
            {
                SpeechButtonIcon.Glyph = "\uE720";
                var fileName = await recordEngine.StopRecording();           
                var result = await voiceRecognitionEngine.VoiceRecognitionFromWavFile(fileName, GlobalizationPreferences.Languages[0]);

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
            }
        }
    }

    public class EventOverView
    {
        public DateTime Date { get; set; }
        public string Subject { get; set; }
    }
}
