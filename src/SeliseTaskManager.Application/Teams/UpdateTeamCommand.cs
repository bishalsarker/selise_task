using FluentValidation;
using MediatR;
using SeliseTaskManager.Application.Interfaces.Common;
using SeliseTaskManager.Domain.Team;
using System.ComponentModel.DataAnnotations;

namespace SeliseTaskManager.Application.Teams
{
    public class UpdateTeamCommand : IRequest<bool>
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }

    public class UpdateTeamCommandValidator : AbstractValidator<UpdateTeamCommand>
    {
        public UpdateTeamCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotNull()
                .NotEmpty();
        }
    }

    public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, bool>
    {
        private readonly IRepository<TeamEntity> _teamRepository;

        public UpdateTeamCommandHandler(IRepository<TeamEntity> teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<bool> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            var existingTeam =
                (await _teamRepository.Query(t => t.Id == request.Id))
                .Item1
                .FirstOrDefault();

            if (existingTeam != null)
            {
                existingTeam.Name = request.Name;
                existingTeam.Description = request.Description ?? string.Empty;

                _teamRepository.Update(existingTeam);
                return await _teamRepository.SaveAsync();
            }

            return false;
        }
    }
}
