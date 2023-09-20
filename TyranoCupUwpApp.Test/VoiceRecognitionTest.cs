using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TyranoCupUwpApp.Shared;
using TyranoCupUwpApp.Shared.api;

namespace TyranoCupUwpApp.Test
{
    [TestClass]
    public class VoiceRecognitionTest
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        IVoiceRecognition _voiceRecognition;
        [TestMethod]
        public async Task RecognitionTest()
        {
            _voiceRecognition = new VoiceRecognition();
            await _voiceRecognition.GetAzureSpeechApiKey();
            string str = await _voiceRecognition.VoiceRecognitionFromWavFile("test.wav", "ja-JP");
            Assert.IsNotNull(str);
            TestContext.WriteLine("output:" + str);
        }
    }
}
