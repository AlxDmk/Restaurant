using RabbitMQ.Client;
using System.Text;

namespace Messaging
{
    public class Producer
    {
        private readonly string _queueName;
        private readonly string _hostName;

        public Producer(string queueName, string hostName)
        {
            _queueName = queueName;
            _hostName = "sparrow.rmq.cloudamqp.com";
        }

        public void Send(string message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                Port = 5672,
                UserName = "yidbynwz",
                Password = "SZP0QrAVJ0rOGQ028Ou7fwP-xHgqJouA",
                VirtualHost = "yidbynwz"

            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(
                exchange:"fanout_exchange",
                ExchangeType.Fanout,
                durable: false,
                autoDelete: false,
                null
            );

            var body = Encoding.UTF8.GetBytes(message); 

            channel.BasicPublish(exchange: "fanout_exchange",
                routingKey: "",
                basicProperties: null,
                body: body); 
        }
    }
}
