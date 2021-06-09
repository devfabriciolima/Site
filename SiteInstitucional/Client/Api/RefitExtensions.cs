using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace SiteInstitucional.Client.Api
{
    public static class RefitExtensions
    {
        public static void AddRefit(this IServiceCollection services)
        {
            services.AddScoped<CustomHttpClientHandler>();
        }

        public static IHttpClientBuilder AddRefitCustomClient<T>(
            this IServiceCollection services, string baseAddress, RefitSettings settings = null) where T : class =>
            services.AddRefitCustomClient<T>(new Uri(baseAddress), settings);

            public static IHttpClientBuilder AddRefitCustomClient<T>(
                this IServiceCollection services, Uri baseAddress, RefitSettings settings = null) where T : class
        {
            return services.AddRefitClient<T>(settings)
                .AddTypedClient((client, serviceProvider) =>
                {
                    var httpClientHandler = serviceProvider.GetRequiredService<CustomHttpClientHandler>();
                    var httpClient = new HttpClient(httpClientHandler)
                    {
                        BaseAddress = baseAddress
                    };
                    var requestBuilder =
                        serviceProvider.GetService<IRequestBuilder<T>>();
                    return RestService.For(httpClient, requestBuilder);
                });
        }
    }
}
