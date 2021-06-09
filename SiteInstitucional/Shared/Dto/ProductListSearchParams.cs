namespace SiteInstitucional.Shared.Dto
{
    public record ProductListSearchParams : ApplicationSearchParams
    {
        public string Code { get; set; }

        public string InitialDate { get; init; }
        public string EndDate { get; init; }
        public string ProductionPeriod => $"{InitialDate} - {EndDate ?? "-"}";
    }
}
