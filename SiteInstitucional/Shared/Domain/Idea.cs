using System.Collections.Generic;

namespace SiteInstitucional.Shared.Domain
{
    public class Idea
    {
        public string Department { get; set; }
        public string CollaboratorRegistration { get; set; }
        public string CollaboratorName { get; set; }
        public string CollaboratorDepartment { get; set; }
        public string MyIdea { get; set; }
        public string WhyApplicable { get; set; }
        public string HowMeasured { get; set; }

        public List<IdeaAttachment> Attachments { get; set; }

        public Idea()
        {
            Attachments = new();
        }
    }
}
