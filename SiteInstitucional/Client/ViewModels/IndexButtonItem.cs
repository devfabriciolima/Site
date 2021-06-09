namespace SiteInstitucional.Client.ViewModels
{
    public record IndexButtonItem
    {
        public string Image { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public string Link { get; init; }

        public static IndexButtonItem[] GetItems() => new IndexButtonItem[]
        {
            new IndexButtonItem
            {
                Image = "button01.jpg",
                Title = "TECNOLOGIA",
                Description = "Excelência operacional, processos avançados e reengenharia de ponta",
                Link = "Technology"
            },
            new IndexButtonItem
            {
                Image = "button02.jpg",
                Title = "PRODUTOS",
                Description = "O maior portfólio de bombas automotivas do mercado",
                Link = "Products"
            },
            new IndexButtonItem
            {
                Image = "button03.jpg",
                Title = "GARANTIA",
                Description = "Instruções de instalação e condições de garantia",
                Link = "Warranty"
            },
            new IndexButtonItem
            {
                Image = "button04.jpg",
                Title = "SUPORTE",
                Description = "Esclareça suas dúvidas com nossa equipe de Assistência Técnica",
                Link = "Contact"
            }
        };
    }
}