using iLG.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace iLG.Infrastructure.Extentions
{
    public static class DatabaseExtentions
    {
        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ILGDbContext>();
            context.Database.MigrateAsync().GetAwaiter().GetResult();
            await SeedAsync(context);
        }

        private static async Task SeedAsync(ILGDbContext context)
        {
            await SeedRoleAsync(context);
        }

        private static async Task SeedRoleAsync(ILGDbContext context)
        {
            if (!await context.Roles.AnyAsync())
            {
                await context.Roles.AddRangeAsync(InitialData.Roles);
                await context.SaveChangesAsync();
            }
        }
    }
}