using System.Threading.Tasks;
using TyranoCupUwpApp.Shared.Models;

namespace TyranoCupUwpApp.Shared.api
{
    public interface IAccessDb
    {
        Task InitializeDatabase();
        void Create(SaveAudioModel model);
        SaveAudioModel Read(string appointmentId);
        void Delete(string appointmentId);
        void Update(SaveAudioModel model);
    }
}
