using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using GS.Employee.Application.ResponseError;
using GS.Employee.Application.UseCases;

namespace GS.Employee.Presentation.API.UseCases.Permission.Delete
{
    [Route("v{ver:apiVersion}/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly DeletePermissionPresenter _deletePermissionPresenter;
        public PermissionController(IMediator mediator, DeletePermissionPresenter deletePermissionPresenter)
        {
            _mediator = mediator;
            _deletePermissionPresenter = deletePermissionPresenter; 
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = new PermissionDeleteCommandRequest(id: id);
            var result = await _mediator.Send(request);
            return _deletePermissionPresenter.GetActionResult(result, null, HttpStatusCode.NoContent);
        }
    }
}
