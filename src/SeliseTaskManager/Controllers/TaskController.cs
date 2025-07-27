using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeliseTaskManager.Application.Interfaces.Models;
using SeliseTaskManager.Application.tasks;
using SeliseTaskManager.Application.Tasks;

namespace SeliseTaskManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TaskController> _logger;

        public TaskController(IMediator mediator, ILogger<TaskController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "1,2,3")]
        public async Task<IActionResult> Get(
            Guid? id, int? status, Guid? assignedTo, Guid teamId, int pageNumber = 1, int pageSize = 20)
        {
            return Ok(await _mediator.Send(new GetTaskQuery()
            {
                Id = id,
                Status = (Domain.Task.TaskStatus?)status,
                AssignedTo = assignedTo,
                TeamId = teamId,
                PaginationFilter = new PaginationFilter()
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                }
            }));
        }

        [HttpPut]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Update(UpdateTaskCommand updateCommand)
        {
            return Ok(await _mediator.Send(updateCommand));
        }

        [HttpPost]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> Create(CreateTaskCommand createCommand)
        {
            return Ok(await _mediator.Send(createCommand));
        }

        [HttpDelete]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(DeleteTaskCommand deleteCommand)
        {
            return Ok(await _mediator.Send(deleteCommand));
        }
    }
}
