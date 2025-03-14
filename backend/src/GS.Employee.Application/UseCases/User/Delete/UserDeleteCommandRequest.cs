using FluentValidation;
using MediatR;
using System;

namespace GS.Employee.Application.UseCases
{
    public class UserDeleteCommandRequest : RequestBase<UserDeleteCommandRequest>, IRequest<UserDeleteCommandResponse>
    {
        public UserDeleteCommandRequest(Guid id)
        {
            Id = id;
            ValidateModel();
        }

        public Guid Id { get; set; }

        private void ValidateModel()
        {
            RuleFor(x => x.Id).Must(x => Guid.TryParse(x.ToString(), out var result)).WithMessage("Id is not valid.").WithState(x => x.Code = NotificationCode.FieldInvalid);
            ValidateModel(this);
        }
    }
}
