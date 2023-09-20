using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TyranoCupUwpApp.Shared;
using TyranoCupUwpApp.Shared.api;

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
            string res = await _openAIFormation.FormatTextToJson("明日の18時から21時まで最寄りのコンビニでバイト", apiKeyManagement.OpenAIApiKey);
            Assert.IsNotNull(res);
            TestContext.WriteLine("response:" + res);
        }
    }
}
