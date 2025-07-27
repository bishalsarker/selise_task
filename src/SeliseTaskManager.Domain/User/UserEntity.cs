using FlyerBuy.Domain.Entities;
using SeliseTaskManager.Domain.Task;
using SeliseTaskManager.Domain.Team;

namespace SeliseTaskManager.Domain.User
{
    public class UserEntity : BaseEntity<Guid>, IEntity
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public UserRoles Role { get; set; } = UserRoles.NotAssigned;

        // Navigation Properties
        public ICollection<TaskEntity> CreatedTasks { get; set; } = default!;
        public ICollection<TaskEntity> AssignedTasks { get; set; } = default!;
    }
}
