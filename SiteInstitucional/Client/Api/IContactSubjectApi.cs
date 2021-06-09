using Refit;
using SiteInstitucional.Shared.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteInstitucional.Client.Api
{
    public interface IContactSubjectApi
    {
        [Get("/api/contactSubject")]
        Task<ApiResponse<List<ContactSubject>>> GetContactSubjects();
    }
}
