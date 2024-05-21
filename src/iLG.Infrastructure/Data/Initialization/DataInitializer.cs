using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace iLG.Infrastructure.Data.Initialization
{
    public static class DataInitializer
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
            await SeedHobbyCategoryAsync(context);
            await SeedHobbyAsync(context);
            await SeedPermissionAsync(context);
            await SeedRoleAsync(context);
            await SeedRolePermissionAsync(context);
            await SeedRelationshipAsync(context);
            await SeedCompanyAsync(context);
            await SeedJobAsync(context);
            await SeedUserAsync(context);
            await SeedUserInfoAsync(context);
            await SeedUserInfoHobbyAsync(context);
            await SeedUserRoleAsync(context);
            await SeedImageAsync(context);
        }

        private static async Task SeedHobbyCategoryAsync(ILGDbContext context)
        {
            if (!await context.HobbyCategories.AnyAsync())
            {
                await context.HobbyCategories.AddRangeAsync(InitialData.HobbyCategories);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedHobbyAsync(ILGDbContext context)
        {
            if (!await context.Hobbies.AnyAsync())
            {
                await context.Hobbies.AddRangeAsync(InitialData.Hobbies);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedPermissionAsync(ILGDbContext context)
        {
            if (!await context.Permissions.AnyAsync())
            {
                await context.Permissions.AddRangeAsync(InitialData.Permissions);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedRoleAsync(ILGDbContext context)
        {
            if (!await context.Roles.AnyAsync())
            {
                await context.Roles.AddRangeAsync(InitialData.Roles);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedRolePermissionAsync(ILGDbContext context)
        {
            if (!await context.RolePermissions.AnyAsync())
            {
                await context.RolePermissions.AddRangeAsync(InitialData.RolePermissions);
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

        private static async Task SeedUserInfoAsync(ILGDbContext context)
        {
            if (!await context.UserInfos.AnyAsync())
            {
                await context.UserInfos.AddRangeAsync(InitialData.UserInfos);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedUserInfoHobbyAsync(ILGDbContext context)
        {
            if (!await context.UserInfoHobby.AnyAsync())
            {
                await context.UserInfoHobby.AddRangeAsync(InitialData.UserInfoHobby);
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

        private static async Task SeedImageAsync(ILGDbContext context)
        {
            if (!await context.Images.AnyAsync())
            {
                await context.Images.AddRangeAsync(InitialData.Images);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedRelationshipAsync(ILGDbContext context)
        {
            if (!await context.Relationships.AnyAsync())
            {
                await context.Relationships.AddRangeAsync(InitialData.Relationships);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedCompanyAsync(ILGDbContext context)
        {
            if (!await context.Companies.AnyAsync())
            {
                await context.Companies.AddRangeAsync(InitialData.Companies);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedJobAsync(ILGDbContext context)
        {
            if (!await context.Jobs.AnyAsync())
            {
                await context.Jobs.AddRangeAsync(InitialData.Jobs);
                await context.SaveChangesAsync();
            }
        }
    }
}