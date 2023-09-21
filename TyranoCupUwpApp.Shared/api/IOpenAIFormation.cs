using System.Threading.Tasks;
using TyranoCupUwpApp.Shared.Models;

namespace TyranoCupUwpApp.Shared.api
{
    public interface IOpenAIFormation
    {
        Task<ScheduleModel> FormatTextToJson(string text, string apiKey);
    }
}
