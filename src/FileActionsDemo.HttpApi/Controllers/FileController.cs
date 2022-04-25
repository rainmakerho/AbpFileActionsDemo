using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace FileActionsDemo.Controllers
{
    
    public class FileController : AbpController
    {
        private readonly IFileAppService _fileAppService;

        public FileController(IFileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        [HttpGet]
        [Route("download/{fileName}")]
        public async Task<IActionResult> DownloadAsync(string fileName)
        {
            var fileDto = await _fileAppService.GetBlobAsync(new GetBlobRequestDto { Name = fileName });
            return File(fileDto.Content, "application/octet-stream", fileDto.Name);
        }

        [HttpPost]
        [Route("upload")]
        [RequestSizeLimit(2011)]
        public async Task<IActionResult> UploadAsync([FromForm] IFormFile formFile)
        {

            var size = formFile.Length;
                if (size > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(memoryStream);

                        var saveBlobInputDto = new SaveBlobInputDto
                        {
                            Name = formFile.Name,
                            Content = memoryStream.ToArray()
                        };
                        await _fileAppService.SaveBlobAsync(saveBlobInputDto);

                    }
                }
 
            


            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = 1, size });
        }
    }
}
