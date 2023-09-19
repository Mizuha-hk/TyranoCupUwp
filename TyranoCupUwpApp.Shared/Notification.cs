using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Notifications; // Notifications library
using TyranoCupUwpApp.Shared.api;

namespace TyranoCupUwpApp.Shared
{
    public class Notification: INotification
    {
        public Notification() { }

        public void Schedule(
            string text,
            DateTime deliveryTime)
        {
            new ToastContentBuilder()
                .AddText(text)
                .Schedule(deliveryTime);
        }
    }
}
