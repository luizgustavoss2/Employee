using System.Collections.Generic;
using System.Linq;
using GS.Employee.Application.Notifications;
using GS.Employee.Infra.CrossCutting.Extension;

namespace GS.Employee.Application.ResponseError
{
    public class ApiError
    {
        /// <summary>
        /// High level textual error code, to help categorize the errors.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// A unique reference for the error instance, for audit purposes, in case of unknown/unclassified errors.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Brief Error message, e.g., 'There is something wrong with the request parameters provided'
        /// </summary>
        public string Message { get; set; }
        public ICollection<ErrorData> Errors { get; set; }

        public ApiError(string code, string id, string message, ICollection<ValidationFailureCustom> errors)
        {
            Code = code;
            Id = id;
            Message = message;

            if(errors != null)
                Errors = errors.Select(x => (ErrorData)x).ToList();
        }

        public ApiError(ICollection<ValidationFailureCustom> errors)
        {
            Errors = errors.Select(x => (ErrorData)x).ToList();
        }

        public static ApiError FromResult(ResponseBase result)
        {
            var error = result.Error ?? ErrorCode.BadRequest;
            var code = $"{result.Domain}-{result.Subdomain}-{result.Error}".ToUpper();
            var id = string.Concat((int)result.Domain, (int)result.Subdomain, (int)error);
            var message = string.Format((result.Error ?? ErrorCode.BadRequest).TryGetDescription("Error ocurred."));
            var notifications = result.Notifications.ToList();
            return new ApiError(code, id, message, notifications);
        }
    }
}
