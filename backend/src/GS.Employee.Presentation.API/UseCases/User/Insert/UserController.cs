using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using GS.Employee.Application.ResponseError;
using GS.Employee.Application.UseCases;
using Pluggy.SDK.Model;
using System;

namespace GS.Employee.Presentation.API.UseCases.User.Insert;

[Route("v{ver:apiVersion}/[controller]")]
[ApiController]
public class UserController(
    IMediator mediator,
    InsertUserPresenter insertUserPresenter,
    ILogger<UserController> logger) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> Post(InsertUserRequest insertRequest)
    {
        logger.LogInformation("Iniciando inserção de usuário com email: {Email}", insertRequest.Email);

        try
        {
            var request = new UserInsertCommandRequest(
                firstName: insertRequest.FirstName,
                lastName: insertRequest.LastName,
                email: insertRequest.Email,
                document: insertRequest.Document,
                birthDate: insertRequest.BirthDate,
                managerId: insertRequest.ManagerId,
                password: insertRequest.Password,
                phones: insertRequest.Phones,
                permissions: insertRequest.Permissions,
                userInsertId: insertRequest.UserInsertId
            );

            var result = await mediator.Send(request);

            if (result?.Id != null)
            {
                logger.LogInformation("Usuário inserido com sucesso. ID: {UserId}", result.Id);
            }
            else
            {
                logger.LogWarning("Falha ao inserir usuário com email: {Email}", insertRequest.Email);
            }

            return insertUserPresenter.GetActionResult(result, result.Id, HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao inserir usuário com email: {Email}", insertRequest.Email);
            return StatusCode((int)HttpStatusCode.InternalServerError, "Ocorreu um erro interno no servidor.");
        }
    }
}