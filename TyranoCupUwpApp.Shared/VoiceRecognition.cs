using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TyranoCupUwpApp.Shared.api;
using Windows.ApplicationModel;
using Windows.Storage;

namespace TyranoCupUwpApp.Shared
{
    public class VoiceRecognition : IVoiceRecognition
    {
        private string SpeechApiKey { get; set; }
        public async Task<string> VoiceRecognitionFromWavFile(string wavFile, string language)
        {
            var stopRecognitionTaskCompletionSource = new TaskCompletionSource<int>(
                TaskCreationOptions.RunContinuationsAsynchronously);
            if(string.IsNullOrEmpty(SpeechApiKey) || string.IsNullOrEmpty(language))
            {
                throw new Exception();
            }
            StorageFolder storageFolder = Package.Current.InstalledLocation;
            StorageFile file = await storageFolder.GetFileAsync(wavFile);
            if(file != null)
            {
                var config = SpeechConfig.FromSubscription(SpeechApiKey, "japanwest");
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
                        recognizer.Canceled += (s, e) =>
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine($"CANCELED: Reason={e.Reason}");

                            if (e.Reason == CancellationReason.Error)
                            {
                                sb.AppendLine($"CANCELED: ErrorCode={e.ErrorCode}");
                                sb.AppendLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
                                sb.AppendLine($"CANCELED: Did you update the subscription info?");
                            }

                            result = sb.ToString();
                        };
                        recognizer.SessionStopped += (s, e) =>
                        {
                            stopRecognitionTaskCompletionSource.TrySetResult(0);
                        };
                        // Starts continuous recognition. Uses StopContinuousRecognitionAsync() to stop recognition.
                        await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
                        // Waits for completion.
                        await stopRecognitionTaskCompletionSource.Task.ConfigureAwait(false);
                        // Stops recognition.
                        await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
                        return result;
                    }
                }
            }
            else {
                throw new FileNotFoundException();
            }
        }

        public async Task GetAzureSpeechApiKey()
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Properties/local.settings.json"));
            string jsonstring =  await FileIO.ReadTextAsync(file);
            if(string.IsNullOrEmpty(jsonstring))
            {
                throw new Exception();
            }
            SpeechApiKey = JsonSerializer.Deserialize<ApiKey>(jsonstring).AzureSpeechApiKey;
        }
    }

    internal class ApiKey
    {
        public string AzureSpeechApiKey { get; set; }
    }
}
