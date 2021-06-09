using System;
using System.Linq;
using System.Threading.Tasks;

using Blazorise;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using Refit;

using SiteInstitucional.Client.Api;
using SiteInstitucional.Client.ViewModels;
using SiteInstitucional.Shared.Domain;
using SiteInstitucional.Shared.Dto;

using static System.String;

namespace SiteInstitucional.Client.Pages.Catalog
{
    public partial class ProductsList : IDisposable
    {
        [Inject]
        public ILogger<ProductsList> Logger { get; set; }

        [Inject]
        public NavigationManager Nav { get; set; }

        [Inject]
        public IDomainApi DomainApi { get; set; }

        [Inject]
        public ProductListContainer ProductsContainer { get; set; }
        
        public string SearchParamsPath => !IsNullOrEmpty(ProductsContainer.Params.Code)
            ? $"Código: {ProductsContainer.Params.Code}"
            : $"{ProductsContainer.Params?.Segment ?? "-"} > "
              + $"{ProductsContainer.Params?.Automaker ?? "-"} > "
              + $"{ProductsContainer.Params?.Model ?? "-"} > "
              + $"{ProductsContainer.Params?.EngineType ?? "-"} > "
              + $"{ProductsContainer.Params?.ProductionPeriod ?? "-"}";

        private Product[] _products;
        private Modal _modalRef;
        private string _imageUri;

        protected override async Task OnInitializedAsync()
        {
            if (ProductsContainer.Params is null)
            {
                Nav.NavigateTo("/");
                return;
            }
            ProductsContainer.OnChange += Search;
            await Search();
        }

        private async Task Search()
        {
            ApiResponse<Product[]> response;
            if (!IsNullOrEmpty(ProductsContainer.Params.Code))
            {
                response = await DomainApi.GetProductsByCode(ProductsContainer.Params.Code);
            }
            else
            {
                response = await DomainApi.GetProductsByParents(
                    new ProductListSearchParams
                    {
                        Segment = ProductsContainer.Params.Segment,
                        Automaker = ProductsContainer.Params.Automaker,
                        Model = ProductsContainer.Params.Model,
                        EngineType = ProductsContainer.Params.EngineType,
                        InitialDate = ProductsContainer.Params.InitialDate,
                        EndDate = ProductsContainer.Params.EndDate
                    });
            }

            if (response.IsSuccessStatusCode)
            {
                if (response.Content.Length == 1)
                {
                    DetailProduct(response.Content.First());
                }
                _products = response.Content;
                StateHasChanged();
                return;
            }
            Logger.LogError(response.Error, "An error occured while fetching products.");
        }

        private void ViewImage(Product product)
        {
            if (_modalRef is null) return;

            _imageUri = $"/api/static/products-images/{product.Code}";
            _modalRef.Show();
        }

        private void DetailProduct(Product product)
        {
            Nav.NavigateTo($"ProductDetail/{product.Code}");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            ProductsContainer.OnChange -= Search;
        }
    }
}
