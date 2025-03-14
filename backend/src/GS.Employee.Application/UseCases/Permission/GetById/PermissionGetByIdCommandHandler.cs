using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using GS.Employee.Domain.Entities;
using GS.Employee.Domain.Interfaces.Repository;
using GS.Employee.Infra.Data.Entities;

namespace GS.Employee.Application.UseCases
{
    public class PermissionGetByIdCommandHandler : IRequestHandler<PermissionGetByIdCommandRequest, PermissionGetByIdCommandResponse>
    {
        private readonly IRepository<Permission> _permissionRepository;
        public PermissionGetByIdCommandHandler(IRepository<Permission> permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<PermissionGetByIdCommandResponse> Handle(PermissionGetByIdCommandRequest request, CancellationToken cancellationToken)
        {
            var response = new PermissionGetByIdCommandResponse();
            var valid = RequestBase<PermissionGetByIdCommandRequest>.ValidateRequest(request, response);
            if (!valid)
                return response;

            var permission = await _permissionRepository.GetAsync<PermissionPersistence>(request.Id);

            response.Permission = permission;
            return response; 
        }
    }
}
