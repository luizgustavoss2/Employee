using System;
using System.Collections.Generic;

namespace GS.Employee.Presentation.API.UseCases
{
    public class UpdateUserRequest
    {
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
    }
}
