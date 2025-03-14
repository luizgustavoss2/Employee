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

public class UserInsertCommandHandler(
    IRepositoryUser userRepository,
    IRepositoryUserPhone userPhoneRepository,
    IRepositoryUserPermission userPermissionRepository,
    ILogger<UserInsertCommandHandler> logger) : IRequestHandler<UserInsertCommandRequest, UserInsertCommandResponse>
{
    private readonly string _tokenConfiguration = "gs";

    public async Task<UserInsertCommandResponse> Handle(UserInsertCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new UserInsertCommandResponse();
        logger.LogInformation("Iniciando inserção de usuário com email: {Email}", request.Email);

        try
        {
            var valid = RequestBase<UserInsertCommandRequest>.ValidateRequest(request, response);
            if (!valid)
            {
                logger.LogWarning("Requisição inválida para o email: {Email}", request.Email);
                return response;
            }

            if (!ValidateBirthDate(request.BirthDate))
            {
                logger.LogWarning("Funcionário deve ser maior de idade. Email: {Email}", request.Email);
                response.AddNotification("User", "Funcionário deve ser maior de idade!");
                return response;
            }

            if (!await ValidateEmail(request.Email))
            {
                logger.LogWarning("Email já cadastrado: {Email}", request.Email);
                response.AddNotification("User", "Email já cadastrado!");
                return response;
            }

            if (!await ValidateDocument(request.Document))
            {
                logger.LogWarning("Documento já cadastrado: {Document}", request.Document);
                response.AddNotification("User", "Documento já cadastrado!");
                return response;
            }

            if (!await ValidateManager(request.ManagerId))
            {
                logger.LogWarning("Gestor não encontrado. ManagerId: {ManagerId}", request.ManagerId);
                response.AddNotification("User", "Gestor não encontrado!");
                return response;
            }

            if (!await ValidatePermissions(request.UserInsertId, request.Permissions))
            {
                logger.LogWarning("Permissão não pode ser maior que do usuário logado. UserInsertId: {UserInsertId}", request.UserInsertId);
                response.AddNotification("User", "Permissão não pode ser maior que do usuário logado!");
                return response;
            }

            var objUser = PrepareObject(request);

            response.Id = await userRepository.CreateAsync(objUser);
            logger.LogInformation("Usuário criado com sucesso. ID: {UserId}", response.Id);

            await userPhoneRepository.DeleteByUserIdAsync(response.Id);

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
            logger.LogError(ex, "Erro ao inserir usuário com email: {Email}", request.Email);
            response.AddNotification("Parameters", "Invalid Parameters: " + ex.Message, ErrorCode.BadRequest);
            return response;
        }
    }

    private User PrepareObject(UserInsertCommandRequest request)
    {
        var pass = MD5Hash.Encrypt(_tokenConfiguration + "-" + request.Password);
        return new User(Guid.NewGuid(), request.FirstName, request.LastName, request.Email, request.Document, request.BirthDate, request.ManagerId, pass, DateTime.Now, null, null);
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

    private async Task<bool> ValidateEmail(string email)
    {
        var result = await userRepository.GetByFilters(null, null, email, null, null);
        return result == null || !result.Any();
    }

    private async Task<bool> ValidateDocument(string document)
    {
        var result = await userRepository.GetByFilters(null, null, null, document, null);
        return result == null || !result.Any();
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