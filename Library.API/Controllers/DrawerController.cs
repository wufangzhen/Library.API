using AutoMapper;
using Library.API.Entities;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrawerController : ControllerBase
    {
        public DrawerController(IRepositoryWrapper repositoryWrapper,
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

        [HttpGet(Name = (nameof(GetUserMenuAsync)))]
        public async Task<IActionResult> GetUserMenuAsync(string userId)
        {

            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
                var claims = (await UserManager.GetClaimsAsync(user)).Where(p=>p.Type=="menu").ToList();
            var result =new List<ClaimInfo>();
            foreach(var i in claims)
            {
                result.Add(new ClaimInfo() { href ="/"+ i.Value ,to=i.Value});
            }
            return Ok(result);
        }

    }
}
