using System.ComponentModel.DataAnnotations;

namespace SiteInstitucional.Shared.Domain
{
    public record ContactData
    {
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        [Required, EmailAddress]
        public string Mail { get; set; }
        public ContactSubject Subject { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
