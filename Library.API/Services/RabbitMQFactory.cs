using RabbitMQ.Client;

namespace Library.API.Services
{
    public class RabbitMQFactory
    {
        public ConnectionFactory GetConnectionFactory()
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = "admin",//用户名
                Password = "123456",//密码
                HostName = "47.111.13.183",//rabbitmq ip
                Port = 5672,
                VirtualHost = "/"
            };
            return factory;
        }
    }
}
