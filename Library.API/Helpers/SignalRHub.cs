using Library.API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Library.API.Helpers
{
    //[Authorize]
    public class SignalRHub : Hub
    {
        public SignalRHub(IDistributedCache distributedCache,UserManager<User> userManager)
        {
            DistributedCache=distributedCache;
            UserManager = userManager;
        }
        IDistributedCache DistributedCache { get; set; }
        UserManager<User> UserManager { get; set; }
        /// <summary>
        /// 客户连接成功时触发
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var cid = Context.ConnectionId;
            var userid = Context.User.Identity.Name;
           
            //根据id获取指定客户端
            var client = Clients.Client(cid);

            //向指定用户发送消息
            await client.SendAsync("Self", cid);
            //像所有用户发送消息
            await Clients.All.SendAsync("AddMsg", $"{cid}加入了聊天室{userid}");
        }

        public override Task OnDisconnectedAsync(System.Exception ex)
        {
            var cid = Context.ConnectionId;
            DistributedCache.Remove(cid);
            Clients.All.SendAsync("AddMsg", $"{cid}退出").Wait();
            return base.OnDisconnectedAsync(ex);
        }
    }
}
