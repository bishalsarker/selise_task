using FlyerBuy.Domain.Entities;
using SeliseTaskManager.Domain.Task;

namespace SeliseTaskManager.Domain.Team
{
    public class TeamEntity : BaseEntity<Guid>, IEntity
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;

        public ICollection<TaskEntity> Tasks { get; set; } = default!;
    }
}
