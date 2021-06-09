using Dapper;
using SiteInstitucional.Shared.Domain;
using System.Data;
using System.Threading.Tasks;

namespace SiteInstitucional.Server.Core.Repositories
{
    public class ContactDataRepository
    {
        private readonly IDbConnection _db;

        public ContactDataRepository(IDbConnection db)
        {
            _db = db;
        }

        internal async Task Save(ContactData contact)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Nome", contact.Name);
            parameters.Add("@Endereco", contact.Address);
            parameters.Add("@Cep", contact.PostalCode);
            parameters.Add("@Bairro", contact.Neighborhood);
            parameters.Add("@Cidade", contact.City);
            parameters.Add("@Estado", contact.State);
            parameters.Add("@Telefone", contact.Phone);
            parameters.Add("@Email", contact.Mail);
            parameters.Add("@Assunto", contact.Subject.Description);
            parameters.Add("@Mensagem", contact.Message);

            await _db.ExecuteAsync("p_InsertContatoSuporte", parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
