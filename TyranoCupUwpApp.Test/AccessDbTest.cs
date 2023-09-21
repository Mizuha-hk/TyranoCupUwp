using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using TyranoCupUwpApp.Shared;
using TyranoCupUwpApp.Shared.api;
using TyranoCupUwpApp.Shared.Models;

namespace TyranoCupUwpApp.Test
{
    [TestClass]
    public class AccessDbTest
    {
        IAccessDb _accessDb;

        [TestMethod]
        public async Task DbCrud()
        {
            _accessDb = new AccessDb();

            await _accessDb.InitializeDatabase();

            string guid = Guid.NewGuid().ToString();

            _accessDb.Create(new SaveAudioModel { AppointmentId = guid, AudioId = "1" });

            var model = _accessDb.Read(guid);
            Assert.IsNotNull(model);
            Assert.AreEqual(model.AppointmentId, guid);
            Assert.AreEqual(model.AudioId, "1");

            _accessDb.Update(new SaveAudioModel { AppointmentId = guid, AudioId = "2" });
            model = _accessDb.Read(guid);
            Assert.IsNotNull(model);
            Assert.AreEqual(model.AppointmentId, guid);
            Assert.AreEqual(model.AudioId, "2");

            _accessDb.Delete(guid);
            model = _accessDb.Read(guid);
            Assert.IsNull(model);
        }
    }
}
