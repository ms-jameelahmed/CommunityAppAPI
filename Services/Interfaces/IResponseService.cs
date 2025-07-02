using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CommunityAppAPI.Services.Common
{
    public interface IResponseService
    {
        ObjectResult SuccessResponse(object data, string message);
        ObjectResult ErrorResponse(string errorMessage);
        ObjectResult ValidationErrorResponse(ModelStateDictionary modelState);
        ObjectResult NotFoundResponse(string message);

        ObjectResult ConflictResponse(string message);
    }
}

