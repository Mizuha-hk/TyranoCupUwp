using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TyranoCupUwpApp.Shared;
using TyranoCupUwpApp.Shared.api;

namespace TyranoCupUwpApp.Test
{
    [TestClass]
    public class RecordAudioTest
    {
        IRecord _record = new Record();
        [TestMethod]
        public async Task AudioTest()
        {
            await _record.StartRecording();
            Task.Delay(1000).Wait();
            var str = await _record.StopRecording();
            Assert.IsNotNull(str);
        }
    }
}
