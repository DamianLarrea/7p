using App.Data;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class OptionsRegistration
    {
        public static HostApplicationBuilder ConfigureOptions(this HostApplicationBuilder builder)
        {
            builder.Services.Configure<UserApiOptions>(builder.Configuration.GetSection(nameof(UserApiOptions)));

            return builder;
        }
    }
}
