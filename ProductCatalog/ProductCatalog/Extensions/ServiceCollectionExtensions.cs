using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.MappingProfiles;
using ProductCatalog.Application.Services;
using ProductCatalog.Application.Validators;
using ProductCatalog.Core.Interfaces;
using ProductCatalog.Infrastructure.Data;
using ProductCatalog.Infrastructure.UnitOfWork;
using System.Text;

namespace ProductCatalog.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // AutoMapper etc (existing)
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ProductProfile>();
                cfg.AddProfile<CategoryProfile>();
            });

            // Services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserService, UserService>();

            // Auth service
            services.AddScoped<IAuthService, AuthService>();

            // FluentValidation
            services.AddValidatorsFromAssemblyContaining<ProductValidator>();

            return services;
        }



        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProductCatalogDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // JWT Auth
            var jwtSettings = configuration.GetSection("JwtSettings");
            var key = jwtSettings.GetValue<string>("Key");
            if (string.IsNullOrEmpty(key))
                throw new InvalidOperationException("JWT Key is not configured.");

            var keyBytes = Encoding.UTF8.GetBytes(key);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // set true in production
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
                    ValidAudience = jwtSettings.GetValue<string>("Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ClockSkew = TimeSpan.Zero
                };
                Console.WriteLine("adkfjad;sf "+options);
            });

            return services;
        }
    }
}
