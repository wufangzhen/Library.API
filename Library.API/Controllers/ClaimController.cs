using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.API.Entities;
using Library.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Library.API.Controllers
{
    [Route("api/claims")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        public ClaimController(IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            ILogger<AuthorController> logger,
            IDistributedCache distributedCache,
            IHashFactory hashFactory,
            UserManager<User> userManager)
        {
            RepositoryWrapper = repositoryWrapper;
            Mapper = mapper;
            Logger = logger;
            DistributedCache = distributedCache;
            HashFactory = hashFactory;
            UserManager = userManager;
        }

        public IDistributedCache DistributedCache { get; }
        public IHashFactory HashFactory { get; }
        public ILogger<AuthorController> Logger { get; }
        public IMapper Mapper { get; }
        public IRepositoryWrapper RepositoryWrapper { get; }
        public UserManager<User> UserManager { get; }

        [HttpGet(Name = (nameof(GetUserClaimAsync)))]
        public async Task<IActionResult> GetUserClaimAsync(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null )
            { return NotFound(); }
            var claims = await UserManager.GetClaimsAsync(user);
            return Ok(claims);
        }

        [HttpPost(Name = nameof(AddUserClaimAsync))]
        public async Task<IActionResult> AddUserClaimAsync(string userId, string claimType, string claimValue)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null || string.IsNullOrWhiteSpace(claimType))
            { return NotFound(); }
            var claims =await UserManager.GetClaimsAsync(user);
            foreach(var i in claims)
            {
                if(i.Type==claimType&&i.Value==claimValue)
                {
                    return StatusCode(StatusCodes.Status304NotModified); 
                }
            }
            var result = await UserManager.AddClaimAsync(user, new System.Security.Claims.Claim(claimType, claimValue));
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                ModelState.AddModelError("Error", result.Errors.FirstOrDefault()?.Description);
                return BadRequest(ModelState);
            }
        }
    }
}