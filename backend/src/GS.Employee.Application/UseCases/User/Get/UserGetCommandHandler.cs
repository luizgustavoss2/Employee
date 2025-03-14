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
        logger.LogInformation("Iniciando listagem de usu�rios");

        try
        {
            var userPersistence = await userRepository.GetAsync<UserPersistence>();

            if (userPersistence == null || !userPersistence.Any())
            {
                logger.LogWarning("Nenhum usu�rio encontrado");
                return response;
            }

            var user = userPersistence.Select<UserPersistence, User>(x => x).OrderBy(x => x.FirstName);
            response.User = user;

            logger.LogInformation("Listagem de usu�rios conclu�da com sucesso. Total de usu�rios: {TotalUsers}", user.Count());
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao listar usu�rios");
            throw; 
        }
    }
}