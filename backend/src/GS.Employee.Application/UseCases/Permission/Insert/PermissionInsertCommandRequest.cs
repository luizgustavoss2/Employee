using System;
using FluentValidation;
using MediatR;

namespace GS.Employee.Application.UseCases
{
    public class PermissionInsertCommandRequest : RequestBase<PermissionInsertCommandRequest>, IRequest<PermissionInsertCommandResponse>
    {
        public PermissionInsertCommandRequest(string description)
        {
            Description = description;
            ValidateModel();
        }
        public string Description { get; set; }

        private void ValidateModel()
        {
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.").WithState(x => x.Code = NotificationCode.FieldMissing);

            ValidateModel(this);
        }
    }
}
