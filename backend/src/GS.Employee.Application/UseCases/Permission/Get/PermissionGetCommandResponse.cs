using System.Collections.Generic;
using GS.Employee.Application.Notifications;
using GS.Employee.Domain.Entities;
namespace GS.Employee.Application.UseCases
{
    public class PermissionGetCommandResponse : ResponseBase
    {
        public IEnumerable<Permission> Permission { get; set; }
    }
}
