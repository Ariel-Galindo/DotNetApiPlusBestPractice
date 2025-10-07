using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Extensions.Caching.Memory;
using WebApiProject.Api.CustomActionFilters;
using WebApiProject.Api.Models.Domain;
using WebApiProject.Api.Models.DTOs;
using WebApiProject.Api.Reponsitories;

namespace WebApiProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private const string WalksCacheKey = "WalksList";

        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<WalksController> _logger;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IMemoryCache cache,
            ILogger<WalksController> logger)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {

            if (_cache.TryGetValue(WalksCacheKey, out List<WalkDto>? walksDto))
            {
                _logger.LogInformation("Fetching walks from cache.");
            } 
            else
            {

                _logger.LogInformation("Fetching walks from database.");

                var walksDomain = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

                walksDto = mapper.Map<List<WalkDto>>(walksDomain);

                if (walksDto == null)
                {
                    return NotFound();
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                    .SetPriority(CacheItemPriority.Normal);

                _cache.Set(WalksCacheKey, walksDto, cacheEntryOptions);
            }

                return Ok(walksDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomain = await walkRepository.GetByIdAsync(id);

            if (walkDomain == null)
            {
                return NotFound();
            }

            var walkDto = mapper.Map<WalkDto>(walkDomain);

            return Ok(walkDto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            var walkDomain = mapper.Map<Walk>(addWalkRequestDto);

            var walkCreated = await walkRepository.CreateAsync(walkDomain);

            var walkDto = mapper.Map<WalkDto>(walkCreated);

            return CreatedAtAction(nameof(GetById), new { id = walkDto.Id }, walkDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            var walkDomain = mapper.Map<Walk>(updateWalkRequestDto);

            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            if (walkDomain == null)
            {
                return NotFound();
            }

            var updatedWalk = mapper.Map<WalkDto>(walkDomain);

            return Ok(updatedWalk);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomain = await walkRepository.DeleteAsync(id);

            if (walkDomain == null)
            {
                return NotFound();
            }

            var walkDeleted = mapper.Map<WalkDto>(walkDomain);

            return Ok(walkDeleted);
        }
    }
}
