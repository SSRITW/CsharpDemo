using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace WebApiDemo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<DeviceModel> Devices { get; set; }
        public virtual DbSet<AccountModel> Accounts { get; set; }
        
    }
}
