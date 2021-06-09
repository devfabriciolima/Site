using MailKit.Net.Smtp;
using MailKit.Security;

using Microsoft.Extensions.Logging;

using MimeKit;

using System;
using System.Threading.Tasks;

using static System.String;

namespace SiteInstitucional.Server.Core
{
    public class SmtpService
    {
        private readonly SchadekConfig _config;
        private readonly SmtpConfig _smtpConfig;
        private readonly ILogger _logger;

        public SmtpService(SchadekConfig config, SmtpConfig smtpConfig, ILogger<SmtpService> logger)
        {
            _config = config;
            _smtpConfig = smtpConfig;
            _logger = logger;
        }

        public async Task SendMessage(MimeMessage message)
        {
            using var client = new SmtpClient();
            try
            {
                if (!_smtpConfig.UseSsl && !_smtpConfig.UseStartTls)
                {
                    _logger.LogDebug("SSL and TLS disabled. Bypassing validation.");
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                }

                if (_smtpConfig.UseStartTls)
                {
                    _logger.LogDebug("Using StartTls connection.");
                    await client.ConnectAsync(_smtpConfig.Server, _smtpConfig.Port, SecureSocketOptions.StartTls);
                }
                else
                {
                    _logger.LogDebug("Using SSL connection.");
                    await client.ConnectAsync(_smtpConfig.Server, _smtpConfig.Port, _smtpConfig.UseSsl);
                }

                if (!IsNullOrEmpty(_smtpConfig.Username) && !IsNullOrEmpty(_smtpConfig.Password))
                {
                    _logger.LogDebug("Authenticating user {Username}", _smtpConfig.Username);
                    await client.AuthenticateAsync(_smtpConfig.Username, _smtpConfig.Password);
                }

                if (_config.UseDummyMailRecipient && !IsNullOrEmpty(_config.DummyMailRecipient))
                {
                    message.To.Clear();
                    message.Cc.Clear();
                    message.Bcc.Clear();
                    message.To.Add(MailboxAddress.Parse(_config.DummyMailRecipient));
                }

                await client.SendAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while sending mail message.");
                throw;
            }
            finally
            {
                if (client.IsConnected)
                {
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}
