using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Dapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SiteInstitucional.Server.Core;
using SiteInstitucional.Shared.Domain;
using SiteInstitucional.Shared.Dto;

using static System.String;

namespace SiteInstitucional.Server.Controllers
{
    [ApiController, Route("api/domain")]
    public class DomainController : ControllerBase
    {
        private readonly ILogger<DomainController> _logger;
        private readonly IDbConnection _db;

        public DomainController(ILogger<DomainController> logger, IDbConnection db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet("segments")]
        public async Task<Segment[]> GetSegments()
        {
            var result = await _db.QueryAsync<string>(
                "p_GetSegmentos".ToLocalizedProcedure(),
                commandType: CommandType.StoredProcedure);
            return result.Select(s => new Segment
            {
                Name = TreatNull(s)
            }).ToArray();
        }

        [HttpPost("automakers")]
        public async Task<Automaker[]> GetAutomakersByParents([FromBody] ApplicationSearchParams search)
        {
            var result = await _db.QueryAsync<string>(
                "p_GetMarcas".ToLocalizedProcedure(),
                new
                {
                    Segmento = RemoveNullTreatment(search.Segment)
                },
                commandType: CommandType.StoredProcedure);
            return result.Select(s => new Automaker
            {
                Segment = TreatNull(search.Segment),
                Name = TreatNull(s)
            }).ToArray();
        }

        [HttpPost("models")]
        public async Task<Model[]> GetModelsByParents([FromBody] ApplicationSearchParams search)
        {
            var result = await _db.QueryAsync<string>(
                "p_GetModelos".ToLocalizedProcedure(),
                new
                {
                    Segmento = RemoveNullTreatment(search.Segment),
                    Marca = RemoveNullTreatment(search.Automaker)
                },
                commandType: CommandType.StoredProcedure);
            return result.Select(s => new Model
            {
                Segment = TreatNull(search.Segment),
                Automaker = TreatNull(search.Automaker),
                Name = TreatNull(s)
            }).ToArray();
        }

        [HttpPost("engine-types")]
        public async Task<EngineType[]> GetEngineTypesByParents([FromBody] ApplicationSearchParams search)
        {
            var result = await _db.QueryAsync<string>(
                "p_GetMotores".ToLocalizedProcedure(),
                new
                {
                    Segmento = RemoveNullTreatment(search.Segment),
                    Marca = RemoveNullTreatment(search.Automaker),
                    Modelo = RemoveNullTreatment(search.Model)
                },
                commandType: CommandType.StoredProcedure);
            return result.Select(s => new EngineType
            {
                Segment = TreatNull(search.Segment),
                Automaker = TreatNull(search.Automaker),
                Model = TreatNull(search.Model),
                Name = TreatNull(s)
            }).ToArray();
        }

        [HttpPost("product-types")]
        public async Task<ProductType[]> GetProductTypesByParents([FromBody] ApplicationSearchParams search)
        {
            var result = await _db.QueryAsync<string>(
                "p_GetProdutos".ToLocalizedProcedure(),
                new
                {
                    Segmento = RemoveNullTreatment(search.Segment),
                    Marca = RemoveNullTreatment(search.Automaker),
                    Modelo = RemoveNullTreatment(search.Model),
                    Motorizacao = RemoveNullTreatment(search.EngineType)
                },
                commandType: CommandType.StoredProcedure);
            return result.Select(s => new ProductType
            {
                Segment = TreatNull(search.Segment),
                Automaker = TreatNull(search.Automaker),
                Model = TreatNull(search.Model),
                EngineType = TreatNull(search.EngineType),
                Name = TreatNull(s)
            }).ToArray();
        }

        [HttpPost("applications")]
        public async Task<Application[]> GetApplicationsByParents([FromBody] ApplicationSearchParams search)
        {
            var result = await _db.QueryAsync<dynamic>(
                "p_GetAplicacoes".ToLocalizedProcedure(),
                new
                {
                    Segmento = RemoveNullTreatment(search.Segment),
                    Marca = RemoveNullTreatment(search.Automaker),
                    Modelo = RemoveNullTreatment(search.Model),
                    Motorizacao = RemoveNullTreatment(TreatNull(search.EngineType), "-", "-1")
                },
                commandType: CommandType.StoredProcedure);
            return result.Select(s => new Application
            {
                Segment = TreatNull(search.Segment),
                Automaker = TreatNull(search.Automaker),
                Model = TreatNull(search.Model),
                EngineType = TreatNull(s.Motorizacao),
                InitialDate = s.DataInicial,
                EndDate = s.DataFinal
            }).ToArray();
        }

        [HttpGet("application/{code}")]
        public async Task<Application[]> GetApplicationsByCode(string code)
        {
            var result = await _db.QueryAsync<dynamic>(
                "p_GetAplicacoesDetalhesPorCodigo".ToLocalizedProcedure(),
                new { Codigo = code },
                commandType: CommandType.StoredProcedure);
            return result.Select(s => new Application
            {
                Segment = s.Segmento,
                Automaker = s.MarcaReferencia,
                Model = s.Modelo,
                EngineType = s.Motorizacao,
                InitialDate = s.DataInicial,
                EndDate = s.DataFinal,
                Comments = s.obs
            }).ToArray();
        }

        [HttpPost("products")]
        public async Task<Product[]> GetProductsByParents([FromBody] ProductListSearchParams search)
        {
            var result = await _db.QueryAsync<dynamic>(
                "p_GetProdutosPorAplicacao".ToLocalizedProcedure(),
                new
                {
                    Segmento = RemoveNullTreatment(search.Segment),
                    Marca = RemoveNullTreatment(search.Automaker),
                    Modelo = RemoveNullTreatment(search.Model),
                    Motorizacao = RemoveNullTreatment(TreatNull(search.EngineType), "-", "-1"),
                    DataInicial = WebUtility.UrlDecode(search.InitialDate) ?? "-",
                    DataFinal = WebUtility.UrlDecode(search.EndDate) ?? "-"
                },
                commandType: CommandType.StoredProcedure);
            var products = result.Select(s => new Product
            {
                Code = s.Codigo,
                Name = s.NomeProduto
            }).ToArray();

            await FillProducts(products);

            return products;
        }

        [HttpGet("products/code/{code}")]
        public async Task<Product[]> GetProductsByCode(string code)
        {
            var resultSchadekCode = await _db.QueryAsync<dynamic>(
                "p_GetResultadosPorBuscaCodigoSchadek".ToLocalizedProcedure(),
                new
                {
                    Codigo = code
                },
                commandType: CommandType.StoredProcedure);
            var resultSchadekOldCode = await _db.QueryAsync<dynamic>(
                "p_GetResultadosPorBuscaCodigoAntigoSchadek".ToLocalizedProcedure(),
                new
                {
                    Codigo = code?.Replace(".", Empty)
                },
                commandType: CommandType.StoredProcedure);
            var resultOthers = await GetProductsByOthersCode(code);
            var result = resultSchadekCode.Concat(resultSchadekOldCode).Concat(resultOthers);
            var products = result.Select(s => new Product
            {
                Code = s.Codigo,
                Name = s.NomeProduto
            }).ToArray();

            await FillProducts(products);

            return products;
        }

        [HttpGet("product/detail/{code}")]
        public async Task<Product> GetProduct(string code)
        {
            var result = await _db.QuerySingleOrDefaultAsync<dynamic>(
                "p_GetDetalhesPorCodigo".ToLocalizedProcedure(),
                new { Codigo = code },
                commandType: CommandType.StoredProcedure);
            var product = new Product
            {
                Code = result.Codigo,
                Name = result.NomeProduto,
                NameComplement = result.ComplementoNome,
                HasComponents = result.IndicadorKit
            };
            await FillProducts(product);
            return product;
        }

        [HttpGet("product/{code}/automakers-references")]
        public async Task<AutomakerReference[]> GetAutomakersReferencesByCode(string code)
        {
            var result = await _db.QueryAsync<dynamic>(
                "p_GetCodigoRefereciaPorCodigoSchadek",
                new { Codigo = code },
                commandType: CommandType.StoredProcedure);
            return result.Select(s => new AutomakerReference
            {
                Automaker = s.MarcaReferencia,
                ReferenceCode = s.CodReferencia
            }).ToArray();
        }

        [HttpGet("product/{code}/components")]
        public async Task<string[]> GetComponentsByCode(string code)
        {
            var result = await _db.QueryAsync<string>(
                "p_GetComponentesPorCodigoSchadek".ToLocalizedProcedure(),
                new { Codigo = code },
                commandType: CommandType.StoredProcedure);
            return result
                .SelectMany(c => c.Split('/', StringSplitOptions.RemoveEmptyEntries))
                .Select(c => c.Trim())
                .OrderBy(c => c)
                .ToArray();
        }

        private async Task FillProducts(params Product[] products)
        {
            foreach (var product in products)
            {
                product.OldCode = await _db.QueryFirstOrDefaultAsync<string>(
                    "p_GetCodigoAntigoPorCodigo",
                    new { CodNovo = product.Code },
                    commandType: CommandType.StoredProcedure);
            }

            foreach (var product in products)
            {
                product.Package = await _db.QueryFirstOrDefaultAsync<string>(
                    "p_GetEmbalagemPorCodigo",
                    new { Codigo = product.Code },
                    commandType: CommandType.StoredProcedure);
                product.Package = product.Package == "CX"
                    ? "Caixa"
                    : product.Package == "SP"
                        ? "Saco plástico"
                        : product.Package;
            }

            foreach (var product in products)
            {
                var packageContents = await _db.QueryAsync<string>(
                    "p_GetConteudoEmbalagemPorCodigo",
                    new { Codigo = product.Code },
                    commandType: CommandType.StoredProcedure);
                product.PackageContent = Join(", ", packageContents);
            }

            foreach (var product in products)
            {
                var codes = await _db.QueryFirstOrDefaultAsync<dynamic>(
                    "p_GetCodBarraeFiscalPorCodigo",
                    new { Codigo = product.Code },
                    commandType: CommandType.StoredProcedure);
                product.BarCode = codes?.EAN13;
                product.TaxCode = codes?.Fiscal;
            }
        }

        private async Task<IEnumerable<dynamic>> GetProductsByOthersCode(string code)
        {
            var resultOthers = await _db.QueryAsync<dynamic>(
                "p_GetCodigoSchadekPorOutrosCodigos",
                new
                {
                    OutrosCodigo = code?.Replace(".", Empty)
                },
                commandType: CommandType.StoredProcedure);
            var result = Enumerable.Empty<dynamic>();
            foreach (var other in resultOthers)
            {
                var resultSchadekCode = await _db.QueryAsync<dynamic>(
                    "p_GetResultadosPorBuscaCodigoSchadek".ToLocalizedProcedure(),
                    new
                    {
                        other.Codigo
                    },
                    commandType: CommandType.StoredProcedure);
                result = result.Concat(resultSchadekCode);
            }
            return result;
        }

        private string TreatNull(string value, string replacement = "-") =>
            IsNullOrEmpty(value) ? replacement : value;

        private string RemoveNullTreatment(string value, string treatment = "-", string replacement = null) =>
            value == treatment ? replacement : value;
    }
}
