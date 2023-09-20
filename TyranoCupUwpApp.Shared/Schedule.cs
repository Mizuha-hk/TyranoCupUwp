using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TyranoCupUwpApp.Shared.api;
using TyranoCupUwpApp.Shared.Models;
using Windows.ApplicationModel.Appointments;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;

namespace TyranoCupUwpApp.Shared
{
    public class Schedule : ISchedule
    {
        public async Task<string> Add(ScheduleModel sch)
        {
            Appointment appointment = new Appointment();
            var amfu = AppointmentManager.GetForUser(User.GetDefault());
            var asr = await amfu.RequestStoreAsync(AppointmentStoreAccessType.AllCalendarsReadOnly);
            appointment.Subject = sch.Subject;
            appointment.StartTime = sch.StartTime;
            appointment.Duration = sch.Duration;
            appointment.Location = sch.Location;
            appointment.Reminder = sch.Reminder;
            return await asr.ShowAddAppointmentAsync(appointment, new Rect());
        }

        public async Task<string> Edit(string localId, ScheduleModel sch)
        {
            Appointment appointment = new Appointment();
            var amfu = AppointmentManager.GetForUser(User.GetDefault());
            var asr = await amfu.RequestStoreAsync(AppointmentStoreAccessType.AllCalendarsReadOnly);
            appointment.Subject = sch.Subject;
            appointment.StartTime = sch.StartTime;
            appointment.Duration = sch.Duration;
            appointment.Location = sch.Location;
            appointment.Reminder = sch.Reminder;
            return await asr.ShowReplaceAppointmentAsync(localId, appointment, new Rect());
        }

        public async Task<bool> Remove(string LocalId)
        {
            if(string.IsNullOrEmpty(LocalId)) return false;
            var amfu = AppointmentManager.GetForUser(User.GetDefault());
            var asr = await amfu.RequestStoreAsync(AppointmentStoreAccessType.AllCalendarsReadOnly);
            return await asr.ShowRemoveAppointmentAsync(LocalId, new Rect());
        }
    }
}
