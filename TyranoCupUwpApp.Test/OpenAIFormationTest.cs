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
            _openAIFormation = new OpenAIFormation();
            await _openAIFormation.GetOpenAIApiKey();
            string res = await _openAIFormation.FormatTextToJson("あした11時からバイトがあります。");
            Assert.IsNotNull(res);
            TestContext.WriteLine("response:" + res);
        }
    }
}
