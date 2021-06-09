namespace SiteInstitucional.Shared.Dto
{
    public record ApplicationSearchParams
    {
        public string Segment { get; init; }
        public string Automaker { get; init; }
        public string Model { get; init; }
        public string EngineType { get; init; }
    }
}
