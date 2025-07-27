using FluentValidation;
using MediatR;
using SeliseTaskManager.Application.Interfaces.Common;
using SeliseTaskManager.Domain.Team;
using System.ComponentModel.DataAnnotations;

namespace SeliseTaskManager.Application.Teams
{
    public class CreateTeamCommand : IRequest<bool>
    {
        [Required]
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }

    public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
    {
        public CreateTeamCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotNull()
                .NotEmpty();
        }
    }

    public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, bool>
    {
        private readonly IRepository<TeamEntity> _teamRepository;

        public CreateTeamCommandHandler(IRepository<TeamEntity> teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<bool> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            var newTeamRequest = new TeamEntity()
            {
                Name = request.Name,
                Description = request.Description ?? string.Empty
            };

            await _teamRepository.AddAsync(newTeamRequest);
            return await _teamRepository.SaveAsync();
        }
    }
}
