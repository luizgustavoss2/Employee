using System;
using FluentValidation;
using MediatR;

namespace GS.Employee.Application.UseCases
{
    public class PermissionUpdateCommandRequest : RequestBase<PermissionUpdateCommandRequest>, IRequest<PermissionUpdateCommandResponse>
    {
        public PermissionUpdateCommandRequest(int id, string description)
        {
            Id = id;
            Description = description;
            ValidateModel();
        }
        public int Id { get; set; }
        public string Description { get; set; }

        private void ValidateModel()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.").WithState(x => x.Code = NotificationCode.FieldMissing);
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.").WithState(x => x.Code = NotificationCode.FieldMissing);

            ValidateModel(this);
        }

        public static explicit operator Domain.Entities.Permission(PermissionUpdateCommandRequest request)
        {
            if (request is null)
                return null;

            return new Domain.Entities.Permission()
            {
               Id = request.Id,
               Description = request.Description
            };
        }
    }
}
