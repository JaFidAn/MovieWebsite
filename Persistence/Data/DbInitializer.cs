
namespace Persistence.Contexts.Data;

public class DbInitializer
{
    public static async Task SeedData(ApplicationDbContext context)
    {
        await context.SaveChangesAsync();
    }
}


