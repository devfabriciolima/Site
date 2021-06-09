using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SiteInstitucional.Server
{
    public static class StartupConfigExtensions
    {
        public static T AddConfigurationSection<T>(this IServiceCollection services, IConfiguration configuration, string key) where T : class
        {
            var configSection = configuration.GetSection(key).Get<T>();
            if (configSection is null) return null;

            services.AddSingleton(configSection);
            return configSection;
        }
    }
}
