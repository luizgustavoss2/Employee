using GS.Employee.Application.Notifications;
namespace GS.Employee.Application.UseCases
{
    public class AuthenticationCommandResponse : ResponseBase
    {
         public AuthenticationResponse AuthenticationClient { get; set; }
    }
}
