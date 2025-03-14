using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using GS.Employee.Domain.Entities;
using GS.Employee.Domain.Interfaces.Repository;
using GS.Employee.Infra.CrossCutting.Util;

namespace GS.Employee.Application.UseCases;

public class UserGetByIdCommandHandler(
    IRepositoryUser userRepository,
    IRepositoryUserPhone userPhoneRepository,
    IRepositoryUserPermission userPermissionRepository,
    ILogger<UserGetByIdCommandHandler> logger) : IRequestHandler<UserGetByIdCommandRequest, UserGetByIdCommandResponse>
{
    private readonly string _tokenConfiguration = "gs";

    public async Task<UserGetByIdCommandResponse> Handle(UserGetByIdCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new UserGetByIdCommandResponse();
        logger.LogInformation("Iniciando busca do usuário com ID: {UserId}", request.Id);

        try
        {
            var valid = RequestBase<UserGetByIdCommandRequest>.ValidateRequest(request, response);
            if (!valid)
            {
                logger.LogWarning("Requisição inválida para o usuário com ID: {UserId}", request.Id);
                return response;
            }

            var user = await userRepository.GetByUserId(request.Id);

            if (user == null)
            {
                logger.LogWarning("Usuário não encontrado. ID: {UserId}", request.Id);
                return response;
            }

            user.PhoneNumbers = await userPhoneRepository.GetByUserId(request.Id);
            user.Permissions = await userPermissionRepository.GetByUserId(request.Id);

            user.Password = MD5Hash.Decrypt(user.Password).Replace(_tokenConfiguration + "-", "");

            response.User = user;
            logger.LogInformation("Usuário encontrado com sucesso. ID: {UserId}", request.Id);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar usuário com ID: {UserId}", request.Id);
            throw; 
        }
    }
}