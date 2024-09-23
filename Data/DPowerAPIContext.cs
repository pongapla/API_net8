
using Microsoft.EntityFrameworkCore;
using DPowerAPI.models;

namespace DPowerAPI.Data
{
    public class DPowerAPIContext : DbContext
    {
        public DPowerAPIContext (DbContextOptions<DPowerAPIContext> options)
            : base(options)
        {
        }

        public DbSet<DPowerAPI.models.User> User { get; set; } = default!;
        public DbSet<DPowerAPI.models.BalanceCustomer> BalanceCustomer { get; set; } = default!;
        public DbSet<DPowerAPI.models.BalanceInventory> BalanceInventory { get; set; } = default!;
        public async Task CallspGetBalanceCustomer()
        {
            // เรียกใช้ Stored Procedure โดยไม่ต้องการข้อมูลผลลัพธ์
            await Database.ExecuteSqlRawAsync("EXEC GetBalanceCustomers");
        }
        public async Task CallspGetBalanceInventory()
        {
            await Database.ExecuteSqlRawAsync("EXEC GetBalanceInventory");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Set name tatble in models
            modelBuilder.Entity<DPowerAPI.models.BalanceCustomer>().ToTable("TEM_BALANCE_CUSTOMER");
            modelBuilder.Entity<DPowerAPI.models.BalanceInventory>().ToTable("TEM_INVENTORY");
        }
        
    }
}
