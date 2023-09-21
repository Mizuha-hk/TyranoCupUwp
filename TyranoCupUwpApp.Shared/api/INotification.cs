using System;

namespace TyranoCupUwpApp.Shared.api
{
    public interface INotification
    {
        void Schedule(
            string tag,
            string text,
            string audioGuid,
            DateTime deliveryTime);

        void Remove(string tag);

        void Reschedule(
            string tag,
            string text,
            string audioGuid,
            DateTime? deliveryTime);
    }
}
