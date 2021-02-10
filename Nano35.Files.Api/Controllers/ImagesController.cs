using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationContext _context;
        public ImagesController(IWebHostEnvironment hostingEnvironment, ApplicationContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }
        
        [HttpPost]
        [Route("CreateStorageItemImage")]
        [RequestSizeLimit(45000000)]
        public async Task<IActionResult> GetAllArticles()
        {    
            // https://www.thecodehubs.com/file-upload-in-angular-7-with-asp-net-core-web-api/
            try
            {
                var headers = Request.Headers["id"].ToString();
                var newPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Static\\StorageItems");
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                var old = Directory
                    .EnumerateFiles(newPath, "*", SearchOption.AllDirectories)
                    .Where(c => c.Contains(headers));
                
                foreach (var item in old)
                {
                    System.IO.File.Delete(item);
                }

                for (int i = 0; i < Request.Form.Files.Count; i++)
                {
                    if (Request.Form.Files[i].Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(Request.Form.Files[i].ContentDisposition).FileName
                            .Trim('"')
                            .Split(".")
                            .Last();
                        
                        var fullPath = Path.Combine(newPath, $"{i}_{headers}.{fileName}");
                        
                        await using var stream = new FileStream(fullPath, FileMode.Create);
                        
                        await Request.Form.Files[i].CopyToAsync(stream);
                    }
                }

                await _context.ImagesOfStorageItems
                    .AddAsync(new ImagesOfStorageItem()
                    {
                        Uploaded = DateTime.Now, 
                        IsConfirmed = false,
                        StorageItemId = Guid.Parse(headers)
                    });
                await _context.SaveChangesAsync();
                
                return Ok("Upload Successful.");
            }
            catch (System.Exception ex)
            {
                return BadRequest("Upload Failed: " + ex.Message);
            }
        }
    }
}