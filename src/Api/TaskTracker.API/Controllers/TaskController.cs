using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Features.TaskOperation.Commands;
using TaskTracker.Application.Features.TaskOperation.Queries;

namespace TaskTracker.API.Controllers
{
    public class TaskController : BaseController
    {
        private readonly IMediator _mediator;

        public TaskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("Index")]
        public async Task<ResultVM> GetGridData(GetTaskGridQuery query)
        {
            return await _mediator.Send(query);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("Upsert")]
        public async Task<ResultVM> Upsert([FromBody][Required] UpsertTaskCommand command)
        {
            return await _mediator.Send(command);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResultVM>> Get(int id)
        {
            var result = await _mediator.Send(new GetTaskDetailQuery { Id = id });
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("Delete")]
        public async Task<ResultVM> Delete([FromBody] DeleteTaskCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}