using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;
using GS.Employee.Application.Notifications;

namespace GS.Employee.Application
{
    public class RequestBase<T> : AbstractValidator<T>
    {
        public NotificationCode Code { get; set; }
        public bool IsValid => !Notifications.Any();
        public IList<ValidationFailureCustom> Notifications { get; private set; }
        protected void ValidateModel(T instance)
        {
            var result = Validate(instance);
            Notifications = result.Errors.Select<ValidationFailure, ValidationFailureCustom>(x => x).ToList();
        }
        public static bool ValidateRequest(RequestBase<T> request, ResponseBase response)
        {
            if (request == null)
            {
                response.AddNotification(ErrorCode.BadRequest);
                return false;
            }

            if (!request.IsValid)
            {
                response.AddNotifications(request.Notifications, ErrorCode.BadRequest);
                return false;
            }

            return true;
        }
    }
}
