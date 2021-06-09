using System.ComponentModel;

namespace SiteInstitucional.Shared.Domain
{
    public enum ConfidentialChannelSubject
    {
        [Description("Selecione")]
        None = 0,
        [Description("Atitude antiética")]
        UnethicalAttitude = 1,
        [Description("Fraudes de qualquer natureza")]
        Fraud = 2,
        [Description("Conflito de interesses")]
        ConflictOfInterests = 3,
        [Description("Discriminação e assédio moral")]
        DiscriminationAndBullying = 4,
        [Description("Manipulação em relatórios financeiros")]
        FinancialReportingHandling = 5,
        [Description("Atividade ilegal")]
        IllegalActivity = 6,
        [Description("Atividades de conduta imprópria")]
        MisconductActivities = 7
    }
}
