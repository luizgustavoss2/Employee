using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace GS.Employee.Infra.Data.Entities
{
    [Table("UserPermission")]
    public class UserPermissionPersistence
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int? PermissionId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }

        public static implicit operator UserPermissionPersistence(Domain.Entities.UserPermission userPermission)
        {
            if (userPermission == null)
                return null;

            return new UserPermissionPersistence()
            {
                Id = userPermission.Id,
                UserId = userPermission.UserId,
                PermissionId = userPermission.PermissionId 
             };
        }

        public static implicit operator Domain.Entities.UserPermission(UserPermissionPersistence userPermission)
        {
            if (userPermission == null)
                return null;

            return new Domain.Entities.UserPermission(
            
                id: userPermission.Id,
                userId: userPermission.UserId, 
                permissionId: userPermission.PermissionId,  
                createdOn: userPermission.CreatedOn,
                updatedOn: userPermission.UpdatedOn,
                deletedOn: userPermission.DeletedOn
            );
        }
    }
}
