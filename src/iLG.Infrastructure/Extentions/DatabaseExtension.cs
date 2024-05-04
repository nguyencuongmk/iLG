using iLG.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace iLG.Infrastructure.Extentions
{
    public static class DatabaseExtentions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ILGDbContext>();
            context.Database.MigrateAsync().GetAwaiter().GetResult();
            await SeedAsync(context);
        }

        private static async Task SeedAsync(ILGDbContext context)
        {
            await SeedRoleAsync(context);
            await SeedUserAsync(context);
            await SeedUserRoleAsync(context);
        }

        private static async Task SeedRoleAsync(ILGDbContext context)
        {
            if (!await context.Roles.AnyAsync())
            {
                await context.Roles.AddRangeAsync(InitialData.Roles);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedUserAsync(ILGDbContext context)
        {
            if (!await context.Users.AnyAsync())
            {
                await context.Users.AddRangeAsync(InitialData.Users);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedUserRoleAsync(ILGDbContext context)
        {
            if (!await context.UserRoles.AnyAsync())
            {
                await context.UserRoles.AddRangeAsync(InitialData.UserRoles);
                await context.SaveChangesAsync();
            }
        }
    }
}