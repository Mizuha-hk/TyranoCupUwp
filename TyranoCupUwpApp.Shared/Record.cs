using System;
using System.Threading.Tasks;
using TyranoCupUwpApp.Shared.api;
using Windows.ApplicationModel;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;

namespace TyranoCupUwpApp.Shared
{
    public class Record : IRecord
    {
        private MediaCapture _mediaCapture;
        private InMemoryRandomAccessStream _stream;
        public bool IsRecording { get; private set; }
        public string _fileName;

        public async Task StartRecording()
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
            _stream = new InMemoryRandomAccessStream();
            await _mediaCapture.StartRecordToStreamAsync(
              MediaEncodingProfile.CreateWav(AudioEncodingQuality.Auto), _stream);
            IsRecording = true;
        }

        public async Task<string> StopRecording()
        {
            await _mediaCapture.StopRecordAsync();
            IsRecording = false;
            string guid = Guid.NewGuid().ToString();
            await SaveAudioToFile(guid);
            return guid;
        }

        private async Task SaveAudioToFile(string guid)
        {
            IRandomAccessStream audioStream = _stream.CloneStream();
            StorageFolder storageFolder = Package.Current.InstalledLocation;
            string desiredName = guid + ".wav";
            StorageFile storageFile = await storageFolder.CreateFileAsync(
              desiredName, CreationCollisionOption.GenerateUniqueName);
            this._fileName = storageFile.Name;
            using (IRandomAccessStream fileStream =
              await storageFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                await RandomAccessStream.CopyAndCloseAsync(
                  audioStream.GetInputStreamAt(0), fileStream.GetOutputStreamAt(0));
                await audioStream.FlushAsync();
                audioStream.Dispose();
            }
        }
    }
}
