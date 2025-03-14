using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using GS.Employee.Domain.Entities;
using GS.Employee.Domain.Interfaces.Repository;

namespace GS.Employee.Application.UseCases
{
    public class PermissionInsertCommandHandler : IRequestHandler<PermissionInsertCommandRequest, PermissionInsertCommandResponse>
    {
        private readonly IRepositoryPermission _permissionRepository;
        public PermissionInsertCommandHandler(IRepositoryPermission permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<PermissionInsertCommandResponse> Handle(PermissionInsertCommandRequest request, CancellationToken cancellationToken)
        {
            var response = new PermissionInsertCommandResponse();
            var valid = RequestBase<PermissionInsertCommandRequest>.ValidateRequest(request, response);
            if (!valid)
                return response;

            var objPermission = PrepareObject(request);

            response.Id = await _permissionRepository.CreateAsync(objPermission);
            return response;
        }

        private Permission PrepareObject(PermissionInsertCommandRequest request) => new Permission(request.Description,DateTime.Now);
    }
}
