namespace SiteInstitucional.Shared.Domain
{
    public record Application
    {
        public string Segment { get; init; }
        public string Automaker { get; init; }
        public string Model { get; init; }
        public string EngineType { get; init; }
        public string InitialDate { get; init; }
        public string EndDate { get; init; }
        public string Comments { get; init; }

        public string ProductionPeriod => $"{InitialDate} - {EndDate ?? "-"}";
    }
}
