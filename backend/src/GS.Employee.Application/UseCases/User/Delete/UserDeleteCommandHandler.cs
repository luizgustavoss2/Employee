using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using GS.Employee.Domain.Entities;
using GS.Employee.Application.Notifications;
using GS.Employee.Domain.Interfaces.Repository;

namespace GS.Employee.Application.UseCases;

public class UserDeleteCommandHandler(
    IRepository<User> userRepository,
    IRepository<User> genericRepository,
    ILogger<UserDeleteCommandHandler> logger) : IRequestHandler<UserDeleteCommandRequest, UserDeleteCommandResponse>
{
    public async Task<UserDeleteCommandResponse> Handle(UserDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new UserDeleteCommandResponse();
        logger.LogInformation("Iniciando exclus�o do usu�rio com ID: {UserId}", request.Id);

        try
        {
            var valid = RequestBase<UserDeleteCommandRequest>.ValidateRequest(request, response);
            if (!valid)
            {
                logger.LogWarning("Requisi��o inv�lida para o usu�rio com ID: {UserId}", request.Id);
                return response;
            }

            var user = await genericRepository.GetAsync<User>(request.Id);

            if (user is null)
            {
                logger.LogWarning("Usu�rio n�o encontrado. ID: {UserId}", request.Id);
                response.AddNotification("Id", "User not found!", ErrorCode.NotFound);
                return response;
            }

            _ = await userRepository.DeleteAsync(request.Id);
            logger.LogInformation("Usu�rio exclu�do com sucesso. ID: {UserId}", request.Id);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao excluir usu�rio com ID: {UserId}", request.Id);
            throw; 
        }
    }
}