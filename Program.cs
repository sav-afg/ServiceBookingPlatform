using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using ServiceBookingPlatform.Data;
using ServiceBookingPlatform.Services;
using System.Text;
namespace ServiceBookingPlatform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            
            // Configure OpenAPI with JWT Security
            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    // Create JWT Bearer security scheme
                    var securityScheme = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Description = "JWT Authorization header using the Bearer scheme."
                    };

                    // Initialize Components and SecuritySchemes if null
                    document.Components ??= new OpenApiComponents();

                    if (document.Components.SecuritySchemes == null)
                    {
                        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>();
                    }

                    // Add security scheme to document
                    document.Components.SecuritySchemes["Bearer"] = securityScheme;

                    // Create security scheme reference for requirement
                    var schemeReference = new OpenApiSecuritySchemeReference("Bearer", document);

                    // Create and add security requirement to all operations
                    foreach (var path in document.Paths.Values)
                    {
                        foreach (var operation in path.Operations!.Values)
                        {
                            // Initialize Security collection if null
                            operation.Security ??= [];

                            var securityRequirement = new OpenApiSecurityRequirement
                            {
                                [schemeReference] = []
                            };
                            operation.Security.Add(securityRequirement);
                        }
                    }

                    return Task.CompletedTask;
                });
            });

            builder.Services.AddScoped<IUserBookingService, UserBookingService>();
            builder.Services.AddScoped<IUserRegistrationService, UserRegistrationService>();
            builder.Services.AddScoped<IUserLogInService, UserLogInService>();
            builder.Services.AddScoped<IUserServiceService, UserServiceService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IRefreshService, RefreshService>();

            /* Database Configuration - Integration Test Support
             Conditionally register database provider based on environment
             When Environment = "Testing", this registration is skipped,
             allowing integration tests to register InMemory database provider
             This prevents the "dual database provider" error in WebApplicationFactory tests*/

            if (builder.Environment.EnvironmentName != "Testing")
            {
                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            }
            // If environment is "Testing", the DbContext is registered by test setup with InMemory provider

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
                    ValidAudience = builder.Configuration["JwtConfig:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]!)),
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options
                        .WithTitle("Service Booking Platform API")
                        .WithTheme(ScalarTheme.Purple)
                        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();

        }
    }
}
