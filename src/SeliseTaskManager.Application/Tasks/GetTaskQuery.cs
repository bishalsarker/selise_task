using FluentValidation;
using Mapster;
using MediatR;
using SeliseTaskManager.Application.Interfaces.Common;
using SeliseTaskManager.Application.Interfaces.Models;
using SeliseTaskManager.Application.Models;
using SeliseTaskManager.Application.Teams;
using SeliseTaskManager.Domain.Common;
using SeliseTaskManager.Domain.Task;
using System.Linq.Expressions;
using TaskStatus = SeliseTaskManager.Domain.Task.TaskStatus;

namespace SeliseTaskManager.Application.tasks
{
    public class GetTaskQuery : IRequest<PaginationResponse<TaskResponse>>
    {
        public Guid? Id { get; set; }
        public TaskStatus? Status { get; set; }
        public Guid? AssignedTo { get; set; }
        public Guid? TeamId { get; set; }
        public DateTime? DueDate { get; set; }
        public PaginationFilter PaginationFilter { get; set; } = default!;
    }

    public class GetTaskQueryValidator : AbstractValidator<GetTaskQuery>
    {
        public GetTaskQueryValidator()
        {
            RuleFor(p => p.Id).NotNull();
        }
    }

    public class GetTaskQueryHandler : IRequestHandler<GetTaskQuery, PaginationResponse<TaskResponse>>
    {
        private readonly IRepository<TaskEntity> _taskRepository;

        public GetTaskQueryHandler(IRepository<TaskEntity> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<PaginationResponse<TaskResponse>> Handle(GetTaskQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<TaskEntity, bool>> predicate = u => true;

            if (request.Id.HasValue)
            {
                predicate = predicate.And(t => t.Id == request.Id);
            }

            if (request.Status.HasValue)
            {
                predicate = predicate.And(t => t.Status == request.Status);
            }

            if (request.AssignedTo.HasValue)
            {
                predicate = predicate.And(t => t.AssignedToUserId == request.AssignedTo);
            }

            if (request.TeamId.HasValue)
            {
                predicate = predicate.And(t => t.TeamId == request.TeamId);
            }

            if (request.DueDate.HasValue)
            {
                predicate = predicate.And(t => t.DueDate >= request.DueDate && t.DueDate <= request.DueDate);
            }

            var results = await _taskRepository.Query(predicate, request.PaginationFilter);

            return new PaginationResponse<TaskResponse>(
                results.Item1.Adapt<List<TaskResponse>>(),
                results.Item2, 
                request.PaginationFilter.PageNumber, 
                request.PaginationFilter.PageSize);
        }
    }
}
