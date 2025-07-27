using FlyerBuy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SeliseTaskManager.Domain.Task;
using SeliseTaskManager.Domain.Team;
using SeliseTaskManager.Domain.User;

namespace SeliseTaskManager.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<TeamEntity> Teams { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskEntity>()
                .HasOne(o => o.AssignedToUser)
                .WithMany(c => c.AssignedTasks)
                .HasForeignKey(o => o.AssignedToUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskEntity>()
                .HasOne(o => o.CreatedByUser)
                .WithMany(c => c.CreatedTasks)
                .HasForeignKey(o => o.CreatedByUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskEntity>()
                .HasOne(o => o.Team)
                .WithMany(c => c.Tasks)
                .HasForeignKey(o => o.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            // var userId = _currentUserService.UserId;

            foreach (var entry in ChangeTracker.Entries<IEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOnUtc = now;
                    entry.Entity.CreatedBy = string.Empty;
                    entry.Entity.LastModifiedOnUtc = now;
                    entry.Entity.LastModifiedBy = string.Empty;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModifiedOnUtc = now;
                    entry.Entity.LastModifiedBy = string.Empty;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
