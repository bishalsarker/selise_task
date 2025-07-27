using FluentValidation;
using MediatR;
using SeliseTaskManager.Application.Interfaces.Common;
using SeliseTaskManager.Domain.Task;
using System.ComponentModel.DataAnnotations;

namespace SeliseTaskManager.Application.Tasks
{
    public class CreateTaskCommand : IRequest<bool>
    {
        [Required]
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public Domain.Task.TaskStatus Status { get; set; } = Domain.Task.TaskStatus.ToDo;
        public Guid AssignedTo { get; set; }
        public Guid TeamId { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(p => p.Title)
                .NotNull()
                .NotEmpty();

            RuleFor(p => p.AssignedTo)
                .NotNull();

            RuleFor(p => p.TeamId)
                .NotNull();

            RuleFor(p => p.DueDate)
                .NotNull();
        }
    }

    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, bool>
    {
        private readonly IRepository<TaskEntity> _taskRepository;

        public CreateTaskCommandHandler(IRepository<TaskEntity> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<bool> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var newtaskRequest = new TaskEntity()
            {
                Title = request.Title,
                Description = request.Description ?? string.Empty,
                Status = request.Status,
                AssignedToUserId = request.AssignedTo,
                CreatedByUserId = new Guid(),
                TeamId = request.TeamId,
                DueDate = DateTime.UtcNow
            };

            await _taskRepository.AddAsync(newtaskRequest);
            return await _taskRepository.SaveAsync();
        }
    }
}
