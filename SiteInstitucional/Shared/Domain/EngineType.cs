namespace SiteInstitucional.Shared.Domain
{
    public record EngineType
    {
        public string Segment { get; init; }
        public string Automaker { get; init; }
        public string Model { get; init; }
        public string Name { get; init; }
    }
}
