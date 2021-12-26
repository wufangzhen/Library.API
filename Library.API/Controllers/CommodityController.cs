using AutoMapper;
using Library.API.Entities;
using Library.API.Helpers;
using Library.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommodityController : ControllerBase
    {
        public CommodityController(
           IMapper mapper,
           ILogger<AuthorController> logger,
           IDistributedCache distributedCache,
           IHashFactory hashFactory,
           IRepositoryBase<Commodity, Guid> commodityRepository,
           IRepositoryBase<CommodityTag, Guid> commodityTagRepository,
           ICommodityService commodityService)
        {
            Mapper = mapper;
            Logger = logger;
            DistributedCache = distributedCache;
            HashFactory = hashFactory;
            CommodityRepository = commodityRepository;
            CommodityService = commodityService;
            CommodityTagRepository = commodityTagRepository;
        }
        IRepositoryBase<Commodity,Guid> CommodityRepository { get; }
        public IDistributedCache DistributedCache { get; }
        public IHashFactory HashFactory { get; }
        public ILogger<AuthorController> Logger { get; }
        public IMapper Mapper { get; }
        public IRepositoryBase<CommodityTag,Guid> CommodityTagRepository {  get; }
        public ICommodityService CommodityService { get; }
        [HttpGet]
        public async Task<IActionResult> GetCommoditiesAsync([FromQuery] ResourceParameters parameters)
        {
            PagedList<Commodity> pagedList;   
            string cacheKey = JsonSerializer.Serialize(parameters);
            string cachedContent=await DistributedCache.GetStringAsync(cacheKey);
            if(string.IsNullOrEmpty(cachedContent))
            {
                pagedList=await CommodityService.GetAllAsync(parameters);
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };
                var serializedContent = JsonSerializer.Serialize(pagedList);
                await DistributedCache.SetStringAsync(cacheKey, serializedContent);
            }
            else
            {
                pagedList = JsonSerializer.Deserialize<PagedList<Commodity>>(cachedContent);
            }
            
            //var p =(await CommodityService.GetAllAsync(parameters)).ToList();

            return Ok(pagedList);
        }
        [HttpGet("Tag")]
        public async Task<IActionResult> GetCommodityTag()
        {
            var result = (await CommodityTagRepository.GetAllAsync()).ToList();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderAsync()
        {
            return Ok();
        }
    }
}
