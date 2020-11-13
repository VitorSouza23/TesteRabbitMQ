using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Worker
{
    using static System.Console;

    class Program
    {
        static void Main()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using IConnection connection = connectionFactory.CreateConnection();
            using IModel model = connection.CreateModel();

            model.QueueDeclare(
                queue: "task_queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            IBasicProperties basicProperties = model.CreateBasicProperties();
            basicProperties.Persistent = true;

            EventingBasicConsumer consumer = new EventingBasicConsumer(model);
            consumer.Received += (sender, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                WriteLine($"Mensagem recebida: {message}");

                int dots = message.Split('.').Length - 1;
                Thread.Sleep(dots * 1000);

                WriteLine("[x] Feito.");

                model.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            model.BasicConsume(
                queue: "task_queue",
                autoAck: true,
                consumer: consumer
            );

            WriteLine("Esperando por mensagens...");
            ReadLine();
        }
    }
}
