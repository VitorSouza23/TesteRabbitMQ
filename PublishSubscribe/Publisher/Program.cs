using RabbitMQ.Client;
using System.Text;

namespace Publish
{
    using static System.Console;

    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using IConnection connection = connectionFactory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "log", type: ExchangeType.Fanout);

            string message = GetMessage(args);
            byte[] body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: "logs",
                routingKey: string.Empty,
                basicProperties: null,
                body: body
            );

            WriteLine($"[x] Enviado: {message}");
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0)
               ? string.Join(" ", args)
               : "info: Hello World!");
        }
    }
}
