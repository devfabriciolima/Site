using Dapper;
using SiteInstitucional.Shared.Domain;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SiteInstitucional.Server.Core.Repositories
{
    public class DepartmentRepository
    {
        private readonly IDbConnection _db;

        public DepartmentRepository(IDbConnection db)
        {
            _db = db;
        }

        internal async Task<List<Department>> GetDepartments()
        {
            var result = await _db.QueryAsync<Department>($"SELECT Nome Name, Contato Email FROM DEPARTAMENTO");

            return result.ToList();
        }
    }
}
