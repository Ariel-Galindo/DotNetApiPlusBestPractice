using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiProject.Api.Models.DTOs
{
    public class ImageUploadRequestDto
    {
        [Required]
        [DataType(DataType.Upload)]
        public IFormFile File { get; set; }

        [Required]
        public string FileName { get; set; }
        public string? FileDescription { get; set; }
    }
}
