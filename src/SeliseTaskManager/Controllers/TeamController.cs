using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeliseTaskManager.Application.Interfaces.Models;
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

        [HttpGet]
        [Authorize(Roles = "1,2,3")]
        public async Task<IActionResult> Get(Guid? id, int pageNumber = 1, int pageSize = 20)
        {
            return Ok(await _mediator.Send(new GetTeamQuery()
            {
                Id = id,
                PaginationFilter = new PaginationFilter()
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                }
            }));
        }

        [HttpPut]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Update(UpdateTeamCommand updateCommand)
        {
            return Ok(await _mediator.Send(updateCommand));
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create(CreateTeamCommand createCommand)
        {
            return Ok(await _mediator.Send(createCommand));
        }

        [HttpDelete]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(DeleteTeamCommand deleteCommand)
        {
            return Ok(await _mediator.Send(deleteCommand));
        }
    }
}
