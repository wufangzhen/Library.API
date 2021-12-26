using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.API.Entities;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Library.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    //[Authorize]
    public class RoleController : ControllerBase
    {
        public RoleController(IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            ILogger<AuthorController> logger,
            IDistributedCache distributedCache,
            IHashFactory hashFactory,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            RepositoryWrapper = repositoryWrapper;
            Mapper = mapper;
            Logger = logger;
            DistributedCache = distributedCache;
            HashFactory = hashFactory;
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public IDistributedCache DistributedCache { get; }
        public IHashFactory HashFactory { get; }
        public ILogger<AuthorController> Logger { get; }
        public IMapper Mapper { get; }
        public IRepositoryWrapper RepositoryWrapper { get; }
        public RoleManager<Role> RoleManager { get; }
        public UserManager<User> UserManager { get; }


        [HttpGet(Name = nameof(GetRoleAsync))]
        public async Task<ActionResult<List<Role>>> GetRoleAsync()
        {
            var result = RoleManager.Roles.ToList();

            return result;
        }

        [HttpPost(Name = nameof(AddUserToRoleAsync))]
        public async Task<IActionResult> AddUserToRoleAsync(string userId, string roleName)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null || string.IsNullOrWhiteSpace(roleName))
            { return NotFound(); }
            bool isRoleExist = await RoleManager.RoleExistsAsync(roleName);
            if (!isRoleExist)
            {
                await RoleManager.CreateAsync(new Role { Name = roleName });
            }
            else
            {
                if (await UserManager.IsInRoleAsync(user, roleName))
                {
                    return StatusCode(StatusCodes.Status304NotModified); ;
                }
            }
            var result = await UserManager.AddToRoleAsync(user, roleName);
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

        [HttpDelete(Name = nameof(DeleteUserToRoleAsync))]
        public async Task<IActionResult> DeleteUserToRoleAsync(string userId, string roleName)
        {

            var user = await UserManager.FindByIdAsync(userId);
            if (user == null || string.IsNullOrWhiteSpace(roleName))
            { return NotFound(); }
            bool isRoleExist = await RoleManager.RoleExistsAsync(roleName);
            if (!isRoleExist)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { msg = "未找到" });
            }
            var result = await UserManager.RemoveFromRoleAsync(user, roleName);
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

        [HttpPut(Name = nameof(UpdateUserToRoleAsync))]
        public async Task<IActionResult> UpdateUserToRoleAsync(string roleName, RoleForUpdateDto roleForUpdate)
        {

            if (string.IsNullOrWhiteSpace(roleName))
            { return NotFound(); }
            var role = await RoleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { msg = "未找到role" });
            }
            Mapper.Map(roleForUpdate, role, typeof(RoleForUpdateDto), typeof(Role));
            var result = await RoleManager.UpdateAsync(role);

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

        //未完成
        [HttpPost("EditUsersInRole", Name = nameof(EditUsersInRole))]
        public async Task<IActionResult> EditUsersInRole(string roleId,[FromBody]List<UserRole> userRole)
        {
            var role = await RoleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { msg = "未找到role" });
            }
            for (int i = 0; i < userRole.Count; i++)
            {
                var user = await UserManager.FindByIdAsync(userRole[i].UserId);
                IdentityResult result = null;
                if (userRole[i].IsSelected==true&&!(await UserManager.IsInRoleAsync(user,role.Name)))
                {
                    result = await UserManager.AddToRoleAsync(user, role.Name);
                }
                else if (!userRole[i].IsSelected && await UserManager.IsInRoleAsync(user, role.Name))
                {
                    result = await UserManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                { 
                    continue;
                }
                if (result.Succeeded)
                {   //判断当前用户是否为最后一个用户，如果是则跳转回EditRole视图，如果不是则进入下一个循环
                    if (i < (userRole.Count - 1))
                        continue;
                    else
                        return StatusCode(StatusCodes.Status404NotFound, new { msg = "完成" });
                }

            }
            return StatusCode(StatusCodes.Status404NotFound, new { msg = "没有选中的用户" });
        }


    }
}