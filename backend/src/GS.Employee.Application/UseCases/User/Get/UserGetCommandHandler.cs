using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GS.Employee.Domain.Entities;
using GS.Employee.Domain.Interfaces.Repository;
using GS.Employee.Infra.Data.Entities;
using System;

namespace GS.Employee.Application.UseCases;

public class UserGetCommandHandler(
    IRepository<User> userRepository,
    ILogger<UserGetCommandHandler> logger) : IRequestHandler<UserGetCommandRequest, UserGetCommandResponse>
{
    public async Task<UserGetCommandResponse> Handle(UserGetCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new UserGetCommandResponse();
        logger.LogInformation("Iniciando listagem de usuários");

        try
        {
            var userPersistence = await userRepository.GetAsync<UserPersistence>();

            if (userPersistence == null || !userPersistence.Any())
            {
                logger.LogWarning("Nenhum usuário encontrado");
                return response;
            }

            var user = userPersistence.Select<UserPersistence, User>(x => x).OrderBy(x => x.FirstName);
            response.User = user;

            logger.LogInformation("Listagem de usuários concluída com sucesso. Total de usuários: {TotalUsers}", user.Count());
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao listar usuários");
            throw; 
        }
    }
}