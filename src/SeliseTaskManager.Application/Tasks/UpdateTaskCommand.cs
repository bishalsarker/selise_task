using FluentValidation;
using MediatR;
using SeliseTaskManager.Application.Interfaces.Common;
using SeliseTaskManager.Domain.Task;
using System.ComponentModel.DataAnnotations;

namespace SeliseTaskManager.Application.Tasks
{
    public class UpdateTaskCommand : IRequest<bool>
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public Domain.Task.TaskStatus Status { get; set; } = Domain.Task.TaskStatus.ToDo;
        public Guid AssignedTo { get; set; }
        public Guid TeamId { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
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

    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, bool>
    {
        private readonly IRepository<TaskEntity> _TaskRepository;

        public UpdateTaskCommandHandler(IRepository<TaskEntity> TaskRepository)
        {
            _TaskRepository = TaskRepository;
        }

        public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var existingTask =
                (await _TaskRepository.Query(t => t.Id == request.Id))
                .Item1
                .FirstOrDefault();

            if (existingTask != null)
            {
                existingTask.Title = request.Title;
                existingTask.Status = request.Status;
                existingTask.DueDate = request.DueDate;
                existingTask.TeamId = request.TeamId;
                existingTask.AssignedToUserId = request.AssignedTo;
                existingTask.Description = request.Description ?? string.Empty;

                _TaskRepository.Update(existingTask);
                return await _TaskRepository.SaveAsync();
            }

            return false;
        }
    }
}
