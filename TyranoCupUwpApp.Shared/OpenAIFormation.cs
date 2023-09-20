using System;
using System.Text.Json;
using System.Threading.Tasks;
using TyranoCupUwpApp.Shared.api;
using Windows.Storage;

namespace TyranoCupUwpApp.Shared
{
    public class OpenAIFormation : IOpenAIFormation
    {
        public string OpenAIApiKey { get; set; }

        private class ApiKey
        {
            public string ChatGptApiKey { get; set; }
        }

        public async Task GetOpenAIApiKey()
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Properties/local.settings.json"));
            string jsonstring = await FileIO.ReadTextAsync(file);
            if (string.IsNullOrEmpty(jsonstring))
            {
                throw new Exception();
            }
            OpenAIApiKey = JsonSerializer.Deserialize<ApiKey>(jsonstring).ChatGptApiKey;
        }

        public async Task<string> FormatTextToJson(string text)
        {
            var api = new OpenAI_API.OpenAIAPI(OpenAIApiKey);
            var chat = api.Chat.CreateConversation();

            string prompt =
                "# 命令書:\r\n" +
                "あなたは{プロの編集者}です。\r\n" +
                "以下の制約条件と入力文をもとに{jsonオブジェクト}を出力してください。\r\n" +
                "現在の日付は" + DateTime.Now + "です。\r\n" +
                "\r\n" +
                "# 制約条件:\r\n" +
                "•入力文から以下の4つの情報を取得する\r\n" +
                "1.予定の要約(文字列型)\r\n" +
                "2.場所(文字列型)\r\n" +
                "3.開始日時(数値型の配列)\r\n" +
                "4.終了日時(数値型の配列)\r\n" +
                "•この4つの情報はnullを許容する\r\n" +
                "•予定の要約はnullを許容しない。\r\n" +
                "•重要なキーワードを取り残さない。\r\n" +
                "•開始日時およびは終了日時は[西暦, 月, 日, 時, 分]の数値の配列で格納する。\r\n" +
                "•開始日時およびは終了日時が今日や明日などの場合、それに対応する西暦、日付、時刻の数値を格納する。\r\n" +
                "\r\n" +
                "# 入力文の例：\r\n" +
                "{2023年9月19日11時00分から2023年9月21日19時00分まで北九州市でハッカソンの予定があります。}\r\n" +
                "\r\n" +
                "# 出力文の例：\r\n" +
                "{ " +
                    "\"title\": \"ハッカソン\", " +
                    "\"location\": \"北九州市\", " +
                    "\"startTime\": [ 2023, 09, 19, 11, 00 ], " +
                    "\"endTime\": [ 2023, 09, 21, 19, 00 ] " +
                "}" +
                "\r\n" +
                "# 入力文：\r\n" +
                "{" + text + "}";
            chat.AppendUserInput(prompt);
            string response = await chat.GetResponseFromChatbotAsync();
            return response;
        }
    }
}
