using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using MimeKit;

using SiteInstitucional.Server.Core;
using SiteInstitucional.Server.Core.Repositories;
using SiteInstitucional.Shared.Domain;

namespace SiteInstitucional.Server.Controllers
{
    [ApiController, Route("api/idea")]
    public class IdeaController
    {
        private readonly ILogger<DepartmentController> _logger;
        private readonly DepartmentRepository _departmentRepository;
        private readonly SmtpService _smtpService;
        private readonly IdeaRepository _ideaRepository;

        public IdeaController(ILogger<DepartmentController> logger, DepartmentRepository departmentRepository, SmtpService smtpService, IdeaRepository ideaRepository)
        {
            _logger = logger;
            _departmentRepository = departmentRepository;
            _smtpService = smtpService;
            _ideaRepository = ideaRepository;
        }

        [HttpPost("send")]
        public async Task SendSuggestionOfIdea(Idea idea)
        {
            var departments = await _departmentRepository.GetDepartments();
            var recipients = departments.Where(d =>
                d.Name.Equals(idea.Department, StringComparison.InvariantCultureIgnoreCase));

            var message = new MimeMessage
            {
                Subject = "Sugestão de Ideia - Site",
                Sender = MailboxAddress.Parse("schadek@schadek.com.br")
            };
            message.Cc.Add(MailboxAddress.Parse("isaac.santos@schadek.com.br"));
            message.Cc.Add(MailboxAddress.Parse("roberto.affonso@schadek.com.br"));
            foreach (var recipient in recipients)
            {
                message.To.Add(MailboxAddress.Parse(recipient.Email));
            }

            var builder = new BodyBuilder
            {
                HtmlBody = $"SETOR DE APLICAÇÃO DA MELHORIA: {idea.Department} <br> "
                           + $"NOME DO COLABORADOR: {idea.CollaboratorName} <br> "
                           + $"MATRÍCULA DO COLABORADOR: {idea.CollaboratorRegistration} <br> "
                           + $"SETOR DO COLABORADOR: {idea.CollaboratorDepartment} <br> "
                           + "<br><br> "
                           + "FORMULÁRIO: <br> "
                           + $"Qual é minha ideia? <br> {idea.MyIdea ?? "N/A"} <br> "
                           + "<br> "
                           + $"Por que minha ideia é aplicável? <br> {idea.WhyApplicable ?? "N/A"} <br> "
                           + "<br> "
                           + $"Como minha ideia pode ser medida após ser aplicada? <br> {idea.HowMeasured ?? "N/A"}"
            };

            idea.Attachments?.ForEach(a =>
            {
                builder.Attachments.Add(a.Filename, a.Bytes, ContentType.Parse(a.ContentType));
            });

            message.Body = builder.ToMessageBody();
            await _smtpService.SendMessage(message);
            await _ideaRepository.Save(idea);
        }
    }
}
