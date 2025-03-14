using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using GS.Employee.Domain.Entities;
using GS.Employee.Application.Notifications;
using GS.Employee.Domain.Interfaces.Repository;

namespace GS.Employee.Application.UseCases
{
    public class PermissionDeleteCommandHandler : IRequestHandler<PermissionDeleteCommandRequest, PermissionDeleteCommandResponse>
    {
        private readonly IRepository<Permission> _permissionRepository;
        private readonly IRepository<Permission> _genericRepository;
        public PermissionDeleteCommandHandler(IRepository<Permission> permissionRepository, IRepository<Permission> genericRepository)
        {
            _permissionRepository = permissionRepository;
            _genericRepository = genericRepository;
        }

        public async Task<PermissionDeleteCommandResponse> Handle(PermissionDeleteCommandRequest request, CancellationToken cancellationToken)
        {
            var response = new PermissionDeleteCommandResponse();
            var valid = RequestBase<PermissionDeleteCommandRequest>.ValidateRequest(request, response);
            if (!valid)
                return response;

            var permission = _genericRepository.GetAsync<Permission>(request.Id);

            if(permission.Result is null)
            {
                response.AddNotification("Id", "Permission not found!", ErrorCode.NotFound);
                return response;
            }

            _ = await _permissionRepository.DeleteAsync(request.Id);
            return response;
        }
    }
}
