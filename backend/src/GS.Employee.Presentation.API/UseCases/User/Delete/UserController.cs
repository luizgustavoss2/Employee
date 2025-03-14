using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using GS.Employee.Application.ResponseError;
using GS.Employee.Application.UseCases;

namespace GS.Employee.Presentation.API.UseCases.User.Delete;

[Route("v{ver:apiVersion}/[controller]")]
[ApiController]
public class UserController(
    IMediator mediator,
    DeleteUserPresenter deleteUserPresenter,
    ILogger<UserController> logger) : ControllerBase
{
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(Guid id)
    {
        logger.LogInformation("Iniciando exclusão do usuário com ID: {UserId}", id);

        try
        {
            var request = new UserDeleteCommandRequest(id: id);
            var result = await mediator.Send(request);

            logger.LogInformation("Usuário excluído com sucesso. ID: {UserId}", id);
            return deleteUserPresenter.GetActionResult(result, null, HttpStatusCode.NoContent);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao excluir usuário com ID: {UserId}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, "Ocorreu um erro interno no servidor.");
        }
    }
}