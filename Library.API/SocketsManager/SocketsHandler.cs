using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.API.SocketsManager
{
    public abstract class SocketsHandler
    {
        protected SocketsHandler(SocketsManager sockets)
        {
            Sockets = sockets;
        }

        public SocketsManager Sockets { get; set; }

        /// <summary>
        /// 连接一个 socket
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public virtual async Task OnConnected(WebSocket socket)
        {
            await Task.Run(() => { Sockets.AddSocket(socket); });
        }

        /// <summary>
        /// 断开指定 socket
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await Sockets.RemoveSocketAsync(Sockets.GetId(socket));
        }

        /// <summary>
        /// 发送消息给指定 socket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open) return;

            await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)),
                WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// 发送消息给指定 id 的 socket
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string id, string message)
        {
            await SendMessage(Sockets.GetSocketById(id), message);
        }

        /// <summary>
        /// 给所有 sockets 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageToAll(string message)
        {
            foreach (var connection in Sockets.GetAllConnections()) await SendMessage(connection.Value, message);
        }

        /// <summary>
        /// 接收到消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="result"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public abstract Task Receive(WebSocket socket, WebSocketReceiveResult result,
            byte[] buffer);
    }
}
