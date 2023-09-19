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
        public void Schedule(
            string tag,
            string text,
            DateTime deliveryTime)
        {
            new ToastContentBuilder()
                .AddText(text)
                .Schedule(deliveryTime, toast =>
                {
                    toast.Tag = tag;
                    toast.Group = "Tyranno";
                });
        }

        public void Remove(string tag) {
            ToastNotifierCompat notifier = ToastNotificationManagerCompat.CreateToastNotifier();
            IReadOnlyList<ScheduledToastNotification> scheduledToasts = notifier.GetScheduledToastNotifications();
            var toRemove = scheduledToasts.FirstOrDefault(i => i.Tag == tag && i.Group == "Tyranno");
            if (toRemove != null)
            {
                notifier.RemoveFromSchedule(toRemove);
            }
        }

        public void Reschedule(
            string tag,
            string text,
            DateTime? deliveryTime)
        {
            ToastNotifierCompat notifier = ToastNotificationManagerCompat.CreateToastNotifier();
            IReadOnlyList<ScheduledToastNotification> scheduledToasts = notifier.GetScheduledToastNotifications();
            var toRemove = scheduledToasts.FirstOrDefault(i => i.Tag == tag && i.Group == "Tyranno");
            var prevDeliveryTime = toRemove.DeliveryTime.DateTime;
            if (toRemove != null)
            {
                notifier.RemoveFromSchedule(toRemove);
            }
            Schedule(tag, text, deliveryTime ?? prevDeliveryTime);
        }
    }
}
