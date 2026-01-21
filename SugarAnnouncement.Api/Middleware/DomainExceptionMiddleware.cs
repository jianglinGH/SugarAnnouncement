using SugarAnnouncement.Core.Exceptions;
using SugarAnnouncement.Api.Common;

namespace SugarAnnouncement.Api.Middleware
{
    // 统一处理异常 
    public class DomainExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public DomainExceptionMiddleware(RequestDelegate next) {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context) {
            try {
                await _next(context); 
            } catch (DomainExceptions ex) {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(ApiResponse<string>.Failure(ex.Message, 400));
            }
        }
    }
}