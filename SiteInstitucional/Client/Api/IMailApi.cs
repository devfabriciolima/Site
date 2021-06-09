using System.Threading.Tasks;

using Refit;

using SiteInstitucional.Shared.Domain;

namespace SiteInstitucional.Client.Api
{
    public interface IMailApi
    {
        [Post("/api/mail/send-contact")]
        Task SendContact([Body] ContactData contact);

        [Post("/api/mail/send-confidential")]
        Task SendConfidential([Body] ConfidentialChannelData confidential);

        //[Post("/api/mail/send-idea")]
        //Task SendSuggestionOfIdea([Body] IdeaData idea);
    }
}