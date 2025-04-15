using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using AuthServer.Data.Repositories;
using AuthServer.Data.UnitOfWork;
using AuthServer.Data;
using AuthServer.Service.Services;
using AuthServer.Service;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace AuthServer.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MapProfile));
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IServiceGeneric<,>), typeof(GenericService<,>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("PgSqlCon");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new NullReferenceException("Connection string is null");
                }

                options.UseNpgsql(connectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                });
            });

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisConnection = configuration.GetConnectionString("Redis");
                if (string.IsNullOrWhiteSpace(redisConnection))
                {
                    throw new InvalidOperationException("Redis connection string is not configured.");
                }

                return ConnectionMultiplexer.Connect(redisConnection);
            });

            return services;
        }
    }
}
