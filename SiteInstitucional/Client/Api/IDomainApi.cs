using System.Threading.Tasks;

using Refit;

using SiteInstitucional.Shared.Domain;
using SiteInstitucional.Shared.Dto;

namespace SiteInstitucional.Client.Api
{
    public interface IDomainApi
    {
        [Get("/api/domain/segments")]
        Task<ApiResponse<Segment[]>> GetSegments();

        [Post("/api/domain/automakers")]
        Task<ApiResponse<Automaker[]>> GetAutomakersByParents([Body] ApplicationSearchParams search);

        [Post("/api/domain/models")]
        Task<ApiResponse<Model[]>> GetModelsByParents([Body] ApplicationSearchParams search);

        [Post("/api/domain/engine-types")]
        Task<ApiResponse<EngineType[]>> GetEngineTypesByParents([Body] ApplicationSearchParams search);

        [Post("/api/domain/applications")]
        Task<ApiResponse<Application[]>> GetApplicationsByParents([Body] ApplicationSearchParams search);

        [Get("/api/domain/application/{code}")]
        Task<ApiResponse<Application[]>> GetApplicationsByCode(string code);

        [Post("/api/domain/products")]
        Task<ApiResponse<Product[]>> GetProductsByParents([Body] ProductListSearchParams search);

        [Get("/api/domain/products/code/{code}")]
        Task<ApiResponse<Product[]>> GetProductsByCode(string code);

        [Get("/api/domain/product/detail/{code}")]
        Task<ApiResponse<Product>> GetProduct(string code);

        [Get("/api/domain/product/{code}/automakers-references")]
        Task<ApiResponse<AutomakerReference[]>> GetAutomakersReferencesByCode(string code);

        [Get("/api/domain/product/{code}/components")]
        Task<ApiResponse<string[]>> GetComponentsByCode(string code);
    }
}