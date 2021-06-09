using System.Threading.Tasks;

using Blazorise;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using SiteInstitucional.Client.Api;
using SiteInstitucional.Shared.Domain;

namespace SiteInstitucional.Client.Pages.Catalog
{
    public partial class ProductDetail
    {
        [Parameter]
        public string Code { get; set; }

        [Inject]
        public ILogger<ProductsList> Logger { get; set; }
        
        [Inject]
        public IDomainApi DomainApi { get; set; }

        private Product _product;
        private Modal _modalRef;
        private string _imageUri;
        private Application[] _applications;
        private AutomakerReference[] _automakersReferences;
        private string[] _components;

        protected override async Task OnInitializedAsync()
        {
            await GetProduct();
            await GetApplications();
            await GetAutomakersReferences();
            await GetComponents();
        }

        private void ViewImage(Product product)
        {
            if (_modalRef is null) return;

            _imageUri = $"/api/static/products-images/{product.Code}";
            _modalRef.Show();
        }

        private async Task GetProduct()
        {
            var response = await DomainApi.GetProduct(Code);
            if (response.IsSuccessStatusCode)
            {
                _product = response.Content;
                StateHasChanged();
                return;
            }
            Logger.LogError(response.Error, "An error occured while fetching product.");
        }

        private async Task GetApplications()
        {
            var response = await DomainApi.GetApplicationsByCode(Code);
            if (response.IsSuccessStatusCode)
            {
                _applications = response.Content;
                StateHasChanged();
                return;
            }
            Logger.LogError(response.Error, "An error occured while fetching applications.");
        }

        private async Task GetAutomakersReferences()
        {
            var response = await DomainApi.GetAutomakersReferencesByCode(Code);
            if (response.IsSuccessStatusCode)
            {
                _automakersReferences = response.Content;
                StateHasChanged();
                return;
            }
            Logger.LogError(response.Error, "An error occured while fetching automakers references.");
        }

        private async Task GetComponents()
        {
            if (!_product.HasComponents)
            {
                return;
            }
            var response = await DomainApi.GetComponentsByCode(Code);
            if (response.IsSuccessStatusCode)
            {
                _components = response.Content;
                StateHasChanged();
                return;
            }
            Logger.LogError(response.Error, "An error occured while fetching components.");
        }
    }
}
