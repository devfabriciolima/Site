using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SiteInstitucional.Server.Core.Repositories;
using SiteInstitucional.Shared.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteInstitucional.Server.Controllers
{
    [ApiController, Route("api/department")]
    public class DepartmentController
    {
        private readonly ILogger<DepartmentController> _logger;
        private readonly DepartmentRepository _db;

        public DepartmentController(ILogger<DepartmentController> logger, DepartmentRepository db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet("")]
        public async Task<List<Department>> GetDepartments()
        {
            return await _db.GetDepartments();
        }
    }
}
