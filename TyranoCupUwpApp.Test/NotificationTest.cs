
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TyranoCupUwpApp.Shared;

namespace TyranoCupUwpApp.Test
{
    [TestClass]
    public class ToastNotificationTest
    {
        [TestMethod]
        public void NotificationTest()
        {
            Notification notification = new Notification();
            notification.Schedule("temp1", "hoge - 1", DateTime.Now.AddSeconds(5));
            notification.Schedule("temp2", "hoge - 2", DateTime.Now.AddSeconds(7));
            notification.Reschedule("temp2", "fuga - 2", DateTime.Now.AddSeconds(5));
            notification.Remove("temp1");
            Assert.IsInstanceOfType(notification, typeof(Notification));
        }
    }
}
