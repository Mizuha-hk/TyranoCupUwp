using System;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace TyranoCupUwpApp.Shared
{
    public sealed class ApiKeyManagement
    {
        private static ApiKeyManagement _apiKeyManagement;
        private string _jsonstring = "";
        private ApiKeyManagement() { }

        public static ApiKeyManagement GetInstance() {
            if (_apiKeyManagement == null)
            {
                _apiKeyManagement = new ApiKeyManagement();
            }
            return _apiKeyManagement;
        }

        private class ApiKey
        {
            public string AzureSpeechApiKey { get; set; }
            public string ChatGptApiKey { get; set; }
        }

        public async Task Initialize()
        {
            if (string.IsNullOrEmpty(_jsonstring))
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Properties/local.settings.json"));
                _jsonstring = await FileIO.ReadTextAsync(file);
                if (string.IsNullOrEmpty(_jsonstring))
                {
                    throw new Exception();
                }
                SpeechApiKey = JsonSerializer.Deserialize<ApiKey>(_jsonstring).AzureSpeechApiKey;
                OpenAIApiKey = JsonSerializer.Deserialize<ApiKey>(_jsonstring).ChatGptApiKey;
            }
        }

        public string OpenAIApiKey { get; private set; }
        public string SpeechApiKey { get; private set; }
    }
}
