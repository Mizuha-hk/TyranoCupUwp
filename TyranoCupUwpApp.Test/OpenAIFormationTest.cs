using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TyranoCupUwpApp.Shared;
using TyranoCupUwpApp.Shared.api;
using TyranoCupUwpApp.Shared.Models;

namespace TyranoCupUwpApp.Test
{
    [TestClass]
    public class OpenAIFormationTest
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        IOpenAIFormation _openAIFormation;
        [TestMethod]
        public async Task FormationTest()
        {
            ApiKeyManagement apiKeyManagement = ApiKeyManagement.GetInstance();
            await apiKeyManagement.Initialize();
            _openAIFormation = new OpenAIFormation();
            ScheduleModel res = await _openAIFormation.FormatTextToJson("30日後に最寄りのコンビニでバイト", apiKeyManagement.OpenAIApiKey);
            Assert.IsNotNull(res);
            TestContext.WriteLine("response:" + res.Subject + "\r\n" + res.Location + "\r\n" + res.StartTime + "\r\n" + res.Duration);
        }
    }
}
