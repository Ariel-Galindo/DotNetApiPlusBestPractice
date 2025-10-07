using Microsoft.EntityFrameworkCore;
using WebApiProject.Api.Data;
using WebApiProject.Api.Models.Domain;
using WebApiProject.Api.Models.DTOs;

namespace WebApiProject.Api.Reponsitories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly WebApiDbContext context;

        public SQLRegionRepository(WebApiDbContext context)
        {
            this.context = context;
        }

        public async Task<Region?> CreateAsync(Region region)
        {
            if (region == null)
            {
                return null;
            }

            await context.Regions.AddAsync(region);
            await context.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegion = await context.Regions.FindAsync(id);

            if (existingRegion == null)
            {
                return null;
            }

            context.Regions.Remove(existingRegion);
            await context.SaveChangesAsync();
            return existingRegion;
        }

        public async Task<List<Region>?> GetAllAsync()
        {
            return await context.Regions.ToListAsync() ?? null;
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await context.Regions.FindAsync(id) ?? null;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await context.Regions.FindAsync(id);

            if (existingRegion == null)
            {
                return null;
            }

            existingRegion.Name = region.Name;
            existingRegion.Code = region.Code;
            existingRegion.RegionImageUrl = region.RegionImageUrl;
            await context.SaveChangesAsync();

            return existingRegion;
        }
    }
}
