using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BarcodeLib;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nano35.Files.Api.Services;

namespace Nano35.Files.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImagesController :
        ControllerBase
    {
        private readonly ILogger<ImagesController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationContext _context;
        public ImagesController(
            ILogger<ImagesController> logger, 
            IWebHostEnvironment hostingEnvironment,
            ApplicationContext context)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }
        
        [HttpGet]
        [Route("GetImagesOfStorageItem")]
        public async Task<ActionResult<IList<string>>> GetImagesOfStorageItem(
            [FromQuery] Guid storageItem)
        {
            return Ok((await _context.ImagesOfStorageItems
                    .Where(f => f.StorageItemId == storageItem)
                    .ToListAsync())
                    .Select(a =>
                        File(System.IO.File.ReadAllBytes(
                            Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "StorageItems", a.NormalizedName)),
                            "application/force-download",
                            a.RealName))
                    .Select(a => Convert.ToBase64String(a.FileContents)));
        }
        
        [HttpPost]
        [Route("CreateStorageItemImage")]
        public async Task<ActionResult<IList<UploadResult>>> GetAllArticles(
            [FromForm] IEnumerable<IFormFile> files, 
            [FromQuery] Guid storageITemId)
        {    
            var maxAllowedFiles = 3;
            long maxFileSize = 1024 * 1024 * 15;
            var filesProcessed = 0;
            var resourcePath = new Uri($"{Request.Scheme}://{Request.Host}/");
            IList<UploadResult> uploadResults = new List<UploadResult>();

            foreach (var file in files)
            {
                var uploadResult = new UploadResult();
                var untrustedFileName = file.FileName;
                uploadResult.FileName = untrustedFileName;
                var trustedFileNameForDisplay = WebUtility.HtmlEncode(untrustedFileName);
                var trustedFileNameForFileStorage = Path.GetRandomFileName().Split('.').First();
                var storedName = $"{trustedFileNameForFileStorage}.{trustedFileNameForDisplay.Split('.').Last()}";

                if (filesProcessed < maxAllowedFiles)
                {
                    if (file.Length == 0)
                    {
                        uploadResult.ErrorCode = 1;
                    }
                    else if (file.Length > maxFileSize)
                    {
                        uploadResult.ErrorCode = 2;
                    }
                    else
                    {
                        try
                        {
                            var path = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "StorageItems", storedName);
                            await using MemoryStream ms = new();
                            await file.CopyToAsync(ms);
                            await System.IO.File.WriteAllBytesAsync(path, ms.ToArray());
                            uploadResult.Uploaded = true;
                            uploadResult.StoredFileName = storedName;
                            await _context.ImagesOfStorageItems.AddAsync(
                                new ImagesOfStorageItem()
                                {
                                    Id = Guid.NewGuid(),
                                    IsConfirmed = false,
                                    Uploaded = DateTime.Now,
                                    NormalizedName = storedName,
                                    RealName = trustedFileNameForDisplay,
                                    StorageItemId = storageITemId
                                });
                            await _context.SaveChangesAsync();
                        }
                        catch (IOException ex)
                        {
                            uploadResult.ErrorCode = 3;
                        }
                    }
                    filesProcessed++;
                }
                else
                {
                    uploadResult.ErrorCode = 4;
                }
                uploadResults.Add(uploadResult);
            }

            return new CreatedResult(resourcePath, uploadResults);
        }
    }

    public class UploadResult
    {
        public int ErrorCode { get; set; }
        public string StoredFileName { get; set; }
        public string FileName { get; set; }
        public bool Uploaded { get; set; }
    }
}