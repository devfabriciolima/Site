using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SiteInstitucional.Server.Core.Repositories;
using SiteInstitucional.Shared.Domain;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SiteInstitucional.Server.Controllers
{
    [ApiController, Route("api/contactSubject")]
    public class ContactSubjectController
    {
        private readonly ILogger<ContactSubjectController> _logger;
        private readonly ContactSubjectRepository _db;

        public ContactSubjectController(ILogger<ContactSubjectController> logger, ContactSubjectRepository db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet("")]
        public async Task<List<ContactSubject>> GetContactSubjects()
        {
            return await _db.GetContactSubjects();
        }
    }
}
