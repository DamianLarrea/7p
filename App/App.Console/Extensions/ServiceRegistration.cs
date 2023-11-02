using App.Core;
using App.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App.Console.Extensions
{
    internal static class ServiceRegistration
    {
        public static HostApplicationBuilder RegisterServices(this HostApplicationBuilder builder)
        {
            builder.Services.AddScoped<Program>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddHttpClient();
            builder.Services.AddMemoryCache();

            return builder;
        }
    }
}
