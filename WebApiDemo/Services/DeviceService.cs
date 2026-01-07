using WebApiDemo.Data;
using WebApiDemo.Services.Interfaces;

namespace WebApiDemo.Services
{
    public class DeviceService : BaseService, IDeviceService
    {
        public DeviceService(AppDbContext db) : base(db)
        {
        }
    }
}
