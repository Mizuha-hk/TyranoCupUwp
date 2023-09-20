using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using TyranoCupUwpApp.Models;

// ユーザー コントロールの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace TyranoCupUwpApp.Views.Controls
{
    public sealed partial class AppointmentDescription : UserControl
    {
        public AppointmentDescription()
        {
            this.InitializeComponent();
        }

        ObservableCollection<ScheduleDetails> ScheduleDetailsList = new ObservableCollection<ScheduleDetails>()
        {
            new ScheduleDetails(){ Title = "hoge", Location = "location1", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1)  },
            new ScheduleDetails(){ Title = "fuga", Location = "location2", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1)  },
            new ScheduleDetails(){ Title = "piyo", Location = "location3", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1)  },
        };
    }
}
