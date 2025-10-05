using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ProductCatalog.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<Middleware.ExceptionMiddleware>();
        }

        public static IApplicationBuilder UseSwaggerMiddleware(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductCatalog API V1");
            });
            return app;
        }
    }
}
