using System;

namespace GS.Employee.Presentation.API.UseCases
{
    public class AuthenticationRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
