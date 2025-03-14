using System;
namespace GS.Employee.Domain.Entities
{
    public class UserPermission 
    {
        public UserPermission()
        {
        }
        public UserPermission(Guid userId, int permissionId, DateTime createdOn)
        {
            UserId = userId;
            PermissionId = permissionId;
            CreatedOn = createdOn;
        }

        public UserPermission(int id, Guid userId, int? permissionId, DateTime createdOn, DateTime? updatedOn, DateTime? deletedOn)
        {
            Id = id;
            UserId = userId;
            PermissionId = permissionId;
            CreatedOn = createdOn;
            UpdatedOn = updatedOn;
            DeletedOn = deletedOn;
        }
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int? PermissionId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }

    }
}
