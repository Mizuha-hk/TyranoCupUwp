using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// ユーザー コントロールの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace TyranoCupUwpApp.Views.Controls
{
    public sealed partial class CustomCalendar : UserControl
    {
        public CustomCalendar()
        {
            this.InitializeComponent();
        }

        private List<EventOverView> Events { get; set; } = new List<EventOverView>() { new EventOverView() { Date = DateTime.Today , Subject ="Test" } };

        private void MainCalendar_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs args)
        {
            args.Item.DataContext = Events
                .Where(x => x.Date == args.Item.Date.Date)
                .ToList();
        }
    }


    public class EventOverView
    {
        public DateTime Date { get; set; }
        public string Subject { get; set; }
    }
}
