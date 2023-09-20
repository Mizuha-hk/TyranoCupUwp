using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.IO;
using System.Threading.Tasks;
using TyranoCupUwpApp.Shared.api;
using Windows.ApplicationModel;
using Windows.Storage;

namespace TyranoCupUwpApp.Shared
{
    public class VoiceRecognition : IVoiceRecognition {
        public async Task<string> VoiceRecognitionFromWavFile(string wavFile, string language)
        {
            ApiKeyManagement apiKeyManagement = ApiKeyManagement.GetInstance();
            await apiKeyManagement.Initialize();
            var stopRecognitionTaskCompletionSource = new TaskCompletionSource<int>(
                TaskCreationOptions.RunContinuationsAsynchronously);
            if(string.IsNullOrEmpty(apiKeyManagement.SpeechApiKey) || string.IsNullOrEmpty(language))
            {
                throw new Exception();
            }
            StorageFolder storageFolder = Package.Current.InstalledLocation;
            StorageFile file = await storageFolder.GetFileAsync(wavFile);
            if(file != null)
            {
                var config = SpeechConfig.FromSubscription(apiKeyManagement.SpeechApiKey, "japanwest");
                config.SpeechRecognitionLanguage = language;
                using (var audioInput = AudioConfig.FromWavFileInput(file.Path))
                {
                    using (var recognizer = new SpeechRecognizer(config, audioInput))
                    {
                        string result = "";
                        recognizer.Recognized += (s, e) =>
                        {
                            if (e.Result.Reason == ResultReason.RecognizedSpeech)
                            {
                                result = e.Result.Text;

                            }
                            else if (e.Result.Reason == ResultReason.NoMatch)
                            {
                                result = $"NOMATCH: Speech could not be recognized.";
                            }
                        };
                        recognizer.SessionStopped += (s, e) =>
                        {
                            stopRecognitionTaskCompletionSource.TrySetResult(0);
                        };
                        await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
                        await stopRecognitionTaskCompletionSource.Task.ConfigureAwait(false);
                        await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
                        return result;
                    }
                }
            }
            else {
                throw new FileNotFoundException();
            }
        }
    }
}
