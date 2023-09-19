using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TyranoCupUwpApp.Shared.api;
using Windows.ApplicationModel;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;

namespace TyranoCupUwpApp.Shared
{
    internal class Record : IRecord
    {
        private MediaCapture _mediaCapture;
        private InMemoryRandomAccessStream _stream;
        public bool IsRecording { get; private set; }

        public async void StartRecording()
        {
            if (IsRecording)
            {
                throw new InvalidOperationException("Recording already in progress!");
            }
            MediaCaptureInitializationSettings settings =
              new MediaCaptureInitializationSettings
              {
                  StreamingCaptureMode = StreamingCaptureMode.Audio
              };
            _mediaCapture = new MediaCapture();
            await _mediaCapture.InitializeAsync(settings);
            await _mediaCapture.StartRecordToStreamAsync(
              MediaEncodingProfile.CreateWav(AudioEncodingQuality.Auto), _stream);
            IsRecording = true;
        }

        public async void StopRecording()
        {
            await _mediaCapture.StopRecordAsync();
            IsRecording = false;
            SaveAudioToFile();
        }

        private async void SaveAudioToFile()
        {
            
        }
    }
}
