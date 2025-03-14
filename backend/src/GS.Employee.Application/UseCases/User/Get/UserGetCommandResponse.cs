using System.Collections.Generic;
using GS.Employee.Application.Notifications;
using GS.Employee.Domain.Entities;
namespace GS.Employee.Application.UseCases
{
    public class UserGetCommandResponse : ResponseBase
    {
        public IEnumerable<User> User { get; set; }
    }
}
