using SeliseTaskManager.Domain.User;

namespace SeliseTaskManager.Infrastructure.Persistence
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                var user1 = new UserEntity { Id = Guid.NewGuid(), FullName = "Admin User", Email = "admin@demo.com", Password = "Admin123!", Role = UserRoles.Admin };
                var user2 = new UserEntity { Id = Guid.NewGuid(), FullName = "Manager User", Email = "manager@demo.com", Password = "Manger123!", Role = UserRoles.Manager };
                var user3 = new UserEntity { Id = Guid.NewGuid(), FullName = "Employee User", Email = "employee@demo.com", Password = "Employee123!", Role = UserRoles.Emplyee };

                await context.Users.AddRangeAsync(user1, user2, user3);
            }

            await context.SaveChangesAsync();
        }
    }
}
