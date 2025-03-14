using System;
using GS.Employee.Application.Notifications;
namespace GS.Employee.Application.UseCases
{
    public class UserInsertCommandResponse : ResponseBase
    {
         public Guid Id { get; set; }
    }
}
