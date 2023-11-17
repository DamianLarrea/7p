using App.Core;
using App.Data;
using App.Data.Caching;
using App.Data.Options;
using Microsoft.Extensions.Configuration;
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
            builder.Services.AddScoped<ICache, MemoryCache>();

            builder.Services.AddMemoryCache();

            AddApiClient(builder);

            return builder;
        }

        private static void AddApiClient(this HostApplicationBuilder builder)
        {
            var opts = builder.Configuration.GetSection(nameof(UserApiOptions)).Get<UserApiOptions>();
            builder.Services.AddHttpClient<IApiClient, Apiclient>(client =>
            {
                client.BaseAddress = new Uri(opts.Url);
                client.Timeout = TimeSpan.FromMilliseconds(opts.RequestTimeoutInMs);
            });
        }
    }
}
