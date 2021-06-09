using Dapper;

using SiteInstitucional.Shared.Domain;

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SiteInstitucional.Server.Core.Repositories
{
    public class ConfidentialChannelRepository
    {
        private readonly IDbConnection _db;

        public ConfidentialChannelRepository(IDbConnection db)
        {
            _db = db;
        }

        internal async Task<List<ConfidentialChannelRecipient>> GetRecipients() =>
            (await _db.QueryAsync<ConfidentialChannelRecipient>(
                "SELECT Codigo as Code, Email, EmCopia as IsInCopy FROM CANAL_CONFIDENCIAL"))
            .ToList();
    }
}
