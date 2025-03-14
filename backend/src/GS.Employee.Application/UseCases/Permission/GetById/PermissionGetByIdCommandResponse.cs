using GS.Employee.Application.Notifications;
using GS.Employee.Domain.Entities;
namespace GS.Employee.Application.UseCases
{
    public class PermissionGetByIdCommandResponse : ResponseBase
    {
         public Permission Permission { get; set; }
    }
}
