using Exercise_15.Data;
using Microsoft.EntityFrameworkCore;

namespace Exercise_15.Extensions
{
    public static class SeedDataAppBuilderExtention
    {
        public static async Task<IApplicationBuilder> SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var _context = provider.GetRequiredService<ApplicationDbContext>();
                var config = provider.GetRequiredService<IConfiguration>();
                var service = provider.GetRequiredService<IServiceProvider>();
                var adminPW = config["AdminPW"];
                //_context.Database.EnsureDeleted();
                //_context.Database.Migrate();
                try
                {
                    await SeedData.Start(_context, service, adminPW);
                }
                catch (Exception ex)
                {
                    var log = provider.GetRequiredService<ILogger<Program>>();
                    log.LogError(String.Join(" ", ex.Message));
                }
            }
            return app;
        }
    }
}
