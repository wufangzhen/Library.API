using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Library.API.Services
{
    public class UserBasedUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}
