using Microsoft.Extensions.Configuration;
using Library.API.Entities;
using Library.API.Helpers;
using Library.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace Library.API.Controllers
{
    [Route("/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public AccountController(
               UserManager<User> userManager,
               RoleManager<Role> roleManager,
               IConfiguration configuration,
               ILogger<AccountController> logger,
               SignInManager<User> signInManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            Configuration = configuration;
            Logger = logger;
            SignInManager = signInManager;
        }

        public IConfiguration Configuration { get; }

        public RoleManager<Role> RoleManager { get; }

        public UserManager<User> UserManager { get; }
        public ILogger<AccountController> Logger { get; }

        public SignInManager<User> SignInManager { get; }

        [HttpPost("register", Name = nameof(Register))]
        public async Task<IActionResult> Register(RegisterUser registerUser)
        {
            var user = new User
            {
                Email = registerUser.Email,
                UserName = registerUser.UserName,
                //BirthDate = registerUser.BirthDate
            };

            IdentityResult result = await UserManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                //生成电子邮件确认令牌
                var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                //生成电子邮件的确认链接
                var confirmationLink = Url.Action("ConfirmEmail", "Account",
                new { userId = user.Id, token = token }, Request.Scheme);
                //需要注入ILogger<AccountController> _logger;服务，记录生成的URL链接
                Logger.Log(LogLevel.Warning, confirmationLink);
                SendEmail.SendEmailBy(user.Email, user.UserName, "验证邮箱", confirmationLink, false);
                //如果用户已登录并属于Admin角色。
                //那么就是Admin正在创建新用户。
                //所以重定向Admin用户到ListRoles的视图列表
                if (SignInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                {
                    //
                }
                return Ok(new{ success="wd" });
            }
            else
            {
                ModelState.AddModelError("Error", result.Errors.FirstOrDefault()?.Description);
                return BadRequest(ModelState);
            }
            
        }

        [HttpGet("ConfirmEmail", Name = nameof(ConfirmEmail))]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("index", "home");
            }

            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                ModelState.AddModelError("Error","用户名为空");
                return BadRequest(ModelState);
            }

            var result = await UserManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return Redirect(BaseConfig.TargerUrl +"/message");
            }

            ModelState.AddModelError("Error","未知错误");
            return BadRequest(ModelState);
        }

        [HttpPost("Login", Name = nameof(Login))]
        public async Task<IActionResult> Login(LoginUser loginUser, string returnUrl)
        {
            User user;
            if(RegexHelper.IsEmail(loginUser.UserName))
            {
                user = await UserManager.FindByEmailAsync(loginUser.UserName);
            }
            else
            {
                user = await UserManager.FindByNameAsync(loginUser.UserName);
            }
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "未找到用户");
            }

                if ( !user.EmailConfirmed &&
                  (await UserManager.CheckPasswordAsync(user, loginUser.Password)))
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "您的电子邮箱还未进行验证。");
                }            
                var result = UserManager.PasswordHasher.VerifyHashedPassword(user,
                 user.PasswordHash,
                 loginUser.Password);
                if (result != PasswordVerificationResult.Success)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "密码错误");
                }

            var p =await ReturnLoginInfo(user);
            return Ok(p);
        }

        private async Task<LoginToken> ReturnLoginInfo(User user)
        {
            var userClaims = await UserManager.GetClaimsAsync(user);
            var userRoles = await UserManager.GetRolesAsync(user);
            foreach (var roleItem in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, roleItem));
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            claims.AddRange(userClaims);

            var tokenConfigSection = Configuration.GetSection("Security:Token");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigSection["Key"]));
            var signCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: tokenConfigSection["Issuer"],
                audience: tokenConfigSection["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: signCredential);

            return new LoginToken
            {
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                expiration = TimeZoneInfo.ConvertTimeFromUtc(jwtToken.ValidTo, TimeZoneInfo.Local),
                UserId=user.Id
                
            };
        }

        [HttpGet("Login", Name = nameof(Login))]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var extrenalName = (await SignInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            List<ExternalInfo> result = new List<ExternalInfo>();
            foreach(var i in extrenalName)
            {
                result.Add(new ExternalInfo
                {
                    Name = i.Name,
                    Url= Url.Link(nameof(ExternalLogin),new { provider=i.Name, returnUrl=returnUrl })

                });
            }
            return Ok(result);
        }

        [HttpPost("ExternalLogin", Name = nameof(ExternalLogin))]
        [HttpGet("ExternalLogin", Name = nameof(ExternalLogin))]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
                                new { ReturnUrl = returnUrl });
            var properties = SignInManager
                .ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [HttpGet("externallogincallback", Name = "externallogincallback")]
        [AllowAnonymous]
        [HttpGet("signin-microsoft")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginUser loginuser = new LoginUser
            {
                ReturnUrl = returnUrl,
                ExternalLogins =(await SignInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"外部提供程序错误: {remoteError}");
                BadRequest(ModelState);
            }

            // 从外部登录提供者,即微软账户体系中，获取关于用户的登录信息。
            var info = await SignInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "加载外部登录信息出错");
            }

            // 获取邮箱地址
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            User user = null;

            if (email != null)
            {
                // 通过邮箱地址去查询用户是否已存在
                user = await UserManager.FindByEmailAsync(email);

                // 如果电子邮件没有被确认，返回登录视图与验证错误
                if (user != null && !user.EmailConfirmed)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "未激活邮件");
                }
            }

            //如果用户之前已经登录过了，会在AspNetUserLogins表有对应的记录，这个时候无需创建新的记录，直接使用当前记录登录系统即可。
            var signInResult = await SignInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                dynamic p =await ReturnLoginInfo(user);
                return Redirect(BaseConfig.TargerUrl + returnUrl+"?token="+p.token+"&userid="+user.Id);
            }

            //如果AspNetUserLogins表中没有记录，则代表用户没有一个本地帐户，这个时候我们就需要创建一个记录了。       
            else
            {

                if (email != null)
                {

                    if (user == null)
                    {
                        user = new User
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };
                        //如果不存在，则创建一个用户，但是这个用户没有密码。
                        await UserManager.CreateAsync(user);

                        //生成电子邮件确认令牌
                        var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);

                        //生成电子邮件的确认链接
                        var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token = token }, Request.Scheme);
                        //需要注入ILogger<AccountController> _logger;服务，记录生成的URL链接
                        Logger.Log(LogLevel.Warning, confirmationLink);
                        SendEmail.SendEmailBy(user.Email, user.UserName, "验证邮箱", confirmationLink, false);
                    }

                    // 在AspNetUserLogins表中,添加一行用户数据，然后将当前用户登录到系统中
                    await UserManager.AddLoginAsync(user, info);
                    await SignInManager.SignInAsync(user, isPersistent: false);

                    return StatusCode(StatusCodes.Status200OK, "请查看电子邮件以激活账号");
                }

                return Ok("Error");
            }
        }

        [HttpPost("Logout", Name = nameof(Logout))]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return Ok();
        }
    }
}
