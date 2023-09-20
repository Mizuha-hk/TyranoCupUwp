using System.Threading.Tasks;

namespace TyranoCupUwpApp.Shared.api
{
    public interface IOpenAIFormation
    {
        Task<string> FormatTextToJson(string text);
    }
}
