using System;
using System.IO;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SiteInstitucional.Server.Controllers
{
    [ApiController, Route("api/static")]
    public class StaticController : ControllerBase
    {
        private readonly ILogger<StaticController> _logger;
        private readonly SchadekConfig _config;

        private const string ImageMimeType = "image/jpeg";
        private const string PdfMimeType = "application/x-pdf";
        private const string ZipMimeType = "application/zip";

        public StaticController(ILogger<StaticController> logger, SchadekConfig config)
        {
            _logger = logger;
            _config = config;
        }


        [HttpGet("products-images/{code}/thumbnail")]
        public IActionResult GetProductImageThumnail(string code) =>
            GetImage(_config.Products.Thumbnails, GetImageName(code));

        [HttpGet("products-images/{code}")]
        public IActionResult GetProductImage(string code) =>
            GetImage(_config.Products.FullImages, GetImageName(code));

        private string GetImageName(string code) => $"{code}.jpg";

        /*
         * Operação utilizada por relatórios do SAP para obter imagens
         */
        [HttpGet("/sap/{**imagePath}")]
        public IActionResult GetSapImage(string imagePath) =>
            GetImage(_config.SAP, imagePath);

        /*
         * Operação utilizada por um sistema que está com o caminho
         * da imagem hardcoded
         */
        //[HttpGet("/css/img/{imageName}")]
        //public IActionResult GetCssImg(string imageName) =>
        //    GetImage(_host.WebRootPath, "img/old-css-img", imageName);

        private IActionResult GetImage(string parentDir, string imagePath)
            => GetImage(_config.StaticContentDirectory, parentDir, imagePath);

        private IActionResult GetImage(string rootDir, string parentDir, string imagePath)
        {
            var path = Path.Combine(rootDir, parentDir, imagePath);
            if (System.IO.File.Exists(path))
            {
                return PhysicalFile(path, ImageMimeType);
            }

            _logger.LogInformation($"Image '{path}' doesn´t exists.");
            path = Path.Combine(_config.StaticContentDirectory, _config.Products.NotFound);
            return PhysicalFile(path, ImageMimeType);
        }


        [HttpGet("downloads/file/{filename}")]
        public IActionResult GetDownloadFile(string filename) =>
            GetFile(_config.Downloads, filename);

        private string GetFileMimeType(string filename) =>
            Path.GetExtension(filename).EndsWith("pdf", StringComparison.InvariantCultureIgnoreCase)
                ? PdfMimeType
                : ZipMimeType;

        private IActionResult GetFile(string parentDir, string filename)
        {
            var mimeType = GetFileMimeType(filename);
            var path = Path.Combine(_config.StaticContentDirectory, parentDir, filename);
            if (System.IO.File.Exists(path))
            {
                return PhysicalFile(path, mimeType);
            }
            _logger.LogInformation($"File '{path}' doesn´t exists.");
            return NotFound($"File '{filename}' doesn´t exists.");
        }
    }
}
