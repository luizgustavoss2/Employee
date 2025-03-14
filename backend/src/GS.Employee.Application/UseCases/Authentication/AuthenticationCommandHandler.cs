using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using GS.Employee.Application.Notifications;
using GS.Employee.Domain.Interfaces.Repository;
using GS.Employee.Infra.CrossCutting.Util;

namespace GS.Employee.Application.UseCases;

public class AuthenticationCommandHandler(
    IRepositoryUser userRepository,
    ILogger<AuthenticationCommandHandler> logger) : IRequestHandler<AuthenticationCommandRequest, AuthenticationCommandResponse>
{
    private readonly string _tokenConfiguration = "gs";

    public async Task<AuthenticationCommandResponse> Handle(AuthenticationCommandRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando autentica��o para o email: {Email}", request.Email);

        var response = new AuthenticationCommandResponse();
        var valid = RequestBase<AuthenticationCommandRequest>.ValidateRequest(request, response);

        if (!valid)
        {
            logger.LogWarning("Requisi��o inv�lida para o email: {Email}", request.Email);
            return response;
        }

        var user = await userRepository.GetByEmail(request.Email);

        if (user == null)
        {
            logger.LogWarning("Usu�rio n�o encontrado para o email: {Email}", request.Email);
            response.AddNotification("User", "Usu�rio n�o encontrado!", ErrorCode.NotFound);
            return response;
        }

        if (MD5Hash.Decrypt(user.Password).Replace(_tokenConfiguration + "-", "") != request.Password)
        {
            logger.LogWarning("Senha inv�lida para o email: {Email}", request.Email);
            response.AddNotification("User", "Senha inv�lida!", ErrorCode.NotFound);
            return response;
        }

        logger.LogInformation("Autentica��o bem-sucedida para o email: {Email}", request.Email);
        response.AuthenticationClient = new AuthenticationResponse() { UserId = user.Id };

        return response;
    }
}