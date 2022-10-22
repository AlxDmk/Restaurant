using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging
{
    public class Consumer :IDisposable
    {
        private readonly string _queueName;
        private readonly string _hostName;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public Consumer(string queueName, string hostName)
        {
            //_queueName = queueName;
            _hostName = "sparrow.rmq.cloudamqp.com";

            var fabric = new ConnectionFactory() {
                HostName = _hostName,
                Port = 5672,
                UserName = "yidbynwz",
                Password = "SZP0QrAVJ0rOGQ028Ou7fwP-xHgqJouA",
                VirtualHost = "yidbynwz"
            };
            _connection = fabric.CreateConnection();
            _channel = _connection.CreateModel();

            _queueName = _channel.QueueDeclare().QueueName;
        }

        public void Receive(EventHandler<BasicDeliverEventArgs> receiveCallback)
        {
            _channel.ExchangeDeclare(
                exchange: "fanout_exchange",
                ExchangeType.Fanout);

            //_channel.QueueDeclare(
            //    queue: _queueName,
            //    durable: false,
            //    exclusive: false,
            //    autoDelete: false,
            //    arguments: null);

            _channel.QueueBind(
                queue: _queueName,
                exchange: "fanout_exchange",
                routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += receiveCallback;

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }
    }
}
