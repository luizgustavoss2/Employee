using MediatR;
using System; 
using System.Threading;
using System.Threading.Tasks;
using GS.Employee.Application.Notifications;
using GS.Employee.Domain.Entities;
using GS.Employee.Domain.Interfaces.Repository;

namespace GS.Employee.Application.UseCases
{
    public class PermissionUpdateCommandHandler : IRequestHandler<PermissionUpdateCommandRequest, PermissionUpdateCommandResponse>
    {
        private readonly IRepositoryPermission _permissionRepository;
        private readonly IRepository<Permission> _genericRepository;
        public PermissionUpdateCommandHandler(IRepositoryPermission permissionRepository, IRepository<Permission> genericRepository)
        {
            _permissionRepository = permissionRepository;
            _genericRepository = genericRepository;
        }

        public async Task<PermissionUpdateCommandResponse> Handle(PermissionUpdateCommandRequest request, CancellationToken cancellationToken)
        {
            var response = new PermissionUpdateCommandResponse();
            var valid = RequestBase<PermissionUpdateCommandRequest>.ValidateRequest(request, response);
            if (!valid)
                return response;

            var permission = _genericRepository.GetAsync<Permission>(request.Id);

            if(permission.Result is null)
            {
                response.AddNotification("Id", "Permission not found!", ErrorCode.NotFound);
                return response;
            }

            response.Id = await _permissionRepository.UpdateAsync((Permission)request);

            return response;
        }
    }
}
