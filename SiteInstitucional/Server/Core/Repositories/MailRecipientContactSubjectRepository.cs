using Dapper;
using SiteInstitucional.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SiteInstitucional.Server.Core.Repositories
{
    public class MailRecipientContactSubjectRepository
    {
        private readonly IDbConnection _db;

        public MailRecipientContactSubjectRepository(IDbConnection db)
        {
            _db = db;
        }

        internal async Task<List<MailRecipientContactSubject>> GetAllByContactSubject(int ContactSubjectCode)
        {
            var result = await _db.QueryAsync<MailRecipientContactSubject>($"SELECT CodigoDestinatarioEmail as MailRecipientCode, " +
                $"CodigoAssuntoContato as ContactSubjectCode, " +
                $"EmCopia as IsInCopy FROM DESTINATARIO_EMAIL_ASSUNTO_CONTATO " +
                $"WHERE CodigoAssuntoContato = {ContactSubjectCode}");

            var resultMailRecipient = await _db.QueryAsync<MailRecipient>($"SELECT Email as Email, " +
                $"Codigo as Code " +
                $"FROM DESTINATARIO_EMAIL " +
                $"WHERE Codigo in ({string.Join(',', result.Select(x => x.MailRecipientCode))})");

            foreach (var item in result)
            {
                item.MailRecipient = resultMailRecipient.FirstOrDefault(x => x.Code == item.MailRecipientCode);
            }

            return result.ToList();
        }
    }
}
