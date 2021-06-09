namespace SiteInstitucional.Shared.Domain
{
    public class MailRecipientContactSubject
    {
        public int MailRecipientCode { get; set; }
        public int ContactSubjectCode { get; set; }
        public bool IsInCopy { get; set; }

        public MailRecipient MailRecipient { get; set; }
        public ContactSubject ContactSubject { get; set; }
    }
}
