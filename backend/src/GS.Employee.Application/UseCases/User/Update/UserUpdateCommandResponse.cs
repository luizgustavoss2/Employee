using System;
using GS.Employee.Application.Notifications;
namespace GS.Employee.Application.UseCases
{
    public class UserUpdateCommandResponse : ResponseBase
    {
         public Guid Id { get; set; }
    }
}
