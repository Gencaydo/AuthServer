namespace AuthServer.API.Extensions
{
    public static class CorsPolicyExtensions
    {
        public static IServiceCollection AddCorsPolicies(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("LocalhostPolicy", policy =>
                {
                    policy.WithOrigins("http://angulartest.gencaydoyurucu.com.tr", "http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            return services;
        }
    }
}
