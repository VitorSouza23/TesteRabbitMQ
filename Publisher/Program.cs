using System;
using System.Text;
using RabbitMQ.Client;

namespace Publisher
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
                    string message = "Teste de mensagem RabbitMQ";
                    byte[] body = Encoding.UTF8.GetBytes(message);

                    model.BasicPublish(exchange: string.Empty, routingKey: "Teste", basicProperties: null, body: body);
                    Console.WriteLine("Mensagem enviada.");
                }
            }

            Console.ReadLine();
        }
    }
}
