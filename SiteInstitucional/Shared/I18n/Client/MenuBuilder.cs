using System.Collections.Generic;

namespace SiteInstitucional.Shared.I18n.Client
{
    public static class MenuBuilder
    {
        public static MenuItem[] Build() => new[]
        {
            new MenuItem("Company", "Empresa", "Trajetória", "Linha do tempo", "Hoje", "Certificados", "Responsabilidade Socioambiental"),
            new MenuItem("Technology", "Tecnologia"),
            new MenuItem("Plant", "Fábrica"),
            new MenuItem("Products", "Produtos", "Bomba d’água", "Bomba de óleo", "Bomba de combustível", "Tubos de sucção", "Válvulas de alívio", "Kit de distribuição", "Polias e Tensionadores"),
            new MenuItem("Warranty", "Garantia"),
            new MenuItem("Contact", "Suporte", "Central de atendimento", "Endereço", "Trabalhe conosco", new KeyValuePair<string,string>("ConfidentialChannel", "Canal Confidencial")),
            new MenuItem("Downloads", "Downloads")
        };
    }
}
