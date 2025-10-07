using AutoMapper;
using WebApiProject.Api.Models.Domain;
using WebApiProject.Api.Models.DTOs;

namespace WebApiProject.Api.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Region mappings
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();

            // Walk mappings
            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<AddWalkRequestDto, Walk>().ReverseMap();
            CreateMap<UpdateWalkRequestDto, Walk>().ReverseMap();

            // Difficulty mappings
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
        }
    }
}
