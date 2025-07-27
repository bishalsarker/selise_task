using FluentValidation;
using MediatR;
using SeliseTaskManager.Application.Interfaces.Common;
using SeliseTaskManager.Domain.Team;
using System.ComponentModel.DataAnnotations;

namespace SeliseTaskManager.Application.Teams
{
    public class DeleteTeamCommand : IRequest<bool>
    {
        [Required]
        public Guid Id { get; set; }
    }

    public class DeleteTeamCommandValidator : AbstractValidator<DeleteTeamCommand>
    {
        public DeleteTeamCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotNull()
                .NotEmpty();
        }
    }

    public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand, bool>
    {
        private readonly IRepository<TeamEntity> _teamRepository;

        public DeleteTeamCommandHandler(IRepository<TeamEntity> teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<bool> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            var existingTeam =
                (await _teamRepository.Query(t => t.Id == request.Id))
                .Item1
                .FirstOrDefault();

            if (existingTeam != null)
            {
                _teamRepository.Delete(existingTeam);
                return await _teamRepository.SaveAsync();
            }

            return false;
        }
    }
}
