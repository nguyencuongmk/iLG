﻿using iLG.API.Handlers;
using iLG.API.Helpers;
using iLG.API.Maps;
using iLG.API.Middleware;
using iLG.API.Services;
using iLG.API.Services.Abstractions;
using iLG.API.Settings;
using iLG.Infrastructure.Data.Initialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace iLG.API.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            // Config Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ILG.API", Description = "ILG Rest Api", Version = "v1.0" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            // Config Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidIssuer = configuration["AppSettings:JwtSettings:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:JwtSettings:Secret"]!)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(0),
                    ValidateAudience = true,
                    ValidAudience = configuration["AppSettings:JwtSettings:Audience"],
                };
            });

            // Config Auto Mapper
            services.AddAutoMapper(typeof(Mapper));

            // Config Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();

            // Config Exception Handler
            services.AddExceptionHandler<ExceptionHandler>();

            // Config Routing
            services.AddRouting(options => options.LowercaseUrls = true);

            return services;
        }

        public static async Task<WebApplication> UseApiServices(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(c =>
                {
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    {
                        swaggerDoc.Servers =
                        [
                            new OpenApiServer()
                            {
                                Url = "",
                                Description = "Local"
                            }
                        ];
                    });
                });
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "ILG API");
                });
                await app.InitializeDatabaseAsync();
            }

            // Initialize JwtHelper with dependency from DI Container
            JwtHelper.Initialize(app.Services.GetRequiredService<IOptions<AppSettings>>());
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseMiddleware<TokenMiddleware>();
            app.UseMiddleware<LoggingMiddleware>();
            app.UseExceptionHandler(options => { });

            return app;
        }
    }
}