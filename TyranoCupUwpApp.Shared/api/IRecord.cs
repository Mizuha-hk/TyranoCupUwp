using System.Threading.Tasks;

namespace TyranoCupUwpApp.Shared.api
{
    public interface IRecord
    {
        bool IsRecording { get; }
        Task StartRecording();
        Task<string> StopRecording();
    }
}
