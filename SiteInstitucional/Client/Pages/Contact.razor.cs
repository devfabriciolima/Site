using SiteInstitucional.Shared.Domain;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SiteInstitucional.Client.Api;
using SiteInstitucional.Client.Shared;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace SiteInstitucional.Client.Pages
{
    public partial class Contact
    {
        [Inject]
        public IMailApi MailApi { get; set; }

        [Inject]
        private IContactSubjectApi ContactSubjectApi { get; set; }

        [Inject]
        private ILogger<Contact> Logger { get; set; }

        private ContactData _model = new ContactData { Subject = new() { Code = 0 } };
        private List<ContactSubject> _listContactSubject = new();
        private MessageModal _messageModal;

        private bool _sendingMessage;

        protected override async Task OnInitializedAsync()
        {
            var response = await ContactSubjectApi.GetContactSubjects();
            if (response.IsSuccessStatusCode)
            {
                _listContactSubject = response.Content;
                return;
            }
            Logger.LogError(response.Error, "An error occured while fetching contact subjects.");
        }

        private async Task SendMail()
        {
            try
            {
                _sendingMessage = true;
                await MailApi.SendContact(_model);
                _messageModal.Message = Localizer["Mensagem enviada com sucesso!"];
                _messageModal.Open();
                _model = new ContactData { Subject = new() { Code = 0 } };
            }
            finally
            {
                _sendingMessage = false;
            }
        }
    }
}
