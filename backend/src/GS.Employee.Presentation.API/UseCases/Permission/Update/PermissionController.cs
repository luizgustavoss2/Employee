using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using GS.Employee.Application.ResponseError;
using GS.Employee.Application.UseCases;

namespace GS.Employee.Presentation.API.UseCases.Permission.Update
{
    [Route("v{ver:apiVersion}/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UpdatePermissionPresenter _updatePermissionPresenter;
        public PermissionController(IMediator mediator, UpdatePermissionPresenter updatePermissionPresenter)
        {
            _mediator = mediator;
            _updatePermissionPresenter = updatePermissionPresenter; 
        }

        [HttpPatch("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> Patch(int id,[FromBody]UpdatePermissionRequest updateRequest)
        {
            var request = new PermissionUpdateCommandRequest(
                id: id,
              description: updateRequest.Description
            );
            var result = await _mediator.Send(request);
            return _updatePermissionPresenter.GetActionResult(result, result.Id, HttpStatusCode.NoContent);
        }
    }
}
