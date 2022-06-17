namespace EM.Web.Infrastructure.Middlewares
{
    using Microsoft.AspNetCore.Http;

    public class NotFoundPageMiddleware
    {
        private readonly RequestDelegate next;

        public NotFoundPageMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            await this.next(httpContext);

            if (httpContext.Response.StatusCode == 404)
            {
                httpContext.Request.Path = "/Home/PageNotFound";

                await this.next(httpContext);
            }
        }
    }
}
