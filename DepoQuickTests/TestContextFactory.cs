using BusinessLogic;
using Microsoft.EntityFrameworkCore;


namespace DepoQuickTests;

public class TestContextFactory
{
    public static DepoQuickContext CreateContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DepoQuickContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());

        var context = new DepoQuickContext(optionsBuilder.Options, true);
        context.Database.EnsureCreated();
        
        return context;
        
    }
}