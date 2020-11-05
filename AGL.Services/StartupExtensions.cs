using System;
using AGL.Services.Contracts;
using AGL.Services.HelperClasses;
using AGL.Services.Implementation;
using AGL.Services.Options;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AGL.Services
{
    public static class StartupExtensions
    {
        /// <summary>
        /// Add a new configuration for environment.
        /// </summary>
        /// <param name="builder">The configuration to read from.</param>
        /// <param name="contentRootPath">Path for Appsetting.json</param>
        /// <param name="environmentName">Environment Name (Development, QA, UAT or Production)</param>
        /// <returns>The same Microsoft.Extensions.Configuration.IConfigurationBuilder.</returns>
        public static IConfigurationBuilder ConfigureEnvironmnt(this IConfigurationBuilder builder, string contentRootPath, string environmentName)
        {
            // Environment specific appSettings file to load
            builder.SetBasePath(contentRootPath)
                .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder;
        }

        /// <summary>
        /// Configures HttpClients, ServiceLayer and other services.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
        /// <returns>The Microsoft.Extensions.DependencyInjection.IServiceCollection so that additional calls can be chained.</returns>
        public static IServiceCollection ConfigureAPIs(this IServiceCollection services, IConfiguration configuration)
        {
            configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            services
                .ConfigureHttpClient(configuration) // Configure HttpClients
                .ConfigureServiceLayer();           // Configure Dependency Injection

            return services;
        }

        /// <summary>
        /// Adds the System.Net.Http.IHttpClientFactory and related other Api services to the Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
        /// <returns>The Microsoft.Extensions.DependencyInjection.IServiceCollection so that additional calls can be chained.</returns>
        private static IServiceCollection ConfigureHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            // Using HttpClientFactory Directly
            services.AddHttpClient();

            ApiOptions apiOptions = AppSettingsHelper.RetrieveSection<ApiOptions>(configuration, "Apis:People");
            if (apiOptions != null)
            {
                services.AddHttpClient("PeopleList", client =>
                {
                    client.BaseAddress = new Uri(apiOptions.Url);
                });
            }

            return services;
        }

        /// <summary>
        /// Adds a transient service of the type specified in TService with an implementation
        /// type specified in TImplementation to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <returns>The Microsoft.Extensions.DependencyInjection.IServiceCollection so that additional calls can be chained.</returns>
        private static IServiceCollection ConfigureServiceLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(StartupExtensions));

            // For Services
            services.AddTransient<IHttpService, HttpService>();
            services.AddTransient<IPeopleService, PeopleService>();

            return services;
        }
    }
}
