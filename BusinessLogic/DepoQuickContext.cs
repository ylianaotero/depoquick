using Microsoft.EntityFrameworkCore;
using DepoQuick.Domain;

public class DepoQuickContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<Deposit> Deposits { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=DepoQuickDb;Trusted_Connection=True;");
    }
}