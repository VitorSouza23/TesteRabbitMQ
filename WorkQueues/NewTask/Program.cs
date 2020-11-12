using System.Text;
using RabbitMQ.Client;

namespace NewTask
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
            using IModel model = connection.CreateModel();

            model.QueueDeclare(
                queue: "task_queue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            string message = GetMessage(args);
            byte[] body = Encoding.UTF8.GetBytes(message);

            model.BasicPublish(
                exchange: string.Empty,
                routingKey: "task_queue",
                basicProperties: null,
                body: body);

            WriteLine("Mensagem enviada.");
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Teste de tasks");
        }
    }
}
