using SeliseTaskManager.Domain.Team;
using SeliseTaskManager.Domain.User;

namespace SeliseTaskManager.Application.Teams
{
    public class TaskResponse
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public TaskStatus Status { get; set; } = default!;
        public DateTime DueDate { get; set; } = default!;
        public Guid AssignedToUserId { get; set; } = default!;
        public Guid CreatedByUserId { get; set; } = default!;
        public Guid TeamId { get; set; } = default!;
    }
}
