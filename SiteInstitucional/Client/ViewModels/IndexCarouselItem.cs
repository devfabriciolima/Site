namespace SiteInstitucional.Client.ViewModels
{
    public record IndexCarouselItem
    {
        public string Image { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public string Link { get; init; }

        public static IndexCarouselItem[] GetItems() => new IndexCarouselItem[]
        {
            new IndexCarouselItem
            {
                Image="05.jpg",
                //Title="QUER ENCONTRAR A BOMBA SCHADEK NA PALMA DA MÃO?",
                //Description = "Baixe o app no seu celular. É rápido, fácil e grátis."
            },
            new IndexCarouselItem
            {
                Image="06.jpg",
                //Title="A SCHADEK EVOLUI COM VOCÊ",
                //Description = "NOVA IDENTIDADE VISUAL, NOVAS EMBALAGENS, A QUALIDADE DE SEMPRE",
                //Link = "#qualityDiv"
            },
            new IndexCarouselItem
            {
                Image="07.jpg",
                //Title="AMPLIAMOS O PORTFÓLIO E AS VANTAGENS",
                //Description = "NOVAS BOMBAS D’ÁGUA E DE COMBUSTÍVEL COM PREÇOS MAIS COMPETITIVOS",
                //Link = "#newLineDiv"
            },
            new IndexCarouselItem
            {
                Image="04.jpg",
                //Title="LANÇAMENTO",
                //Description = "O Maior portfólio de bombas d'água do mercado",
                //Link = "#newProductsDiv"
            }
        };
    }
}
