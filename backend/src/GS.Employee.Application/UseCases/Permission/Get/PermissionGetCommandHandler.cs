using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GS.Employee.Domain.Entities;
using GS.Employee.Domain.Interfaces.Repository;
using GS.Employee.Infra.Data.Entities; 

namespace GS.Employee.Application.UseCases
{
    public class PermissionGetCommandHandler : IRequestHandler<PermissionGetCommandRequest, PermissionGetCommandResponse>
    {
        private readonly IRepository<Permission> _permissionRepository;
        public PermissionGetCommandHandler(IRepository<Permission> permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<PermissionGetCommandResponse> Handle(PermissionGetCommandRequest request, CancellationToken cancellationToken)
        {
            var response = new PermissionGetCommandResponse();

            var permissionPersistence = await _permissionRepository.GetAsync<PermissionPersistence>();

            var permission = permissionPersistence.Select<PermissionPersistence, Permission>(x => x);
            response.Permission = permission;
            return response; 
        }
    }
}
