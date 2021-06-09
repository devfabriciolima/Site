namespace SiteInstitucional.Shared.Domain
{
    public record ProductType
    {
        public string Code { get; init; }
        public string Segment { get; init; }
        public string Automaker { get; init; }
        public string Model { get; init; }
        public string EngineType { get; init; }
        public string Name { get; init; }
    }
}
