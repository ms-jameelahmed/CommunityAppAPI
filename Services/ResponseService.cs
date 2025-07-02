namespace CommunityAppAPI.Services
{
    using global::CommunityAppAPI.Services.Common;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.Extensions.Logging;

    namespace CommunityAppAPI.Services.Common
    {
        public class ResponseService : IResponseService
        {
            private readonly ILogger<ResponseService> _logger;

            public ResponseService(ILogger<ResponseService> logger)
            {
                _logger = logger;
            }

            public ObjectResult SuccessResponse(object data, string message)
            {
                var uniqueId = Guid.NewGuid();
                var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

                _logger.LogInformation("Success: {Message}, UniqueId: {UniqueId}, Timestamp: {Timestamp}",
                    message, uniqueId, timestamp);

                return new OkObjectResult(new
                {
                    success = true,
                    message,
                    uniqueId,
                    timestamp,
                    data
                });
            }

            public ObjectResult ErrorResponse(string errorMessage)
            {
                var uniqueId = Guid.NewGuid();
                var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

                _logger.LogError("Error: {Error}, UniqueId: {UniqueId}, Timestamp: {Timestamp}",
                    errorMessage, uniqueId, timestamp);

                return new ObjectResult(new
                {
                    success = false,
                    message = errorMessage,
                    uniqueId,
                    timestamp,
                    data = (object)null
                })
                {
                    StatusCode = 500
                };
            }

            public ObjectResult ValidationErrorResponse(ModelStateDictionary modelState)
            {
                var uniqueId = Guid.NewGuid();
                return new BadRequestObjectResult(new
                {
                    success = false,
                    message = "Validation failed",
                    uniqueId,
                    data = modelState
                });
            }

            public ObjectResult NotFoundResponse(string message)
            {
                var uniqueId = Guid.NewGuid();
                return new NotFoundObjectResult(new
                {
                    success = false,
                    message,
                    uniqueId,
                    data = (object)null
                });
            }

            public ObjectResult ConflictResponse(string message)
            {
                return new ObjectResult(new
                {
                    success = false,
                    message,
                    uniqueId = Guid.NewGuid(),
                    timestamp = DateTime.UtcNow,
                    data = (object)null
                })
                {
                    StatusCode = 409
                };
            }

            
        }
    }

}
