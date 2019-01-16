using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SudokuApp.Common.Configuration.Options;

namespace SudokuApp.Api.Extensions
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Добавление конфигураций.
        /// </summary>
        public static IServiceCollection AddOptionsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSection<PageNotFoundHandlingMiddlewareOptions>(configuration);
            services.AddSection<ExceptionHandlingMiddlewareOptions>(configuration);
            services.AddSection<ServiceNameOptions>(configuration);

            return services;
        }

        public static IServiceCollection AddSection<T>(this IServiceCollection services, IConfiguration configuration, string name = null)
            where T : class
        {
            services.Configure<T>(opts => configuration.Bind(name ?? typeof(T).Name, opts));

            return services;
        }
    }
}
