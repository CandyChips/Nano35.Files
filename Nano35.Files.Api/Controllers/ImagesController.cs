using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nano35.Contracts.Storage.Artifacts;
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
        public async Task<ActionResult<IList<FileContentResult>>> GetImagesOfStorageItem(
            [FromQuery] Guid storageItem)
        {
            return Ok((await _context.ImagesOfStorageItems
                    .Where(f => f.StorageItemId == storageItem)
                    .ToListAsync())
                    .Select(a =>
                    {
                        var fileBytes = System.IO.File.ReadAllBytes(
                            Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "StorageItems", a.NormalizedName));
                        return File(fileBytes, "application/force-download", a.RealName);
                    }));
        }
        
        [HttpPost]
        [Route("CreateStorageItemImage")]
        public async Task<ActionResult<IList<UploadResult>>> GetAllArticles(
            [FromForm] IEnumerable<IFormFile> files, [FromQuery] Guid storageITemId)
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
                var trustedFileNameForDisplay = 
                    WebUtility.HtmlEncode(untrustedFileName);

                if (filesProcessed < maxAllowedFiles)
                {
                    if (file.Length == 0)
                    {
                        _logger.LogInformation("{FileName} length is 0", 
                            trustedFileNameForDisplay);
                        uploadResult.ErrorCode = 1;
                    }
                    else if (file.Length > maxFileSize)
                    {
                        _logger.LogInformation("{FileName} of {Length} bytes is " +
                            "larger than the limit of {Limit} bytes", 
                            trustedFileNameForDisplay, file.Length, maxFileSize);
                        uploadResult.ErrorCode = 2;
                    }
                    else
                    {
                        try
                        {
                            var trustedFileNameForFileStorage = Path.GetRandomFileName();
                            var path = Path.Combine(_hostingEnvironment.ContentRootPath, 
                                "wwwroot", "StorageItems", 
                                trustedFileNameForFileStorage);
                            await using MemoryStream ms = new();
                            await file.CopyToAsync(ms);
                            await System.IO.File.WriteAllBytesAsync(path, ms.ToArray());
                            _logger.LogInformation("{FileName} saved at {Path}", 
                                trustedFileNameForDisplay, path);
                            uploadResult.Uploaded = true;
                            uploadResult.StoredFileName = trustedFileNameForFileStorage;
                            await _context.ImagesOfStorageItems.AddAsync(
                                new ImagesOfStorageItem()
                                {
                                    Id = Guid.NewGuid(),
                                    IsConfirmed = false,
                                    Uploaded = DateTime.Now,
                                    NormalizedName = trustedFileNameForFileStorage,
                                    RealName = trustedFileNameForDisplay,
                                    StorageItemId = storageITemId
                                });
                            await _context.SaveChangesAsync();
                        }
                        catch (IOException ex)
                        {
                            _logger.LogError("{FileName} error on upload: {Message}", 
                                trustedFileNameForDisplay, ex.Message);
                            uploadResult.ErrorCode = 3;
                        }
                    }

                    filesProcessed++;
                }
                else
                {
                    _logger.LogInformation("{FileName} not uploaded because the " +
                        "request exceeded the allowed {Count} of files", 
                        trustedFileNameForDisplay, maxAllowedFiles);
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