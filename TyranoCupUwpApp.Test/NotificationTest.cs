
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
            notification.Schedule("hoge", DateTime.Now.AddSeconds(10));
            Assert.IsInstanceOfType(notification, typeof(Notification));
        }
    }
}
