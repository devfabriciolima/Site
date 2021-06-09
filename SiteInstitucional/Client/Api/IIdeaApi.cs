using System.Threading.Tasks;
using Refit;
using SiteInstitucional.Shared.Domain;

namespace SiteInstitucional.Client.Api
{
    public interface IIdeaApi
    {
        [Post("/api/idea/send")]
        Task SendSuggestionOfIdea([Body] Idea idea);
    }
}
