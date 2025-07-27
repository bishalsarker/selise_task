using FluentValidation;
using MediatR;
using SeliseTaskManager.Application.Interfaces.Common;
using SeliseTaskManager.Domain.Task;
using System.ComponentModel.DataAnnotations;

namespace SeliseTaskManager.Application.tasks
{
    public class DeleteTaskCommand : IRequest<bool>
    {
        [Required]
        public Guid Id { get; set; }
    }

    public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
    {
        public DeleteTaskCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotNull()
                .NotEmpty();
        }
    }

    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly IRepository<TaskEntity> _taskRepository;

        public DeleteTaskCommandHandler(IRepository<TaskEntity> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var existingtask =
                (await _taskRepository.Query(t => t.Id == request.Id))
                .Item1
                .FirstOrDefault();

            if (existingtask != null)
            {
                _taskRepository.Delete(existingtask);
                return await _taskRepository.SaveAsync();
            }

            return false;
        }
    }
}
