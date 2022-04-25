
using Microsoft.AspNetCore.Mvc;


namespace FileUpload.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {

        [HttpPost("Upload")]
        //[RequestSizeLimit(2222)]
        public async Task<IActionResult> UploadAsync(IFormFile formFile)
        {

            //var filter = _sp.GetRequiredService<RequestSizeLimitFilter>();
            var size = formFile.Length;
            if (size > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await formFile.CopyToAsync(memoryStream);

                }
            }
            return Ok(new { count = 1, size });
        }

        [HttpPost("Upload2")]
        [RequestSizeLimit(3333)]
        public async Task<IActionResult> Upload2Async(IFormFile formFile)
        {

            //var filter = _sp.GetRequiredService<RequestSizeLimitFilter>();
            var size = formFile.Length;
            if (size > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await formFile.CopyToAsync(memoryStream);

                }
            }
            return Ok(new { count = 1, size });
        }
    }
}
