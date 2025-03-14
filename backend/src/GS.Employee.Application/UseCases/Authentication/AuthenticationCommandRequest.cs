using FluentValidation;
using MediatR;
using System;

namespace GS.Employee.Application.UseCases
{
    public class AuthenticationCommandRequest : RequestBase<AuthenticationCommandRequest>, IRequest<AuthenticationCommandResponse>
    {
        public AuthenticationCommandRequest(string email, string password)
        {
            Email = email;
            Password = password;
            ValidateModel();
        }

        public string Email { get; set; }
        public string Password { get; set; }

        private void ValidateModel()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.").WithState(x => x.Code = NotificationCode.FieldMissing);
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.").WithState(x => x.Code = NotificationCode.FieldMissing);
            ValidateModel(this);
        }
    }
}
