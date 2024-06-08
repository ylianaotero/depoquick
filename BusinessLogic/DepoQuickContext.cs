using DepoQuick.Domain;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic;

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

    public DepoQuickContext()
    {
        UseInMemoryDatabase = false;
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
    
    public DbSet<Payment> Payments { get; set; }
    
    public DbSet<LogEntry> LogEntries { get; set; }
    public DbSet<Rating> Ratings { get; set; }

    public DbSet<Notification> Notifications { get; set; }
    
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
        modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<User>().HasMany(u => u.Logs).WithOne().HasForeignKey(l => l.UserId);
        modelBuilder.Entity<Promotion>().OwnsOne(p => p.ValidityDate);
        modelBuilder.Entity<Reservation>().OwnsOne(r => r.Date);
        modelBuilder.Entity<Reservation>().Property(r => r.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<User>()
            .ToTable("Users");

        modelBuilder.Entity<Administrator>()
            .ToTable("Administrators")
            .HasBaseType<User>();

        modelBuilder.Entity<Client>()
            .ToTable("Clients")
            .HasBaseType<User>()
            .HasMany(u => u.Notifications);
        
        modelBuilder.Entity<Deposit>().HasMany<Rating>(d=>d.Ratings);
        modelBuilder.Entity<Deposit>().Property(d => d.Id).ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Rating>().HasOne<Reservation>(r=>r.Reservation);
        
        modelBuilder.Entity<Payment>().HasOne<Reservation>(p=>p.Reservation);

        modelBuilder.Entity<Notification>().HasOne<Client>(n => n.Client);
         
        modelBuilder.Entity<Deposit>().OwnsMany(d => d.AvailableDates);
        //  modelBuilder.Entity<Notification>().HasOne<Reservation>(p=>p.Reservation);
    }
    
    
}