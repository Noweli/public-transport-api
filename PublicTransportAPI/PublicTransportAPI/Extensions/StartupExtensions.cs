using Microsoft.EntityFrameworkCore;

namespace PublicTransportAPI.Extensions;

public static class StartupExtensions
{
    public static async Task<WebApplication> CreateDatabase<T>(this WebApplication webHost) where T : DbContext
    {
        await using var scope = webHost.Services.CreateAsyncScope();
        var services = scope.ServiceProvider;

        try
        {
            var db = services.GetRequiredService<T>();
            await db.Database.MigrateAsync();
        }
        catch (Exception e)
        {
            await Console.Out.WriteLineAsync($"Failed to migrate database with error message: {e.Message}");
        }

        return webHost;
    }
}