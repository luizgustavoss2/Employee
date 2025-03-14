using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace GS.Employee.Infra.Data.Entities
{
    [Table("UserPhone")]
    public class UserPhonePersistence 
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Phone { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }

        public static implicit operator UserPhonePersistence(Domain.Entities.UserPhone userPhone)
        {
            if (userPhone == null)
                return null;

            return new UserPhonePersistence()
            {
                Id = userPhone.Id,
                UserId = userPhone.UserId,
                Phone = userPhone.Phone 
             };
        }

        public static implicit operator Domain.Entities.UserPhone(UserPhonePersistence userPhone)
        {
            if (userPhone == null)
                return null;

            return new Domain.Entities.UserPhone(
            
                id: userPhone.Id,
                userId: userPhone.UserId, 
                phone: userPhone.Phone,  
                createdOn: userPhone.CreatedOn,
                updatedOn: userPhone.UpdatedOn,
                deletedOn: userPhone.DeletedOn
            );
        }
    }
}
