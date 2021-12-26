using Library.API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Library.API.Controllers
{
    
    [Route("SignIR")]
    public class SignalRController : ControllerBase
    {
        private readonly IHubContext<SignalRHub> _countHub;
        IDistributedCache DistributedCache { get; set; }

        public SignalRController(IHubContext<SignalRHub> countHub, IDistributedCache distributedCache)
        {
            _countHub = countHub;
        }

        [HttpGet]
        public async Task<IActionResult> ConnectionSignalR(string userid, string connection)
        {
            DistributedCache.SetString(connection, userid);
           // await _countHub.Clients.All.SendAsync("AddMsg", $"{user}：{message}");
            return Ok("ok");
        }
        [HttpPost("user")]

        public async Task<IActionResult> send(string userId,string message)
        {
            await _countHub.Clients.Client(userId).SendAsync(message);
            //await _countHub.Groups.AddToGroupAsync(userId, group);
            return Ok();
        }
        [HttpPost("joinGroup")]
        public async Task<IActionResult> joinGroup(string userid,string group)
        {
            await _countHub.Groups.AddToGroupAsync(userid, group);
            return Ok();
        }
        [HttpPost("sendgroup")]
        public async Task<IActionResult> sendGroup(string group,string message)
        {
            await _countHub.Clients.Group(group).SendAsync(message);
            return Ok();
        }
    }
}
