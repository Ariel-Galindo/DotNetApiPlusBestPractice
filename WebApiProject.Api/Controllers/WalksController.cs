using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebApiProject.Api.CustomActionFilters;
using WebApiProject.Api.Models.Domain;
using WebApiProject.Api.Models.DTOs;
using WebApiProject.Api.Reponsitories;

namespace WebApiProject.Api.Controllers
{
    // https://localhost:{port}/api/walks
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalksController : ControllerBase
    {
        private const string WalksCacheKey = "WalksList";

        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<WalksController> _logger;
        private static readonly SemaphoreSlim _walksSemaphore = new SemaphoreSlim(1, 1);

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IMemoryCache cache,
            ILogger<WalksController> logger)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            _cache = cache;
            _logger = logger;
        }

        #region GET: https://localhost:{port}/api/walks?filterOn=&filterQuery=&sortBy=&isAscending=&pageNumber=&pageSize=
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {

            if (_cache.TryGetValue(WalksCacheKey, out List<WalkDto>? walksDto))
            {
                _logger.LogInformation("Fetching data from cache.");
            } 
            else
            {
                try
                {
                    await _walksSemaphore.WaitAsync(); // awaiting for task to complete

                    // Caching
                    if (_cache.TryGetValue(WalksCacheKey, out walksDto))
                    {
                        _logger.LogInformation("Fetching data from cache.");
                    }
                    else
                    {
                        _logger.LogInformation("Fetching data from database.");

                        // Fetch data from database
                        var walksDomain = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

                        // Convert domain to DTO
                        walksDto = mapper.Map<List<WalkDto>>(walksDomain);

                        if (walksDto == null)
                        {
                            return NotFound();
                        }

                        // Set cache options
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                            .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                            .SetPriority(CacheItemPriority.Normal)
                            .SetSize(1);

                        _cache.Set(WalksCacheKey, walksDto, cacheEntryOptions);
                    }
                }
                finally
                {
                    _walksSemaphore.Release(); // releasing the semaphore
                }
            }
            return Ok(walksDto);
        }
        #endregion

        #region GET: https://localhost:{port}/api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {

            string cachekey = $"Walk_{id}";

            if (_cache.TryGetValue(cachekey, out WalkDto? walkDto))
            {
                _logger.LogInformation("Fetching data from cache.");
            } 
            else
            {
                try
                {
                    await _walksSemaphore.WaitAsync(); // awaiting for task to complete

                    // Caching
                    if (_cache.TryGetValue(cachekey, out walkDto))
                    {
                        _logger.LogInformation("Fetching data from cache.");
                    }
                    else
                    {
                        _logger.LogInformation("Fetching data from database.");

                        // Fetch data from database
                        var walkDomain = await walkRepository.GetByIdAsync(id);

                        // Convert domain to DTO
                        walkDto = mapper.Map<WalkDto>(walkDomain);

                        if (walkDomain == null)
                        {
                            return NotFound();
                        }

                        // Set cache options
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                            .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                            .SetPriority(CacheItemPriority.Normal)
                            .SetSize(1);

                        _cache.Set(cachekey, walkDto, cacheEntryOptions);
                    }
                }
                finally
                {
                    _walksSemaphore.Release(); // releasing the semaphore
                }
            }
            return Ok(walkDto);
        }
        #endregion

        #region POST: https://localhost:{port}/api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            // Convert DTO to Domain
            var walkDomain = mapper.Map<Walk>(addWalkRequestDto);

            // Create new walk
            var walkCreated = await walkRepository.CreateAsync(walkDomain);

            // Convert Domain to DTO
            var walkDto = mapper.Map<WalkDto>(walkCreated);

            // Invalidate the cache
            _cache.Remove(WalksCacheKey);
            _logger.LogInformation("New walk created and cache removed.");

            return CreatedAtAction(nameof(GetById), new { id = walkDto.Id }, walkDto);
        }
        #endregion

        #region PUT: https://localhost:{port}/api/walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            // Convert DTO to Domain
            var walkDomain = mapper.Map<Walk>(updateWalkRequestDto);

            // Update walk
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            if (walkDomain == null)
            {
                return NotFound();
            }

            // Convert Domain to DTO
            var updatedWalk = mapper.Map<WalkDto>(walkDomain);

            // Invalidate the cache
            _cache.Remove($"walk_{id}");
            _logger.LogInformation($"Walk with id {id} updated and cache removed.");

            return Ok(updatedWalk);
        }
        #endregion

        #region DELETE: https://localhost:{port}/api/walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // Delete walk
            var walkDomain = await walkRepository.DeleteAsync(id);

            if (walkDomain == null)
            {
                return NotFound();
            }

            // Convert Domain to DTO
            var walkDeleted = mapper.Map<WalkDto>(walkDomain);

            // Invalidate the cache
            _cache.Remove($"walk_{id}");
            _logger.LogInformation($"Walk with id {id} deleted and cache removed.");

            return Ok(walkDeleted);
        }
        #endregion
    }
}
