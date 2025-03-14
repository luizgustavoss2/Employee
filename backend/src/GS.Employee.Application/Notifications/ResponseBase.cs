using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace GS.Employee.Application.Notifications
{
    public enum EDomain
    {
        Unity = 1,
        Vixi = 2,
    }

    [ExcludeFromCodeCoverage]
    public class ResponseBase
    {
        public ResponseBase()
        {
            Notifications = new List<ValidationFailureCustom>();
        }
        public EDomain Domain { get => EDomain.Unity; }
        public EDomain Subdomain { get => EDomain.Vixi; }
        public ErrorCode? Error { get; set; }
        public bool IsValid => !Notifications.Any();
        public IList<ValidationFailureCustom> Notifications { get; private set; }


        public void AddNotifications(IList<ValidationFailureCustom> errors, ErrorCode errorCode)
        {
            Notifications = errors;
            Error = errorCode;
        }
        public void AddNotification(ErrorCode errorCode)
        {
            Notifications.Add(new ValidationFailureCustom(errorCode));
            Error = errorCode;
        }
        public void AddNotification(string message)
        {
            Notifications.Add(new ValidationFailureCustom(message));
        }
        public void AddNotification(string property, string message)
        {
            Notifications.Add(new ValidationFailureCustom(property, message));
        }
        public void AddNotification(string property, string message, ErrorCode errorCode)
        {
            Notifications.Add(new ValidationFailureCustom(property, message, errorCode));
            Error = errorCode;
        }
        public void AddNotification(string property, string message, ErrorCode errorCode, NotificationCode notificationCode)
        {
            Notifications.Add(new ValidationFailureCustom(property, message, errorCode, notificationCode));
            Error = errorCode;
        }
    }
}
