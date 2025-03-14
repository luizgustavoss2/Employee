using MediatR;

namespace GS.Employee.Application.UseCases
{
    public class PermissionGetCommandRequest : RequestBase<PermissionGetCommandRequest>, IRequest<PermissionGetCommandResponse>
    {
    }
}
