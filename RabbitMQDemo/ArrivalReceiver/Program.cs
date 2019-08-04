using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Utility;

namespace ArrivalReceiver
{
    internal class Program
    {
        private const string WorkerName = "Worker1";

        private static void Main(string[] args)
        {
            var factory = new ConnectionFactory {HostName = Const.HostName};
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(Const.ExchangeName, Const.ExchangeType);

                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queueName, Const.ExchangeName, "");

                Console.WriteLine($"[*]{WorkerName}等待申请信息");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.Default.GetString(body);
                    Console.WriteLine($"{WorkerName}收到参数: {0}", message);
                };

                channel.BasicConsume(queueName, true, consumer);

                Console.ReadKey();
            }
        }
    }
}