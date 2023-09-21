using System;
using System.Text.Json;
using System.Threading.Tasks;
using TyranoCupUwpApp.Shared.api;
using TyranoCupUwpApp.Shared.Models;

namespace TyranoCupUwpApp.Shared
{
    public class OpenAIFormation : IOpenAIFormation
    {
        private class Data
        {
            public string Title { get; set; }
            public int[] StartTime { get; set; }
            public int[] EndTime { get; set; }
            public string Location { get; set; }
        }
        public async Task<ScheduleModel> FormatTextToJson(string text, string apiKey)
        {
            var api = new OpenAI_API.OpenAIAPI(apiKey);
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
                    "\"Title\": \"ハッカソン\", " +
                    "\"Location\": \"北九州市\", " +
                    "\"StartTime\": [ 2023, 9, 19, 11, 0 ], " +
                    "\"EndTime\": [ 2023, 9, 21, 19, 0 ] " +
                "}" +
                "\r\n" +
                "# 入力文：\r\n" +
                "{" + text + "}";
            chat.AppendUserInput(prompt);
            string response = await chat.GetResponseFromChatbotAsync();
            return Deserialize(response);
        }

        private ScheduleModel Deserialize(string response)
        {
            var data = JsonSerializer.Deserialize<Data>(response);

            ScheduleModel scheduleModel = ScheduleModel.GetInstance();
            scheduleModel.Subject = "" + data.Title;
            scheduleModel.Location = "" + data.Location;
            int[] startTime = data.StartTime;
            int[] endTime = data.EndTime;
            if (startTime != null) scheduleModel.StartTime = new DateTime(startTime[0], startTime[1], startTime[2], startTime[3], startTime[4], 0);
            if (endTime != null) scheduleModel.Duration = (new DateTime(endTime[0], endTime[1], endTime[2], endTime[3], endTime[4], 0) - scheduleModel.StartTime);
            return scheduleModel;
        }
    }
}
