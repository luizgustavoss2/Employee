using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GS.Employee.Application.Notifications;
using GS.Employee.Domain.Entities;
using GS.Employee.Domain.Interfaces.Repository;
using GS.Employee.Infra.CrossCutting.Util;

namespace GS.Employee.Application.UseCases;

public class UserUpdateCommandHandler(
    IRepositoryUser userRepository,
    IRepository<User> genericRepository,
    IRepositoryUserPhone userPhoneRepository,
    IRepositoryUserPermission userPermissionRepository,
    ILogger<UserUpdateCommandHandler> logger) : IRequestHandler<UserUpdateCommandRequest, UserUpdateCommandResponse>
{
    private readonly string _tokenConfiguration = "gs";

    public async Task<UserUpdateCommandResponse> Handle(UserUpdateCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new UserUpdateCommandResponse();
        logger.LogInformation("Iniciando atualização do usuário com ID: {UserId}", request.Id);

        try
        {
            var valid = RequestBase<UserUpdateCommandRequest>.ValidateRequest(request, response);
            if (!valid)
            {
                logger.LogWarning("Requisição inválida para o usuário com ID: {UserId}", request.Id);
                return response;
            }

            var user = await genericRepository.GetAsync<User>(request.Id);

            if (user is null)
            {
                logger.LogWarning("Usuário não encontrado. ID: {UserId}", request.Id);
                response.AddNotification("Id", "Usuário não encontrado!", ErrorCode.NotFound);
                return response;
            }

            if (!ValidateBirthDate(request.BirthDate))
            {
                logger.LogWarning("Funcionário deve ser maior de idade. ID: {UserId}", request.Id);
                response.AddNotification("User", "Funcionário deve ser maior de idade!");
                return response;
            }

            if (!await ValidateEmail(request.Id, request.Email))
            {
                logger.LogWarning("Email já cadastrado. ID: {UserId}, Email: {Email}", request.Id, request.Email);
                response.AddNotification("User", "Email já cadastrado!");
                return response;
            }

            if (!await ValidateDocument(request.Id, request.Document))
            {
                logger.LogWarning("Documento já cadastrado. ID: {UserId}, Documento: {Document}", request.Id, request.Document);
                response.AddNotification("User", "Documento já cadastrado!");
                return response;
            }

            if (!await ValidateManager(request.ManagerId))
            {
                logger.LogWarning("Gestor não encontrado. ManagerId: {ManagerId}", request.ManagerId);
                response.AddNotification("User", "Manager is not found");
                return response;
            }

            if (!await ValidatePermissions(request.UserInsertId, request.Permissions))
            {
                logger.LogWarning("Permissões maiores que as do usuário logado. UserInsertId: {UserInsertId}", request.UserInsertId);
                response.AddNotification("User", "Permissions higher than master");
                return response;
            }

            if (!string.IsNullOrEmpty(request.Password))
            {
                request.Password = MD5Hash.Encrypt(_tokenConfiguration + "-" + request.Password);
            }

            response.Id = await userRepository.UpdateAsync((User)request);
            logger.LogInformation("Usuário atualizado com sucesso. ID: {UserId}", response.Id);

            await userPhoneRepository.DeleteByUserIdAsync(request.Id);

            if (request.Phones != null && request.Phones.Any())
            {
                foreach (var phone in request.Phones)
                {
                    await userPhoneRepository.CreateAsync(new UserPhone()
                    {
                        UserId = response.Id,
                        Phone = phone,
                        CreatedOn = DateTime.UtcNow
                    });
                }
                logger.LogInformation("Telefones associados ao usuário. ID: {UserId}", response.Id);
            }

            await userPermissionRepository.DeleteByUserIdAsync(response.Id);

            if (request.Permissions != null && request.Permissions.Any())
            {
                foreach (var permission in request.Permissions)
                {
                    await userPermissionRepository.CreateAsync(new UserPermission()
                    {
                        UserId = response.Id,
                        PermissionId = permission,
                        CreatedOn = DateTime.UtcNow
                    });
                }
                logger.LogInformation("Permissões associadas ao usuário. ID: {UserId}", response.Id);
            }

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao atualizar usuário com ID: {UserId}", request.Id);
            response.AddNotification("Parameters", "Invalid Parameters: " + ex.Message, ErrorCode.BadRequest);
            return response;
        }
    }

    private bool ValidateBirthDate(string birthDate)
    {
        DateTime hoje = DateTime.Today;
        int idade = hoje.Year - Convert.ToDateTime(birthDate).Year;

        if (Convert.ToDateTime(birthDate).Date > hoje.AddYears(-idade))
        {
            idade--;
        }

        return idade >= 18;
    }

    private async Task<bool> ValidateEmail(Guid userId, string email)
    {
        var result = await userRepository.GetByFilters(null, null, email, null, null);
        return result == null || !result.Any() || result.FirstOrDefault()!.Id == userId;
    }

    private async Task<bool> ValidateDocument(Guid userId, string document)
    {
        var result = await userRepository.GetByFilters(null, null, null, document, null);
        return result == null || !result.Any() || result.FirstOrDefault()!.Id == userId;
    }

    private async Task<bool> ValidateManager(Guid managerId)
    {
        if (managerId != Guid.Empty)
        {
            var result = await userRepository.GetByUserId(managerId);
            return result != null;
        }
        return true;
    }

    private async Task<bool> ValidatePermissions(Guid userId, List<int> permissions)
    {
        if (userId != Guid.Empty)
        {
            if (permissions != null && permissions.Any())
            {
                var result = await userPermissionRepository.GetByUserId(userId);
                return result != null && permissions.All(permission => result.Select(x => x.PermissionId).Contains(permission));
            }
            return true;
        }
        return false;
    }
}