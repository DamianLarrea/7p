using App.Core;
using App.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            return builder;
        }
    }
}
