using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using GS.Employee.Domain.Entities;
using MediatR;

namespace GS.Employee.Application.UseCases
{
    public class UserUpdateCommandRequest : RequestBase<UserUpdateCommandRequest>, IRequest<UserUpdateCommandResponse>
    {
        public UserUpdateCommandRequest(Guid id, string firstName, string lastName, string email, string document, string birthDate, Guid managerId, string password, List<string> phones, List<int> permissions, Guid userInsertId)
        {
            Id = id;
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
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
        public string BirthDate { get; set; }
        public Guid ManagerId { get; set; }
        public string Password { get; set; }
        public List<string> Phones { get; set; }
        public List<int> Permissions { get; set; }
        public Guid UserInsertId { get; set; }

        private void ValidateModel()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.").WithState(x => x.Code = NotificationCode.FieldMissing);
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Nome é obrigatório!").WithState(x => x.Code = NotificationCode.FieldMissing);
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Sobrenome é obrigatório!").WithState(x => x.Code = NotificationCode.FieldMissing);
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email é obrigatório!").WithState(x => x.Code = NotificationCode.FieldMissing);
            RuleFor(x => x.Document).NotEmpty().WithMessage("Document0 é obrigatório!").WithState(x => x.Code = NotificationCode.FieldMissing);
            RuleFor(x => x.BirthDate).NotEmpty().WithMessage("Data de Nascimento é obrigatório!").WithState(x => x.Code = NotificationCode.FieldMissing);
            RuleFor(x => x.Password).NotEmpty().WithMessage("Senha é obrigatório!").WithState(x => x.Code = NotificationCode.FieldMissing);
            RuleFor(x => x.UserInsertId).NotEmpty().WithMessage("Usuário de edição é obrigatório!").WithState(x => x.Code = NotificationCode.FieldMissing);


            ValidateModel(this);
        }

        public static explicit operator Domain.Entities.User(UserUpdateCommandRequest request)
        {
            if (request is null)
                return null;

            var user =  new Domain.Entities.User()
            {
                Id = request.Id,
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
                    user.PhoneNumbers.Append(new UserPhone() {UserId = user.Id, Phone = phone });
                }
            }

            return user;
        }
    }
}
