using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TyranoCupUwpApp.Models;
using Windows.ApplicationModel.Appointments;
using Windows.UI.Xaml.Controls;

// ユーザー コントロールの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace TyranoCupUwpApp.Views.Controls
{
    public sealed partial class AppointmentDescription : UserControl
    {
        public AppointmentDescription()
        {
            this.InitializeComponent();
        }

        IList<ScheduleDetails> ScheduleDetailsList = new ObservableCollection<ScheduleDetails>();

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
                    StartTime = appointment.StartTime.ToString("yyyy/MM/dd tt hh:mm:ss ~ "),
                    EndTime = appointment.StartTime.AddDays(appointment.Duration.Days).ToString("yyyy/MM/dd tt hh:mm:ss"),
                    Location = appointment.Location,
                });
            }

            InitializeComponent();
        }
    }
}
