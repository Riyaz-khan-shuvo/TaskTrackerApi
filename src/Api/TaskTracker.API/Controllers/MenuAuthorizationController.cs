using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Features.MenuAuthorizationOperation.Command;
using TaskTracker.Application.Features.MenuAuthorizationOperation.Queries;

namespace TaskTracker.API.Controllers
{
    public class MenuAuthorizationController : BaseController
    {

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("GetRoleGridData")]
        public async Task<ResultVM> GetRoleGridData(GetRoleGridQuery query)
        {
            return await Mediator.Send(query);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("GetUserGridData")]
        public async Task<ResultVM> GetUserGridData(GetUserGridQuery query)
        {
            return await Mediator.Send(query);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("GetMenuAccessData")]
        public async Task<ResultVM> GetMenuAccessData([FromBody][Required] CommonVM vm)
        {
            var result = await Mediator.Send(new GetMenuAccessDataQuery(vm));
            return result;
        }

        [HttpGet("AssignedMenuList/{userId}")]
        public async Task<IActionResult> GetAssignedMenuList(Guid userId)
        {
            var query = new GetAssignedMenuListQuery { UserId = userId };
            var result = await Mediator.Send(query);

            if (result.Status == "Success")
                return Ok(result);
            else
                return BadRequest(result);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("UpsertRole")]
        public async Task<ResultVM> UpsertRole([FromBody][Required] UpsertRoleCommand role)
        {
            return await Mediator.Send(role);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("GetRoleList")]
        public async Task<ResultVM> GetRoleList([FromBody][Required] CommonVM dto)
        {
            var result = await Mediator.Send(
                new GetRoleListQuery(
                    dto,
                    new[] { "b.Id" },
                    new[] { dto.Id.ToString() }
                )
            );
            return result;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("UpsertRoleMenu")]
        public async Task<ResultVM> UpsertRoleMenu(UpsertRoleMenuCommand query)
        {
            return await Mediator.Send(query);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("UpsertUserMenu")]
        public async Task<ResultVM> UpsertUserMenu(UpsertUserMenuCommand query)
        {
            return await Mediator.Send(query);
        }
    }
}
