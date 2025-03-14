using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using GS.Employee.Application.UseCases;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace GS.Employee.Presentation.API.UseCases.User.Get;

[Route("v{ver:apiVersion}/[controller]")]
[ApiController]
public class UserController(
    IMediator mediator,
    GetUserPresenter getUserPresenter,
    ILogger<UserController> logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(UserGetCommandResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get()
    {
        logger.LogInformation("Iniciando listagem de usuários");

        try
        {
            var request = new UserGetCommandRequest();
            var result = await mediator.Send(request);

            if (result.User != null && result.User.Any())
            {
                logger.LogInformation("Listagem de usuários concluída com sucesso. Total de usuários: {TotalUsers}", result.User.Count());
            }
            else
            {
                logger.LogWarning("Nenhum usuário encontrado");
            }

            return getUserPresenter.GetActionResult(result, result.User, HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao listar usuários");
            return StatusCode((int)HttpStatusCode.InternalServerError, "Ocorreu um erro interno no servidor.");
        }
    }
}