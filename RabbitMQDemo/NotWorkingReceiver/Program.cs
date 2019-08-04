using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Utility;

namespace NotWorkingReceiver
{
    internal class Program
    {
        private const string WorkerName = "Worker2";

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
                consumer.Received += Consumer_Received;

                channel.BasicConsume(queueName, true, consumer);

                Console.ReadKey();
            }
        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.Default.GetString(body);

            Console.WriteLine($"{WorkerName}收到消息: {message}");
        }
    }
}