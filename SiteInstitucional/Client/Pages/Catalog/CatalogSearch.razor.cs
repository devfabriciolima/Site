using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;

using SiteInstitucional.Client.Api;
using SiteInstitucional.Client.ViewModels;
using SiteInstitucional.Shared.Domain;
using SiteInstitucional.Shared.Dto;

using static System.String;

namespace SiteInstitucional.Client.Pages.Catalog
{
    public partial class CatalogSearch
    {
        private const string SelectedSearchTypeCssClass = "current";

        [Inject]
        private ILogger<CatalogSearch> Logger { get; set; }

        [Inject]
        private IDomainApi DomainApi { get; set; }

        [Inject]
        public ApplicationListContainer ApplicationListContainer { get; set; }

        [Inject]
        public ProductListContainer ProductListContainer { get; set; }

        [Inject]
        private NavigationManager Nav { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter]
        public bool IsApplicationListPage { get; set; }

        [Parameter]
        public bool IsProductListPage { get; set; }

        private string _searchByApplicationClass = SelectedSearchTypeCssClass;
        private string _searchByCodeClass = Empty;

        private Segment[] _segments;
        private string _selectedSegment;

        private bool DisableAutomaker => IsNullOrEmpty(_selectedSegment);
        private Automaker[] _automakers;
        private string _selectedAutomaker;

        private bool DisableModel => IsNullOrEmpty(_selectedAutomaker);
        private Model[] _models;
        private string _selectedModel;

        private bool DisableEngineType => IsNullOrEmpty(_selectedModel);
        private EngineType[] _engineTypes;
        private string _selectedEngineType;

        private string _code;

        async Task SearchByCodeKeyUp(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await SearchByCode();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            var response = await DomainApi.GetSegments();
            if (response.IsSuccessStatusCode)
            {
                _segments = response.Content;
                return;
            }
            Logger.LogError(response.Error, "An error occured while fetching segments.");
        }

        private void ToogleSearchType()
        {
            var temp = _searchByApplicationClass;
            _searchByApplicationClass = _searchByCodeClass;
            _searchByCodeClass = temp;
        }

        private async Task OnSelectSegment(ChangeEventArgs e)
        {
            _selectedSegment = e.Value?.ToString();
            _selectedAutomaker = _selectedModel = _selectedEngineType = null;
            _automakers = null;
            _models = null;
            _engineTypes = null;
            await LoadAutomakers();
        }

        private async Task OnSelectAutomaker(ChangeEventArgs e)
        {
            _selectedAutomaker = e.Value?.ToString();
            _selectedModel = _selectedEngineType = null;
            _models = null;
            _engineTypes = null;
            await LoadModels();
        }
        private async Task OnSelectModel(ChangeEventArgs e)
        {
            _selectedModel = e.Value?.ToString();
            _selectedEngineType = null;
            _engineTypes = null;
            await LoadEngineTypes();
        }

        private void OnSelectEngineType(ChangeEventArgs e)
        {
            _selectedEngineType = e.Value?.ToString();
        }

        private async Task LoadAutomakers()
        {
            if (DisableAutomaker)
            {
                _automakers = null;
                return;
            }
            var response = await DomainApi.GetAutomakersByParents(new ApplicationSearchParams { Segment = _selectedSegment });
            if (response.IsSuccessStatusCode)
            {
                _automakers = response.Content;
                return;
            }
            Logger.LogError(response.Error, $"An error occured while fetching automakers.");
        }

        private async Task LoadModels()
        {
            if (DisableModel)
            {
                _models = null;
                return;
            }
            var response = await DomainApi.GetModelsByParents(
                new ApplicationSearchParams
                {
                    Segment = _selectedSegment,
                    Automaker = _selectedAutomaker
                });
            if (response.IsSuccessStatusCode)
            {
                _models = response.Content;
                return;
            }
            Logger.LogError(response.Error, $"An error occured while fetching models.");
        }

        private async Task LoadEngineTypes()
        {
            if (DisableEngineType)
            {
                _engineTypes = null;
                return;
            }
            var response = await DomainApi.GetEngineTypesByParents(
                new ApplicationSearchParams
                {
                    Segment = _selectedSegment,
                    Automaker = _selectedAutomaker,
                    Model = _selectedModel
                });
            if (response.IsSuccessStatusCode)
            {
                _engineTypes = response.Content;
                return;
            }
            Logger.LogError(response.Error, $"An error occured while fetching engine types.");
        }

        private async Task SearchByApplication()
        {
            if (IsNullOrEmpty(_selectedSegment)
                || IsNullOrEmpty(_selectedAutomaker)
                || IsNullOrEmpty(_selectedModel))
            {
                return;
            }

            await ApplicationListContainer.SetParamsAsync(new ApplicationSearchParams
            {
                Segment = _selectedSegment,
                Automaker = _selectedAutomaker,
                Model = _selectedModel,
                EngineType = _selectedEngineType
            });
            if (!IsApplicationListPage)
            {
                Nav.NavigateTo("ApplicationList");
            }
        }

        private async Task SearchByCode()
        {
            if (IsNullOrEmpty(_code)) return;
            await ProductListContainer.SetParamsAsync(new ProductListSearchParams
            {
                Code = _code
            });
            if (!IsProductListPage)
            {
                Nav.NavigateTo("ProductList");
            }
        }
    }
}
