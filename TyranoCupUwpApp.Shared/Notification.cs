using Microsoft.Toolkit.Uwp.Notifications; // Notifications library
using System;
using System.Collections.Generic;
using System.Linq;
using TyranoCupUwpApp.Shared.api;
using Windows.Foundation.Metadata;
using Windows.System.Profile;
using Windows.UI.Notifications;

namespace TyranoCupUwpApp.Shared
{
    public class Notification : INotification
    {
        public void Schedule(
            string tag,
            string text,
            string audioGuid,
            DateTime deliveryTime)
        {
            var contentBuilder = new ToastContentBuilder();
            bool supportsCustomAudio = true;

            // If we're running on Desktop before Version 1511, do NOT include custom audio
            // since it was not supported until Version 1511, and would result in a silent toast.
            if (AnalyticsInfo.VersionInfo.DeviceFamily.Equals("Windows.Desktop")
                && !ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 2))
            {
                supportsCustomAudio = false;
            }

            if (supportsCustomAudio && audioGuid.Length != 0)
            {
                contentBuilder.AddAudio(new Uri("ms-appx:///" + audioGuid + ".wav"));
            }

            contentBuilder
                .AddText(text)
                .Schedule(deliveryTime, toast =>
                {
                    toast.Tag = tag;
                    toast.Group = "Tyranno";
                });
        }

        public void Remove(string tag)
        {
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
            string audioGuid,
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
            Schedule(tag, text, audioGuid, deliveryTime ?? prevDeliveryTime);
        }
    }
}
