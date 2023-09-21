using System.Threading.Tasks;
using TyranoCupUwpApp.Shared.Models;

namespace TyranoCupUwpApp.Shared.api
{
    public interface ISchedule
    {
        Task<string> Add(ScheduleModel sch);
        Task<string> Edit(string LocalId, ScheduleModel sch);
        Task<bool> Remove(string LocalId);
    }
}
