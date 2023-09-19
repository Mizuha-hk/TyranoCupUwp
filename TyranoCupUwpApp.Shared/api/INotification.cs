using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
