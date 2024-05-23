using BusinessLogic;
using Microsoft.EntityFrameworkCore;


namespace DepoQuickTests;

public class TestContextFactory
{
    public static DepoQuickContext CreateContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DepoQuickContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
        return new DepoQuickContext(optionsBuilder.Options);
        
    }
}