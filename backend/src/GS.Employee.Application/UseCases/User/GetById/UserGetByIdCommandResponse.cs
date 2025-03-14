using GS.Employee.Application.Notifications;
using GS.Employee.Domain.Entities;
namespace GS.Employee.Application.UseCases
{
    public class UserGetByIdCommandResponse : ResponseBase
    {
         public User User { get; set; }
    }
}
