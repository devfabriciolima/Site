using System;
using System.Net.Http;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using SiteInstitucional.Client.Api;
using SiteInstitucional.Client.ViewModels;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;

namespace SiteInstitucional.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = baseAddress });
            builder.Services.AddLocalization();
            
            builder.Services.AddRefit();
            builder.Services.AddRefitCustomClient<IDomainApi>(baseAddress);
            builder.Services.AddRefitCustomClient<IMailApi>(baseAddress);
            builder.Services.AddRefitCustomClient<IContactSubjectApi>(baseAddress);
            builder.Services.AddRefitCustomClient<IDepartmentApi>(baseAddress);
            builder.Services.AddRefitCustomClient<IIdeaApi>(baseAddress);

            builder.Services.AddSingleton<ApplicationListContainer>();
            builder.Services.AddSingleton<ProductListContainer>();

            builder.Services
                .AddBlazorise(options =>
                    {
                        options.ChangeTextOnKeyPress = true;
                    })
              .AddBootstrapProviders()
              .AddFontAwesomeIcons();

            var host = builder.Build();
            var jsInterop = host.Services.GetRequiredService<IJSRuntime>();
            var result = await jsInterop.InvokeAsync<string>("blazorCulture.get");
            if (result != null)
            {
                var culture = new CultureInfo(result);
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
            }

            host.Services.UseBootstrapProviders()
                .UseFontAwesomeIcons();

            await host.RunAsync();
        }
    }
}
