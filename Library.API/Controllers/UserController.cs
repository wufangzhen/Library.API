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
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController(UserManager<User> userManager,
            IMapper mapper,
            ILogger<AuthorController> logger,
            IDistributedCache distributedCache,
            IHashFactory hashFactory)
        {
            UserManager = userManager;
            Mapper = mapper;
            Logger = logger;
            DistributedCache = distributedCache;
            HashFactory = hashFactory;
        }

        public IDistributedCache DistributedCache { get; }
        public IHashFactory HashFactory { get; }
        public ILogger<AuthorController> Logger { get; }
        public IMapper Mapper { get; }
        public UserManager<User> UserManager { get; }

        // GET: api/<UserController1>
        [HttpGet("{userId}",Name =nameof(GetUserAsync))]
        public async Task<ActionResult<User>> GetUserAsync(string userId)
        {
            var user =await UserManager.FindByIdAsync(userId);
            return user;
        }

        [HttpGet(Name = nameof(GetUsersAsync))]
        [ResponseCache(Duration = 60)]
        public IActionResult GetUsersAsync()
        {
            var user = UserManager.Users.ToList();
            return Ok(user);
        }

        [HttpPost(Name =nameof(CreateUserAsync))]
        public async Task<ActionResult> CreateUserAsync(UserForUpdateDto updateuser)
        {
            var user = new User();
            Mapper.Map(updateuser, user, typeof(UserForUpdateDto), typeof(User));
            IdentityResult result;
             result = await UserManager.CreateAsync(user);
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

        [HttpDelete("{userid}", Name =nameof(DeleteUserAsync))]
        public async Task<ActionResult> DeleteUserAsync(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            var result = await UserManager.DeleteAsync(user);
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

        [HttpPut("{userid}",Name = nameof(UpdateUserAsync))]
        public async Task<IActionResult> UpdateUserAsync(string userid,UserForUpdateDto updateuser)
        {
            var user =await UserManager.FindByIdAsync(userid);
            Mapper.Map(updateuser, user, typeof(UserForUpdateDto), typeof(User));
            var result =await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(user);
            }
            else
            {
                ModelState.AddModelError("Error", result.Errors.FirstOrDefault()?.Description);
                return BadRequest(ModelState);
            }
        }
    }
}
