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

namespace Nano35.Files.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImagesController :
        ControllerBase
    {
        private IWebHostEnvironment _hostingEnvironment;
        public ImagesController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public class Test
        {
            public IFormFileCollection Files { get; set; }
            public Guid Id { get; set; }
        }
        
        [HttpPost]
        [Route("CreateStorageItemImage")]
        public async Task<IActionResult> GetAllArticles()
        {    
            try
            {
                var headers = Request.Headers["id"].ToString();
                var folderName = "Static\\StorageItems";
                var webRootPath = _hostingEnvironment.ContentRootPath;
                var newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                var old = Directory.EnumerateFiles(newPath, "*", SearchOption.AllDirectories)
                    .Where(c => c.Contains(headers));
                foreach (var item in old)
                {
                    System.IO.File.Delete(item);
                }

                for (int i = 0; i < Request.Form.Files.Count; i++)
                {
                    if (Request.Form.Files[i].Length > 0)
                    {
                        string fileName = ContentDispositionHeaderValue.Parse(Request.Form.Files[i].ContentDisposition).FileName.Trim('"').Split(".").Last();
                        var name = $"{i}_{headers}.{fileName}";
                        string fullPath = Path.Combine(newPath, name);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            Request.Form.Files[i].CopyTo(stream);
                        }
                    }
                }
                return Ok("Upload Successful.");
            }
            catch (System.Exception ex)
            {
                return Ok("Upload Failed: " + ex.Message);
            }
        }
    }
}