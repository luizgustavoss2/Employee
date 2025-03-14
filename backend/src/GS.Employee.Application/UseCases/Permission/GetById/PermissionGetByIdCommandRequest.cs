using FluentValidation;
using MediatR;
using System;

namespace GS.Employee.Application.UseCases
{
    public class PermissionGetByIdCommandRequest : RequestBase<PermissionGetByIdCommandRequest>, IRequest<PermissionGetByIdCommandResponse>
    {
        public PermissionGetByIdCommandRequest(int id)
        {
            Id = id;
            ValidateModel();
        }

        public int Id { get; set; }

        private void ValidateModel()
        {
            //RuleFor(x => x.Id).Must(x => x.ToString(), out var result)).WithMessage("Id is not valid.").WithState(x => x.Code = NotificationCode.FieldInvalid);
            ValidateModel(this);
        }
    }
}
