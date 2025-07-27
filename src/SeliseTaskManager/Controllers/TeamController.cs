using MediatR;
using Microsoft.AspNetCore.Mvc;
using SeliseTaskManager.Application.Teams;

namespace SeliseTaskManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TeamController> _logger;

        public TeamController(IMediator mediator, ILogger<TeamController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam(CreateTeamCommand createTeamCommand)
        {
            return Ok(await _mediator.Send(createTeamCommand));
        }
    }
}
