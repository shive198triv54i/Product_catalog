using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ProductCatalog.Api.Swagger
{
    public static class SwaggerConfig
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ProductCatalog API",
                    Version = "v1"
                });
            });
        }
    }
}
