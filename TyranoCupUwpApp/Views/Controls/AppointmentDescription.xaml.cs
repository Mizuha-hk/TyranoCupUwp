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
using Windows.ApplicationModel.Appointments;

// ユーザー コントロールの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace TyranoCupUwpApp.Views.Controls
{
    public sealed partial class AppointmentDescription : UserControl
    {
        public AppointmentDescription()
        {
            this.InitializeComponent();
        }

        IList<ScheduleDetails> ScheduleDetailsList = new ObservableCollection<ScheduleDetails>() {
            new ScheduleDetails() { Title = "test", StartTime = DateTime.Now, EndTime = DateTime.Now, Location = "test" },  
        };

        public void SetScheduleDetailsList(IList<Appointment> appointments, DateTimeOffset date)
        {
            ScheduleDetailsList.Clear();

            var dayAppointments = appointments
                .Where(x => x.StartTime.Date == date.Date);

            foreach (var appointment in dayAppointments)
            {
                ScheduleDetailsList.Add(new ScheduleDetails() 
                { 
                    Title = appointment.Subject,
                    StartTime = appointment.StartTime.Date,
                    EndTime = appointment.StartTime.AddDays(appointment.Duration.Days).Date,
                    Location = appointment.Location,
                });
            }

            InitializeComponent();
        }
    }
}
