using FlyerBuy.Domain.Entities;
using SeliseTaskManager.Domain.Team;
using SeliseTaskManager.Domain.User;

namespace SeliseTaskManager.Domain.Task
{
    public class TaskEntity : BaseEntity<Guid>, IEntity
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public TaskStatus Status { get; set; } = default!;
        public DateTime DueDate { get; set; } = default!;

        public Guid AssignedToUserId { get; set; } = default!;
        public UserEntity AssignedToUser { get; set; } = default!;

        public Guid CreatedByUserId { get; set; } = default!;
        public UserEntity CreatedByUser { get; set; } = default!;

        public Guid TeamId { get; set; } = default!;
        public TeamEntity Team { get; set; } = default!;
    }
}
