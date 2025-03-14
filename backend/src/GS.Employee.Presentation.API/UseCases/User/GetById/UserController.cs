using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using GS.Employee.Application.UseCases;
using GS.Employee.Presentation.API.UseCases.User.GetById;

namespace GS.Employee.Presentation.API.UseCases.User.GetById;

[Route("v{ver:apiVersion}/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
[ApiConventionType(typeof(DefaultApiConventions))]
[ApiController]
public class UserController(
    IMediator mediator,
    GetByIdUserPresenter getByIdUserPresenter,
    ILogger<UserController> logger) : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserGetByIdCommandResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid id)
    {
        logger.LogInformation("Iniciando busca do usuário com ID: {UserId}", id);

        try
        {
            var request = new UserGetByIdCommandRequest(id);
            var result = await mediator.Send(request);

            if (result.User != null)
            {
                logger.LogInformation("Usuário encontrado com ID: {UserId}", id);
            }
            else
            {
                logger.LogWarning("Usuário não encontrado com ID: {UserId}", id);
            }

            return getByIdUserPresenter.GetActionResult(result, result.User, HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar o usuário com ID: {UserId}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, "Ocorreu um erro interno no servidor.");
        }
    }
}