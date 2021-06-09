namespace SiteInstitucional.Shared.Domain
{
    public class IdeaAttachment
    {
        public int IdeaId { get; set; }
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public byte[] Bytes { get; set; }

        public IdeaAttachment() { }

        public IdeaAttachment(string filename, string contentType, byte[] bytes)
        {
            Filename = filename;
            ContentType = contentType;
            Bytes = bytes;
        }
    }
}
