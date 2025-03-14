using System;
using System.Collections.Generic;

namespace GS.Employee.Domain.Entities;

public class User : BaseEntity
{

    public User() { }

    public User(
        Guid id,
        string firstName,
        string lastName,
        string email,
        string document,
        string birthDate,
        Guid managerId,
        string password,
        DateTime createdOn,
        DateTime? updatedOn,
        DateTime? deletedOn)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Document = document;
        BirthDate = birthDate;
        ManagerId = managerId;
        Password = password;
        CreatedOn = createdOn;
        UpdatedOn = updatedOn;
        DeletedOn = deletedOn;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Document { get; set; }
    public string BirthDate { get; set; } 
    public Guid ManagerId { get; set; }
    public string Password { get; set; } 

    public virtual IEnumerable<UserPhone> PhoneNumbers { get; set; } 
    public virtual IEnumerable<UserPermission> Permissions { get; set; }
    public virtual User? Manager { get; set; } 
}