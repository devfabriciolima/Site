using static System.String;

namespace SiteInstitucional.Shared.Domain
{
    public record Product
    {
        public string Code { get; init; }
        public string Name { get; init; }
        public string NameComplement { get; init; }
        public bool HasNameComplement => !IsNullOrEmpty(NameComplement);
        public bool HasComponents { get; init; }
        public string OldCode { get; set; }
        public bool HasOldCode => !IsNullOrEmpty(OldCode);
        public string Package { get; set; }
        public string PackageContent { get; set; }
        public string BarCode { get; set; }
        public string TaxCode { get; set; }
    }
}
