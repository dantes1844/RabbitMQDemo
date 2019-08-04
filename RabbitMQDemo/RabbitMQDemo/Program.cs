using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            Console.WriteLine("Press [Enter] to exit");
            var factory = new ConnectionFactory { HostName = Utility.Const.HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                while (true)
                {
                    var message = Console.ReadLine();
                    if (message == null || message == "q" || message == "Q") { break; }

                    channel.ExchangeDeclare(Utility.Const.ExchangeName, Utility.Const.ExchangeType);

                    var body = Encoding.Default.GetBytes(message);

                    channel.BasicPublish(Utility.Const.ExchangeName, "", null, body);

                    Console.WriteLine($"[x] sent {message}");
                }
            }
        }
    }
}