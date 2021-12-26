using Library.API.SocketsManager;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Helpers
{
    public class WebSocketMessageHandler : SocketsHandler
    {
        public WebSocketMessageHandler(SocketsManager.SocketsManager sockets) : base(sockets)
        {
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);
            var socketId = Sockets.GetId(socket);
            await SendMessageToAll($"{socketId}已加入");
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            await base.OnDisconnected(socket);
            var socketId = Sockets.GetId(socket);
            await SendMessageToAll($"{socketId}离开了");
        }

        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketId = Sockets.GetId(socket);
            var message = $"{socketId} 发送了消息：{Encoding.UTF8.GetString(buffer, 0, result.Count)}";
            await SendMessageToAll(message);
        }
    }
}
