using System;
namespace GS.Employee.Domain.Entities
{
    public class UserPhone 
    {
        public UserPhone()
        {
        }

        public UserPhone(int id, Guid userId, string phone, DateTime createdOn, DateTime? updatedOn, DateTime? deletedOn)
        {
            Id = id;
            UserId = userId;
            Phone = phone;
            CreatedOn = createdOn;
            UpdatedOn = updatedOn;
            DeletedOn = deletedOn;
        }
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }

    }
}
