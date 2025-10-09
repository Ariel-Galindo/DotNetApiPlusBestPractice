using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiProject.Api.Models.DTOs
{
    public class ImageDto
    {
        public Guid Id { get; set; }
        public IFormFile File { get; set; }
        public string FileName { get; set; }
        public string? FileDescription { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeInBytes { get; set; }
        public string NamePath { get; set; }
    }
}
