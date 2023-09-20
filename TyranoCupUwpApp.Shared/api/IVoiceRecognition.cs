using System.Threading.Tasks;

namespace TyranoCupUwpApp.Shared.api
{
    public interface IVoiceRecognition
    {
        Task<string> VoiceRecognitionFromWavFile(string wavFile, string language);
    }
}
