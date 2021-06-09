using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using SiteInstitucional.Client.Api;
using SiteInstitucional.Client.ViewModels;
using SiteInstitucional.Shared.Domain;
using SiteInstitucional.Shared.Dto;

namespace SiteInstitucional.Client.Pages.Catalog
{
    public partial class ApplicationList : IDisposable
    {
        [Inject]
        public ILogger<ApplicationList> Logger { get; set; }

        [Inject]
        private NavigationManager Nav { get; set; }

        [Inject]
        public IDomainApi DomainApi { get; set; }

        [Inject]
        public ApplicationListContainer ApplicationsContainer { get; set; }

        [Inject]
        public ProductListContainer ProductsContainer { get; set; }
        
        private Application[] _applications;

        protected override async Task OnInitializedAsync()
        {
            if (ApplicationsContainer.Params is null)
            {
                Nav.NavigateTo("/");
                return;
            }
            ApplicationsContainer.OnChange += SearchByApplication;
            await SearchByApplication();
        }

        private async Task SearchByApplication()
        {
            var response = await DomainApi.GetApplicationsByParents(
                new ApplicationSearchParams
                {
                    Segment = ApplicationsContainer.Params.Segment,
                    Automaker = ApplicationsContainer.Params.Automaker,
                    Model = ApplicationsContainer.Params.Model,
                    EngineType = ApplicationsContainer.Params.EngineType
                });
            if (response.IsSuccessStatusCode)
            {
                if(response.Content.Length == 1)
                {
                    await ViewProducts(response.Content.First());
                }
                _applications = response.Content;
                StateHasChanged();
                return;
            }
            Logger.LogError(response.Error, "An error occured while fetching applications.");
        }

        private async Task ViewProducts(Application application)
        {
            await ProductsContainer.SetParamsAsync(new ProductListSearchParams
            {
                Segment = application.Segment,
                Automaker = application.Automaker,
                Model = application.Model,
                EngineType = application.EngineType,
                InitialDate = application.InitialDate,
                EndDate = application.EndDate
            });
            Nav.NavigateTo("ProductList");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            ApplicationsContainer.OnChange -= SearchByApplication;
        }
    }
}
