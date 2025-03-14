using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using GS.Employee.Application.ResponseError;
using GS.Employee.Application.UseCases;

namespace GS.Employee.Presentation.API.UseCases.Permission.Insert
{
    [Route("v{ver:apiVersion}/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly InsertPermissionPresenter _insertPermissionPresenter;
        public PermissionController(IMediator mediator, InsertPermissionPresenter insertPermissionPresenter)
        {
            _mediator = mediator;
            _insertPermissionPresenter = insertPermissionPresenter; 
        }

        [HttpPost()]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> Post(InsertPermissionRequest insertRequest)
        {
            var request = new  PermissionInsertCommandRequest(
              description: insertRequest.Description
            );
            var result = await _mediator.Send(request);
            return _insertPermissionPresenter.GetActionResult(result, result.Id, HttpStatusCode.Created);
        }
    }
}
