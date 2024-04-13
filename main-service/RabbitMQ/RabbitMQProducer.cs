using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace main_service.RabbitMQ;

public interface IRabbitMQProducer
{
    public void PublishMessage < T > (T message);
}

public class RabbitMQProducer : IRabbitMQProducer
{
    public void PublishMessage<T>(T message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672
        };
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: "test", durable: false, exclusive: false, autoDelete: false, arguments: null);
        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(exchange: "", routingKey: "test", body: body);
        Console.WriteLine(" [x] Sent {0}", message);
    }
}