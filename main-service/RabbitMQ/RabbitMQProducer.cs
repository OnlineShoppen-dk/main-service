using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace main_service.RabbitMQ;

public interface IRabbitMQProducer
{
    // Admin Actions
    // - Products
    public void PublishProductQueue<T>(T message);
    
    // User Actions
    // - Orders
    public void PublishNewOrderQueue<T>(T message);
}

public class RabbitMQProducer : IRabbitMQProducer
{
    private readonly IConfiguration _configuration;

    public RabbitMQProducer(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void PublishProductQueue<T>(T message)
    {
        var hostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? _configuration["RabbitMQ:Host"];
        var productQueue = Environment.GetEnvironmentVariable("RABBITMQ_ORDER_QUEUE") ?? _configuration["RabbitMQ:ProductQueue"];
        var user = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? _configuration["RabbitMQ:User"];
        var password = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? _configuration["RabbitMQ:Pass"];

        var factory = new ConnectionFactory()
        {
            HostName = hostName,
            UserName = user,
            Password = password
        };
        
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(queue: productQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        
        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(exchange: "", routingKey: productQueue, body: body);
        channel.Close();
        connection.Close();
    }

    public void PublishNewOrderQueue<T>(T message)
    {
        var hostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? _configuration["RabbitMQ:Host"];
        var productQueue = Environment.GetEnvironmentVariable("RABBITMQ_ORDER_QUEUE") ?? _configuration["RabbitMQ:ProductQueue"];
        var user = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? _configuration["RabbitMQ:User"];
        var password = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? _configuration["RabbitMQ:Pass"];
        
        var factory = new ConnectionFactory()
        {
            HostName = hostName,
            UserName = user,
            Password = password
        };
        
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(queue: productQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        
        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(exchange: "", routingKey: productQueue, body: body);
        channel.Close();
        connection.Close();
    }

    /*
    EXAMPLE USAGE:
    public void PublishMessage<T>(T message)
    {

        channel.BasicPublish(exchange: "", routingKey: "test", body: body);
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine("Received: {0}", message);
        };
        channel.BasicConsume(queue: productQueue,
            autoAck: true,
            consumer: consumer);
        
        Console.WriteLine(" [x] Sent {0}", json);
        
    }
    
    public void PublishNewProductMessage<T>(T message)
    {
        throw new NotImplementedException();
    }

    public void PublishUpdateProductMessage<T>(T message)
    {
        throw new NotImplementedException();
    }
    */
}