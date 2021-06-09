using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using MimeKit;

using SiteInstitucional.Server.Core;
using SiteInstitucional.Server.Core.Repositories;
using SiteInstitucional.Shared;
using SiteInstitucional.Shared.Domain;

using System.Threading.Tasks;

namespace SiteInstitucional.Server.Controllers
{
    [ApiController, Route("api/mail")]
    public class MailController : ControllerBase
    {
        private readonly ILogger<DomainController> _logger;
        private readonly SmtpService _smtpService;
        private readonly ContactSubjectRepository _contactRepository;
        private readonly MailRecipientContactSubjectRepository _mailRecipientContactSubjectRepository;
        private readonly ContactDataRepository _contactDataRepository;
        private readonly ConfidentialChannelRepository _confidentialChannelRepository;

        public MailController(ILogger<DomainController> logger,
                              SmtpService smtpService,
                              ContactSubjectRepository contactRepository,
                              MailRecipientContactSubjectRepository mailRecipientContactSubjectRepository,
                              ContactDataRepository contactDataRepository, ConfidentialChannelRepository confidentialChannelRepository)
        {
            _logger = logger;
            _smtpService = smtpService;
            _contactRepository = contactRepository;
            _mailRecipientContactSubjectRepository = mailRecipientContactSubjectRepository;
            _contactDataRepository = contactDataRepository;
            _confidentialChannelRepository = confidentialChannelRepository;
        }

        [HttpPost("send-contact")]
        public async Task SendContact(ContactData contact)
        {
            ContactSubject contactSubject = await _contactRepository.GetById(contact.Subject.Code);
            contact.Subject = contactSubject;
            var message = new MimeMessage
            {
                Subject = $"Contato Site - {contactSubject.Description}"
            };
            await FillAddressesAsync(message, contactSubject);
            var builder = new BodyBuilder
            {
                HtmlBody = $"Nome: {contact.Name} <br> "
                           + $"Endereço: {contact.Address} <br> "
                           + $"Bairro: {contact.Neighborhood} <br> "
                           + $"Cidade: {contact.City} <br> "
                           + $"Estado: {contact.State} <br> "
                           + $"CEP: {contact.PostalCode} <br> "
                           + $"Telefone: {contact.Phone} <br> "
                           + $"Email: {contact.Mail} <br> "
                           + $"Mensagem: <br>{contact.Message}"
            };
            message.Sender = MailboxAddress.Parse(contact.Mail);
            message.Body = builder.ToMessageBody();
            await _contactDataRepository.Save(contact);
            await _smtpService.SendMessage(message);
        }

        private async Task FillAddressesAsync(MimeMessage message, ContactSubject subject)
        {
            var recipients = await _mailRecipientContactSubjectRepository.GetAllByContactSubject(subject.Code);

            foreach (var item in recipients)
            {
                if (!item.IsInCopy)
                    message.To.Add(MailboxAddress.Parse(item.MailRecipient.Email));
                else
                    message.Cc.Add(MailboxAddress.Parse(item.MailRecipient.Email));
            }
        }

        [HttpPost("send-confidential")]
        public async Task SendConfidential(ConfidentialChannelData confidential)
        {
            var subject = confidential.Subject == ConfidentialChannelSubject.None
                ? "-"
                : confidential.Subject.GetEnumDescription();
            var message = new MimeMessage
            {
                Subject = $"Canal Confidencial - Site - {subject}"
            };

            message.Sender = MailboxAddress.Parse(confidential.Mail ?? "schadek@schadek.com.br");

            var recipients = await _confidentialChannelRepository.GetRecipients();
            foreach (var recipient in recipients)
            {
                var address = MailboxAddress.Parse(recipient.Email);
                if (recipient.IsInCopy)
                {
                    message.Cc.Add(address);
                }
                else
                {
                    message.To.Add(address);
                }
            }

            var builder = new BodyBuilder
            {
                HtmlBody = $"Nome: {confidential.Name} <br> "
                           + $"Email: {confidential.Mail} <br> "
                           + $"Mensagem: <br>{confidential.Message}"
            };
            message.Body = builder.ToMessageBody();
            await _smtpService.SendMessage(message);
        }
    }
}
