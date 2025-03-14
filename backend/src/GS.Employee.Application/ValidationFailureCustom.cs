using FluentValidation.Results;
using GS.Employee.Application.Notifications;

namespace GS.Employee.Application
{
    public enum NotificationCode
    {
        FieldExpected = 1010,
        FieldInvalid = 1020,
        FieldInvalidFormat = 1030,
        FieldMissing = 1040,
        FieldNotFound = 1050,
        HeaderExpected = 2010,
        HeaderInvalid = 2020,
        HeaderInvalidFormat = 2030,
        HeaderMissing = 2040,
        HeaderNotFound = 2050,
        HeaderUnexpected = 2070,
        ResourceExpected = 3010,
        ResourceInvalid = 3020,
        ResourceInvalidFormat = 3030,
        ResourceMissing = 3040,
        ResourceNotFound = 3050,
        UnsupportedScheme = 3060,
        ResourceUnexpected = 3070,
        UnexpectedErrorUnexpected = 4070,
        FileInvalidFormat = 5010,
        FileNotReceived = 5020
    }

    public class ValidationFailureCustom
    {
        public ValidationFailureCustom(string message)
        {
            Message = message;
        }

        public ValidationFailureCustom(string property, string message)
        {
            Property = property;
            Message = message;
        }

        public ValidationFailureCustom(string property, string message, ErrorCode errorCode)
        {
            Property = property;
            Message = message;
            ErrorCode = errorCode;
        }
        public ValidationFailureCustom(ErrorCode errorCode)
        {
            ErrorCode = errorCode;
        }

        public ValidationFailureCustom(string property, string message, ErrorCode errorCode, NotificationCode notificationCode)
        {
            Property = property;
            Message = message;
            ErrorCode = errorCode;
            NotificationCode = notificationCode;
        }

        public string Property { get; set; }
        public string Message { get; set; }
        public NotificationCode NotificationCode { get; }
        public ErrorCode ErrorCode { get; set; }

        public static implicit operator ValidationFailureCustom(ValidationFailure validate)
        {
            if (validate is null)
                return null;

            return new ValidationFailureCustom(validate.PropertyName, validate.ErrorMessage, ErrorCode.BadRequest, NotificationCode.FieldInvalid);
        }
    }
}
