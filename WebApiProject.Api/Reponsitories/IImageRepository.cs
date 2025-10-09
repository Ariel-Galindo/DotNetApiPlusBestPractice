using WebApiProject.Api.Models.Domain;

namespace WebApiProject.Api.Reponsitories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
