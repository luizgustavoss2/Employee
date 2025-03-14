using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using GS.Employee.Application.UseCases;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

namespace GS.Employee.Presentation.API.UseCases.AuthenticationClient.Authentication;

[Route("v{ver:apiVersion}/[controller]")]
[ApiController]
public class AuthenticationController(
    IMediator mediator,
    AuthenticationPresenter authenticationPresenter,
    ILogger<AuthenticationController> logger) : ControllerBase 
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AuthenticationRequest request)
    {
        try
        {
            logger.LogInformation("Iniciando autenticação para o email: {Email}", request.Email);

            var command = new AuthenticationCommandRequest(request.Email, request.Password);
            var result = await mediator.Send(command);

            if (result.AuthenticationClient != null)
            {
                logger.LogInformation("Autenticação bem-sucedida para o email: {Email}", request.Email);
            }
            else
            {
                logger.LogWarning("Autenticação falhou para o email: {Email}", request.Email); 
            }

            return authenticationPresenter.GetActionResult(result, result.AuthenticationClient, HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao autenticar o email: {Email}", request.Email); 
            return StatusCode((int)HttpStatusCode.InternalServerError, "Ocorreu um erro interno no servidor.");
        }
    }
}