using Microsoft.AspNetCore.Identity;
using Nemetschek.Data.Models.usr;
using Nemetschek.Services.Interfaces;
using Nemetschek.Services;
using Nemetschek.Data.Repositories.Interfaces;
using Nemetschek.Data.Repositories;
using Nemetschek.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.RateLimiting;

namespace Nemetschek.API.User.Extensions
{
    /// <summary>
    /// Static class containing extension methods for registering services against an <see cref="IServiceCollection"/> type.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Extension method that adds data retrieval services to the <see cref="IServiceCollection"/> in scope.
        /// </summary>
        /// <param name="services">The current <see cref="IServiceCollection"/> in scope.</param>
        public static void AddDataServices(this IServiceCollection services)
        {
            AddRepositories(services);
            AddServices(services);
        }

        /// <summary>
        /// Function that ads all repositories to the <see cref="IServiceCollection"/> in scope.
        /// </summary>
        /// <param name="services"></param>
        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<INemetschekRepo, NemetschekRepo>();
        }

        /// <summary>
        /// Function that adds all services to the <see cref="IServiceCollection"/> in scope.
        /// </summary>
        /// <param name="services"></param>
        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPasswordHasher<Nemetschek.Data.Models.usr.User>, PasswordHasher<Nemetschek.Data.Models.usr.User>>();
        }

        /// <summary>
        /// Function that adds the <see cref="AppDbContext"/> to the <see cref="IServiceCollection"/> in scope, using the connection string from the configuration.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        }

        /// <summary>
        /// Function that adds custom Swagger configuration to the <see cref="IServiceCollection"/> in scope.
        /// </summary>
        /// <param name="services"></param>
        public static void AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });
                c.SchemaFilter<ClearStringExampleFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    }] = Array.Empty<string>()
                });
            });
        }

        /// <summary>
        /// Function that adds JWT authentication to the <see cref="IServiceCollection"/> in scope, using the configuration settings for JWT.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                    };
                });

            services.AddAuthorization();
        }

        /// <summary>
        /// Function that adds a rate limiting policy to the <see cref="IServiceCollection"/> in scope, which limits requests based on the client's IP address.
        /// </summary>
        /// <param name="services"></param>
        public static void AddRateLimitingPolicy(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddPolicy("PerIpPolicy", httpContext =>
                {
                    var clientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                    return RateLimitPartition.GetTokenBucketLimiter(
                      partitionKey: clientIp,
                      factory: _ => new TokenBucketRateLimiterOptions
                      {
                          TokenLimit = 30,            // max tokens in bucket
                          QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                          QueueLimit = 0,             // no queuing—drop immediately if empty
                          ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                          TokensPerPeriod = 30,
                          AutoReplenishment = true
                      });
                });
            });
        }

    }
}
