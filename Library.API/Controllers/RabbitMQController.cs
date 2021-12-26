using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMQController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //创建连接工厂
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = "admin",//用户名
                Password = "123456",//密码
                HostName = "47.111.13.183",//rabbitmq ip
                Port= 5672,
                VirtualHost = "/"
            };

            //创建连接
            var connection = factory.CreateConnection();
            //创建通道
            var channel = connection.CreateModel();
            //声明一个队列
            channel.QueueDeclare("hello", false, false, false, null);
            Console.WriteLine("\nRabbitMQ连接成功，请输入消息，输入exit退出！");
            //string input;
            //do
            //{
            //    input = Console.ReadLine();
            //    var sendBytes = Encoding.UTF8.GetBytes(input);
            //    //发布消息
            //    channel.BasicPublish("", "hello", null, sendBytes);
            //}
            //while (input.Trim().ToLower() != "exit");
            var sendBytes = Encoding.UTF8.GetBytes("Hello");
            //发布消息
            channel.BasicPublish("", "hello", null, sendBytes);
            channel.Close();
            connection.Close();
            return Ok();
        }
    }
}
