using Refit;
using SiteInstitucional.Shared.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteInstitucional.Client.Api
{
    public interface IDepartmentApi
    {
        [Get("/api/department")]
        Task<ApiResponse<List<Department>>> GetDepartments();
    }
}