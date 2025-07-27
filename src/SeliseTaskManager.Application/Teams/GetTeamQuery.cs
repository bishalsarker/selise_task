using FluentValidation;
using Mapster;
using MediatR;
using SeliseTaskManager.Application.Interfaces.Common;
using SeliseTaskManager.Application.Interfaces.Models;
using SeliseTaskManager.Application.Models;
using SeliseTaskManager.Domain.Common;
using SeliseTaskManager.Domain.Team;
using System.Linq.Expressions;

namespace SeliseTaskManager.Application.Teams
{
    public class GetTeamQuery : IRequest<PaginationResponse<TeamResponse>>
    {
        public Guid? Id { get; set; }
        public PaginationFilter PaginationFilter { get; set; } = default!;
    }

    public class GetTeamQueryValidator : AbstractValidator<GetTeamQuery>
    {
        public GetTeamQueryValidator()
        {
            RuleFor(p => p.Id).NotNull();
        }
    }

    public class GetTeamQueryHandler : IRequestHandler<GetTeamQuery, PaginationResponse<TeamResponse>>
    {
        private readonly IRepository<TeamEntity> _teamRepository;

        public GetTeamQueryHandler(IRepository<TeamEntity> teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<PaginationResponse<TeamResponse>> Handle(GetTeamQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<TeamEntity, bool>> predicate = u => true;

            if (request.Id.HasValue)
            {
                predicate = predicate.And(t => t.Id == request.Id);
            }

            var results = await _teamRepository.Query(predicate, request.PaginationFilter);

            return new PaginationResponse<TeamResponse>(
                results.Item1.Adapt<List<TeamResponse>>(),
                results.Item2, 
                request.PaginationFilter.PageNumber, 
                request.PaginationFilter.PageSize);
        }
    }
}
