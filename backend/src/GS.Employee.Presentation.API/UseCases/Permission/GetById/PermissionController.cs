using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using GS.Employee.Application.UseCases;

namespace GS.Employee.Presentation.API.UseCases.Permission.GetById
{
    [Route("v{ver:apiVersion}/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly GetByIdPermissionPresenter _getByIdPermissionPresenter;
        public PermissionController(IMediator mediator, GetByIdPermissionPresenter getByIdPermissionPresenter)
        {
            _mediator = mediator;
            _getByIdPermissionPresenter = getByIdPermissionPresenter; 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var request = new PermissionGetByIdCommandRequest(id);
            var result = await _mediator.Send(request);
            return _getByIdPermissionPresenter.GetActionResult(result, result.Permission, HttpStatusCode.OK);
        }
    }
}
