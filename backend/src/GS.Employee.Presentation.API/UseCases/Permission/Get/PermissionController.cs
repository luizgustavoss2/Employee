using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using GS.Employee.Application.UseCases;

namespace GS.Employee.Presentation.API.UseCases.Permission.Get
{
    [Route("v{ver:apiVersion}/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly GetPermissionPresenter _getPermissionPresenter;
        public PermissionController(IMediator mediator, GetPermissionPresenter getPermissionPresenter)
        {
            _mediator = mediator;
            _getPermissionPresenter = getPermissionPresenter; 
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var request = new PermissionGetCommandRequest();
            var result = await _mediator.Send(request); 
            return _getPermissionPresenter.GetActionResult(result, result.Permission, HttpStatusCode.OK); 
        }
    }
}
