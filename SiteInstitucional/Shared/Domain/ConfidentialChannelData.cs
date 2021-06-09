using System.ComponentModel.DataAnnotations;

namespace SiteInstitucional.Shared.Domain
{
    public record ConfidentialChannelData
    {
        public string Name { get; set; }
        [EmailAddress]
        public string Mail { get; set; }
        public ConfidentialChannelSubject Subject { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
