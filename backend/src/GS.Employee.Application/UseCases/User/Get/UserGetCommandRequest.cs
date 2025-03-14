using MediatR;

namespace GS.Employee.Application.UseCases
{
    public class UserGetCommandRequest : RequestBase<UserGetCommandRequest>, IRequest<UserGetCommandResponse>
    {
    }
}
