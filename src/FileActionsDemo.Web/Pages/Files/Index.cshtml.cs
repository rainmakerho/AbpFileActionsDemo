using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace FileActionsDemo.Web.Pages.Files
{
    public class Index : AbpPageModel
    {
        private readonly IFileAppService _fileAppService;

        [BindProperty]
        public UploadFileDto UploadFileDto { get; set; }

        public Index(IFileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        public void OnGet()
        {

        }


        [RequestSizeLimit(2000)]
        public async Task<IActionResult> OnPostAsync()
        {
            using(var memoryStream = new MemoryStream())
            {
                await UploadFileDto.File.CopyToAsync(memoryStream);

                var saveBlobInputDto = new SaveBlobInputDto
                {
                    Name = UploadFileDto.Name,
                    Content = memoryStream.ToArray()
                };
                await _fileAppService.SaveBlobAsync(saveBlobInputDto);

            }

            return Page();
        }
    }

    public class UploadFileDto
    {
        [Required]
        [Display(Name="File")]
        public IFormFile File { get; set; }


        [Required]
        [Display(Name = "Filename")]
        public string Name { get; set; }
    }
}
