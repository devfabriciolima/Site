namespace SiteInstitucional.Server
{
    public class SchadekConfig
    {
        public string StaticContentDirectory { get; set; }
        public SchadekProductsConfig Products { get; set; }
        public string Downloads { get; set; }

        public bool UseDummyMailRecipient { get; set; }
        public string DummyMailRecipient { get; set; }

        public string SAP { get; set; }
    }

    public class SchadekProductsConfig
    {
        public string Thumbnails { get; set; }
        public string FullImages { get; set; }
        public string NotFound { get; set; }
    }
}
