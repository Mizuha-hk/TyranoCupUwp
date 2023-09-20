using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            string res = await _openAIFormation.FormatTextToJson("2023年9月19日11時から2023年9月21日19時まで北九州市でハッカソンの予定があります。");
            Assert.IsNotNull(res);
            TestContext.WriteLine("response:" + res);
        }
    }
}
