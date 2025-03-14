using System;
namespace GS.Employee.Domain.Entities
{
    public class Permission 
    {
        public Permission()
        {
        }
        public Permission(string description, DateTime createdOn)
        {
            Description = description;
            CreatedOn = createdOn;
        }

        public Permission(int id, string description, DateTime createdOn, DateTime? updatedOn, DateTime? deletedOn)
        {
            Id = id;
            Description = description;
            CreatedOn = createdOn;
            UpdatedOn = updatedOn;
            DeletedOn = deletedOn;
        }
        public  int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }

    }
}
