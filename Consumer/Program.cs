using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = connectionFactory.CreateConnection())
            {
                using (IModel model = connection.CreateModel())
                {
                    model.QueueDeclare(queue: "Teste", durable: false, exclusive: false, autoDelete: false, arguments: null);

                    EventingBasicConsumer consumer = new EventingBasicConsumer(model);
                    consumer.Received += (model, ea) =>
                    {
                        byte[] body = ea.Body.ToArray();
                        string message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(message);
                    };

                    model.BasicConsume(queue: "Teste", autoAck: true, consumer: consumer);
                    Console.WriteLine("Consumer funcionando...");
                }
            }

            Console.ReadLine();
        }
    }
}
