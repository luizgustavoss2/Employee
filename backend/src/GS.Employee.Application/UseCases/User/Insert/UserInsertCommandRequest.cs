using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using GS.Employee.Domain.Entities;
using MediatR;

namespace GS.Employee.Application.UseCases
{
    public class UserInsertCommandRequest : RequestBase<UserInsertCommandRequest>, IRequest<UserInsertCommandResponse>
    {
        public UserInsertCommandRequest(string firstName, string lastName, string email, string document, string birthDate, Guid managerId, string password, List<string> phones, List<int> permissions, Guid userInsertId)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Document = document;
            BirthDate = birthDate;
            ManagerId = managerId;
            Password = password;
            Phones = phones;
            Permissions = permissions;
            UserInsertId = userInsertId;
            ValidateModel();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
        public string BirthDate { get; set; }
        public Guid ManagerId { get; set; }
        public string Password { get; set; }
        public List<string> Phones { get; set; }
        public List<int> Permissions { get; set; }
        public  Guid UserInsertId { get; set; }

        private void ValidateModel()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Nome é obrigatório!").WithState(x => x.Code = NotificationCode.FieldMissing);
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Sobrenome é obrigatório!").WithState(x => x.Code = NotificationCode.FieldMissing);
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email é obrigatório!").WithState(x => x.Code = NotificationCode.FieldMissing);
            RuleFor(x => x.Document).NotEmpty().WithMessage("Documento é obrigatório!").WithState(x => x.Code = NotificationCode.FieldMissing);
            RuleFor(x => x.BirthDate).NotEmpty().WithMessage("Data de Nascimento é obrigatório!").WithState(x => x.Code = NotificationCode.FieldMissing);
            RuleFor(x => x.Password).NotEmpty().WithMessage("Senha é obrigatório!").WithState(x => x.Code = NotificationCode.FieldMissing);
            RuleFor(x => x.UserInsertId).NotEmpty().WithMessage("Usuario de inclusão é obrigatório!").WithState(x => x.Code = NotificationCode.FieldMissing);

            ValidateModel(this);
        }

        public static explicit operator User(UserInsertCommandRequest request)
        {
            if (request is null)
                return null;

            var user = new User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Document = request.Document,
                BirthDate = request.BirthDate,
                ManagerId = request.ManagerId,
                Password = request.Password
            };

            if (request.Phones != null)
            {
                user.PhoneNumbers = new List<UserPhone>();

                foreach (var phone in request.Phones)
                {
                    user.PhoneNumbers.Append(new UserPhone() { Phone = phone });
                }
            }

            if (request.Permissions != null)
            {
                user.Permissions = new List<UserPermission>();

                foreach (var permission in request.Permissions)
                {
                    user.Permissions.Append(new UserPermission() { PermissionId = permission });
                }
            }

            return user;
        }

    }
}
