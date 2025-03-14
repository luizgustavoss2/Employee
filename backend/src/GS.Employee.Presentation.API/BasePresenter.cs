using Microsoft.AspNetCore.Mvc;
using System.Net;
using GS.Employee.Application.Notifications;
using GS.Employee.Application.ResponseError;

namespace GS.Employee.Presentation.API
{
    public abstract class BasePresenter : ResponseBase
    {
        public virtual IActionResult GetActionResult<T>(T result, object obj = null, HttpStatusCode? statusCode = null)
            where T : ResponseBase
        {
            if (!result.IsValid)
                return CreateErrorResult(result);

            return (statusCode, obj is null) switch
            {
                (HttpStatusCode.Created, false) => new CreatedResult(obj.ToString(), null),
                (HttpStatusCode.Created, true) => new CreatedResult(string.Empty, string.Empty),
                (_, false) => new OkObjectResult(obj),
                (_, true) => new NoContentResult(),
            };
        }

        protected virtual IActionResult CreateErrorResult<T>(T result)
            where T : ResponseBase
        {
            var error = result.Error ?? ErrorCode.BadRequest;
            var errorBody = ApiError.FromResult(result);

            return error switch
            {
                ErrorCode.BadRequest => new BadRequestObjectResult(errorBody),
                ErrorCode.NotFound => new NotFoundObjectResult(errorBody),
                ErrorCode.Conflict => new ConflictObjectResult(errorBody),
                _ => new BadRequestObjectResult(errorBody),
            };
        }
    }
}