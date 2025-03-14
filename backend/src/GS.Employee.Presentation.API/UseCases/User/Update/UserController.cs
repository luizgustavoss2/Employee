using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using GS.Employee.Application.ResponseError;
using GS.Employee.Application.UseCases;

namespace GS.Employee.Presentation.API.UseCases.User.Update;

[Route("v{ver:apiVersion}/[controller]")]
[ApiController]
public class UserController(
    IMediator mediator,
    UpdateUserPresenter updateUserPresenter,
    ILogger<UserController> logger) : ControllerBase
{
    [HttpPatch("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> Patch(Guid id, [FromBody] UpdateUserRequest updateRequest)
    {
        logger.LogInformation("Iniciando atualização do usuário com ID: {UserId}", id);

        try
        {
            var request = new UserUpdateCommandRequest(
                id: id,
                firstName: updateRequest.FirstName,
                lastName: updateRequest.LastName,
                email: updateRequest.Email,
                document: updateRequest.Document,
                birthDate: updateRequest.BirthDate,
                managerId: updateRequest.ManagerId,
                password: updateRequest.Password,
                phones: updateRequest.Phones,
                permissions: updateRequest.Permissions,
                userInsertId: updateRequest.UserInsertId
            );

            var result = await mediator.Send(request);

            if (result.Id != null)
            {
                logger.LogInformation("Usuário atualizado com sucesso. ID: {UserId}", result.Id);
            }
            else
            {
                logger.LogWarning("Falha ao atualizar usuário com ID: {UserId}", id);
            }

            return updateUserPresenter.GetActionResult(result, result.Id, HttpStatusCode.NoContent);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao atualizar usuário com ID: {UserId}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, "Ocorreu um erro interno no servidor.");
        }
    }
}