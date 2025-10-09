using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiProject.Api.CustomActionFilters;
using WebApiProject.Api.Models.Domain;
using WebApiProject.Api.Models.DTOs;
using WebApiProject.Api.Reponsitories;

namespace WebApiProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;
        private readonly IMapper mapper;

        public ImagesController(IImageRepository imageRepository, IMapper mapper)
        {
            this.imageRepository = imageRepository;
            this.mapper = mapper;
        }

        #region POST: https://localhost:{port}/api/images/upload
        [HttpPost]
        [Route("upload")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            // Validate file upload
            ValidateFileUpload(request);

            // Map DTO to Domain model
            var imageDomain = new Image
            {
                File = request.File,
                FileExtension = Path.GetExtension(request.File.FileName),
                FileSizeInBytes = request.File.Length,
                FileName = request.FileName,
                FileDescription = request.FileDescription,
            };

            // Save image
            await imageRepository.Upload(imageDomain);

            // Map Domain model to DTO for response
            var imageDto = mapper.Map<ImageDto>(imageDomain);

            return Ok(imageDto);
        }
        #endregion

        #region Validation Methods
        // Validate file upload
        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("File", "Unsupported file type. Only .jpg, .jpeg, and .png are allowed.");
            }

            if (request.File.Length > 5 * 1024 * 1024) // 5 MB limit
            {
                ModelState.AddModelError("File", "File size exceeds the 5 MB limit.");
            }
        }
        #endregion
    }
}
