using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace GS.Employee.Infra.Data.Entities
{
    [Table("User")]
    public class UserPersistence : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
        public string BirthDate { get; set; }
        public Guid ManagerId { get; set; }
        public string Password { get; set; }

        public static implicit operator UserPersistence(Domain.Entities.User user)
        {
            if (user == null)
                return null;

            return new UserPersistence()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Document = user.Document,
                BirthDate = user.BirthDate,
                ManagerId = user.ManagerId,
                Password = user.Password
            };
        }

        public static implicit operator Domain.Entities.User(UserPersistence user)
        {
            if (user == null)
                return null;

            return new Domain.Entities.User(

                id: user.Id,
                firstName: user.FirstName,
                lastName: user.LastName,
                email: user.Email,
                document: user.Document,
                birthDate: user.BirthDate,
                managerId: user.ManagerId,
                password: user.Password,
                createdOn: user.CreatedOn,
                updatedOn: user.UpdatedOn,
                deletedOn: user.DeletedOn
            );
        }
    }
}
