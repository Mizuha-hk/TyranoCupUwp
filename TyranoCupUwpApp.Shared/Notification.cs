using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Notifications; // Notifications library
using TyranoCupUwpApp.Shared.api;
using Windows.UI.Notifications;

namespace TyranoCupUwpApp.Shared
{
    public class Notification: INotification
    {
        private string _tag;
        public Notification() { }

        public void Schedule(
            string text,
            DateTime deliveryTime)
        {
            _tag = Guid.NewGuid().ToString();
            new ToastContentBuilder()
                .AddText(text)
                .Schedule(deliveryTime, toast =>
                {
                    toast.Tag = _tag;
                    toast.Group = "Tyranno";
                });
        }

        public void Remove() {
            ToastNotifierCompat notifier = ToastNotificationManagerCompat.CreateToastNotifier();
            IReadOnlyList<ScheduledToastNotification> scheduledToasts = notifier.GetScheduledToastNotifications();
            var toRemove = scheduledToasts.FirstOrDefault(i => i.Tag == _tag && i.Group == "Tyranno");
            if (toRemove != null)
            {
                notifier.RemoveFromSchedule(toRemove);
            }
        }
    }
}
