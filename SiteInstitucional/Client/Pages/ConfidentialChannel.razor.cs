using Microsoft.AspNetCore.Components;

using SiteInstitucional.Client.Api;
using SiteInstitucional.Client.Shared;
using SiteInstitucional.Shared.Domain;

using System.Threading.Tasks;

namespace SiteInstitucional.Client.Pages
{
    public partial class ConfidentialChannel
    {
        [Inject]
        public IMailApi MailApi { get; set; }

        private ConfidentialChannelData _model = new ConfidentialChannelData { Subject = ConfidentialChannelSubject.None };
        private MessageModal _messageModal;

        private bool _sendingMessage;

        private async Task SendMail()
        {
            try
            {
                _sendingMessage = true;
                await MailApi.SendConfidential(_model);
                _messageModal.Message = Localizer["Mensagem enviada com sucesso!"];
                _messageModal.Open();
                _model = new ConfidentialChannelData { Subject = ConfidentialChannelSubject.None };
            }
            finally
            {
                _sendingMessage = false;
            }
        }
    }
}
