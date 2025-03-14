using System.ComponentModel;

namespace GS.Employee.Application.Notifications
{
    public enum ErrorCode
    {
        [Description("Invalid parameters were inputed")]
        BadRequest = 400,

        [Description("Requested resource was not found")]
        NotFound = 404,

        [Description("Conflict processing request")]
        Conflict = 409,
    }
}
