using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace GS.Employee.Infra.Data.Entities
{
    [Table("Permission")]
    public class PermissionPersistence 
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }

        public static implicit operator PermissionPersistence(Domain.Entities.Permission permission)
        {
            if (permission == null)
                return null;

            return new PermissionPersistence()
            {
                Id = permission.Id,
                Description = permission.Description 
             };
        }

        public static implicit operator Domain.Entities.Permission(PermissionPersistence permission)
        {
            if (permission == null)
                return null;

            return new Domain.Entities.Permission(
            
                id: permission.Id,
                description: permission.Description,  
                createdOn: permission.CreatedOn,
                updatedOn: permission.UpdatedOn,
                deletedOn: permission.DeletedOn
            );
        }
    }
}
