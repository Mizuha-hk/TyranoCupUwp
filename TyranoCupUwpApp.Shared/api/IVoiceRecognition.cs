using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyranoCupUwpApp.Shared.api
{
    public interface IVoiceRecognition
    {
        Task<string> VoiceRecognitionFromWavFile(string wavFile, string language);
        Task GetAzureSpeechApiKey();
    }
}
