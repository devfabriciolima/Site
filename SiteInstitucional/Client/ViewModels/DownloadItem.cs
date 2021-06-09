using System;

namespace SiteInstitucional.Client.ViewModels
{
    public record DownloadItem
    {
        public Guid Id { get; set; }
        public string Type { get; init; }
        public string Name { get; init; }
        public string Thumbnail { get; init; }
        public string Filename { get; init; }

        public DownloadItem() => Id = Guid.NewGuid();
        public DownloadItem(Guid id) => Id = id;

        public static DownloadItem[] GetItems() => new DownloadItem[]
        {
            new()
            {
                Type = "Catálogo",
                Name = "Catálogo Schadek 2017",
                Thumbnail = "Capa_Catalogo_Schadek.jpg",
                Filename = "Catalogo-Schadek-2017.pdf"
            },
            new()
            {
                Type = "Catálogo",
                Name = "Catálogo de Produtos Kit de Distribuição",
                Thumbnail = "CatalogoProdutos_KitDistribuicao.png",
                Filename = "CatalogoProdutos_KitDistribuicao.pdf"
            },
            new()
            {
                Type = "Treinamento",
                Name = "Arrefecimento de motores a combustão",
                Thumbnail = "cartilhas-img.jpg",
                Filename = "arrefecimento-de-motores-a-combustao.pdf"
            },
            new()
            {
                Type = "Treinamento",
                Name = "Atrito e torque",
                Thumbnail = "cartilhas-img2.jpg",
                Filename = "atrito-e-torque.pdf"
            },
            new()
            {
                Type = "Treinamento",
                Name = "Bomba d'água - cuidados na aplicação",
                Thumbnail = "cartilhas-img3.jpg",
                Filename = "bomba_dagua-aplicacao.pdf"
            },
            new()
            {
                Type = "Treinamento",
                Name = "Bomba de óleo - cuidados na aplicação",
                Thumbnail = "cartilhas-img4.jpg",
                Filename = "bomba_de_oleo-aplicacao.pdf"
            },
            new()
            {
                Type = "Treinamento",
                Name = "Elementos de vedação",
                Thumbnail = "cartilhas-img5.jpg",
                Filename = "elementos-de-vedacao.pdf"
            },
            new()
            {
                Type = "Treinamento",
                Name = "Lubrificação de motores a combustão",
                Thumbnail = "cartilhas-img6.jpg",
                Filename = "lubrificacao-de-motores.pdf"
            },
            new()
            {
                Type = "Treinamento",
                Name = "Manutenção preventiva",
                Thumbnail = "cartilhas-img7.jpg",
                Filename = "manutencao-preventiva.pdf"
            },
            new()
            {
                Type = "Treinamento",
                Name = "Roscas para tubos",
                Thumbnail = "cartilhas-img8.jpg",
                Filename = "roscas-para-tubos.pdf"
            },
            new()
            {
                Type = "Treinamento",
                Name = "Selos mecânicos",
                Thumbnail = "cartilhas-img9.jpg",
                Filename = "selos-mecanicos.pdf"
            },
            new()
            {
                Type = "Treinamento",
                Name = "Torque lubrificado",
                Thumbnail = "cartilhas-img10.jpg",
                Filename = "torque-lubrificado.pdf"
            },
            new()
            {
                Type = "Treinamento",
                Name = "Torque",
                Thumbnail = "cartilhas-img11.jpg",
                Filename = "torque.pdf"
            }
        };
    }
}
