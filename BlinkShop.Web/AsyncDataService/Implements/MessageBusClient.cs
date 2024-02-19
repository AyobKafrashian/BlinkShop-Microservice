using BlinkShop.Web.AsyncDataService.Interfaces;
using BlinkShop.Web.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace BlinkShop.Web.AsyncDataService.Implements
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;

            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                VirtualHost = "/",
                UserName = "guest",
                Password = "guest",
                Port = int.Parse(_configuration["RabbitMQPort"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to MessageBus");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }


        public void PublishCreateNewCoupon(CouponDto couponDto)
        {
            try
            {
                var message = JsonSerializer.Serialize(couponDto);
                if (_connection.IsOpen)
                {
                    Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
                    SendMessage(message);
                }
                else
                    Console.WriteLine("--> RabbitMQ connectionis closed, not sending");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error in create new coupon ===> {ex.Message}");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                            routingKey: "test_queue",
                            basicProperties: null,
                            body: body);
            Console.WriteLine($"--> We have sent {message}");
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}
