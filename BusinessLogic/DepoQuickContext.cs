using Microsoft.EntityFrameworkCore;
using DepoQuick.Domain;

public class DepoQuickContext : DbContext
{
    public bool UseInMemoryDatabase { get; set; }
    

    public DepoQuickContext(DbContextOptions<DepoQuickContext> options, bool useInMemoryDatabase) : base(options)
    {
        UseInMemoryDatabase = useInMemoryDatabase;
        if (!UseInMemoryDatabase)
        {
            this.Database.Migrate();   
        }
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Administrator> Administrators { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<Deposit> Deposits { get; set; }
    
    
    public DbSet<LogEntry> LogEntries { get; set; }
    public DbSet<Rating> Ratings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost,1433;Database=depoquick;User Id=sa;Password=Passw1rd;");

        }

        optionsBuilder.EnableDetailedErrors(true);
        optionsBuilder.EnableSensitiveDataLogging(true);
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
        modelBuilder.Entity<Deposit>().HasMany<Rating>(d=>d.Ratings);
        modelBuilder.Entity<Reservation>().HasOne<Rating>(r=>r.Rating);
    }
    
    
}