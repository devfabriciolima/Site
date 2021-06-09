using Dapper;
using SiteInstitucional.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SiteInstitucional.Server.Core.Repositories
{
    public class ContactSubjectRepository
    {
        private readonly IDbConnection _db;

        public ContactSubjectRepository(IDbConnection db)
        {
            _db = db;
        }

        internal async Task<List<ContactSubject>> GetContactSubjects()
        {
            var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var languageDescription = culture == "pt" ? "Descricao" : culture == "en" ? "Descricao_english" : "Descricao_espanol";
            var result = await _db.QueryAsync<ContactSubject>($"SELECT Codigo as Code, {languageDescription} as Description FROM ASSUNTO_CONTATO");

            return result.ToList();
        }

        internal async Task<ContactSubject> GetById(int code)
        {
            var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var languageDescription = culture == "pt" ? "Descricao" : culture == "en" ? "Descricao_english" : "Descricao_espanol";
            var result = await _db.QueryAsync<ContactSubject>($"SELECT Codigo as Code, {languageDescription} as Description FROM ASSUNTO_CONTATO WHERE Codigo = {code}");

            return result.FirstOrDefault();
        }
    }
}
