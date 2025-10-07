using System.ComponentModel.DataAnnotations;

namespace WebApiProject.Api.Models.DTOs
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "Code has to be a minimum of 3 characters.")]
        [MaxLength(2, ErrorMessage = "Code has to be a maximum of 3 characters.")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be a maximum of 100 characters.")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
