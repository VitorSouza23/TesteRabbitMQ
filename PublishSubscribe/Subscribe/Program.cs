namespace Subscribe
{
    using System.Text;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
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

            channel.ExchangeDeclare(
                exchange: "logs",
                type: ExchangeType.Fanout
            );

            string queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(
                queue: queueName,
                exchange: "logs",
                routingKey: string.Empty
            );

            WriteLine("[*] Esperando por logs...");

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                WriteLine($" [x] {message}");
            };

            channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer: consumer
            );

            WriteLine("Acarque qualquer tecla para sair...");
            ReadKey();
        }
    }
}
