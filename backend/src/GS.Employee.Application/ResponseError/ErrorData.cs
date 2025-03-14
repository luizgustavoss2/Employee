using GS.Employee.Infra.CrossCutting.Extension;

namespace GS.Employee.Application.ResponseError
{
    public class ErrorData
    {
        /// <summary>
        /// Low level textual error code, e.g., UK.OBIE.Field.Missing
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// A description of the error that occurred. e.g., 'A mandatory field isn't supplied' or 'RequestedExecutionDateTime must be in future' OBIE doesn't standardise this field
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Recommended but optional reference to the JSON Path of the field with error, e.g., Data.Initiation.InstructedAmount.Currency
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// URL to help remediate the problem, or provide more information, or to API Reference, or help etc
        /// </summary>
        public string Url { get; set; }

        public static explicit operator ErrorData(ValidationFailureCustom notification)
        {
            return new ErrorData
            {
                Message = notification.Message,
                Path = notification.Property,
                ErrorCode = notification.NotificationCode.TryGetDescription()
            };
        }
    }
}
