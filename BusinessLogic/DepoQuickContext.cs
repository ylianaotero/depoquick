using Microsoft.EntityFrameworkCore;
using DepoQuick.Domain;

public class DepoQuickContext : DbContext
{
    public DepoQuickContext(DbContextOptions<DepoQuickContext> options) : base(options)
    {
        this.Database.Migrate();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<Deposit> Deposits { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=localhost,1433;Database=depoquick;User Id=sa;Password=Passw1rd;");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasMany(u => u.Logs).WithOne().HasForeignKey(l => l.UserId);
        modelBuilder.Entity<Promotion>().OwnsOne(p => p.ValidityDate);
        modelBuilder.Entity<Reservation>().OwnsOne(r => r.Date);
        modelBuilder.Entity<User>()
            .ToTable("Users");

        modelBuilder.Entity<Administrator>()
            .ToTable("Administrators")
            .HasBaseType<User>();

        modelBuilder.Entity<Client>()
            .ToTable("Clients")
            .HasBaseType<User>();
    }
    
    
}