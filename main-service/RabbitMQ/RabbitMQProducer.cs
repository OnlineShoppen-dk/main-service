using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace main_service.RabbitMQ;

public interface IRabbitMQProducer
{
    public void PublishProductQueue<T>(T message);
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
        var hostName = _configuration["RABBITMQ_HOST"];
        var productQueue = _configuration["RABBITMQ_PRODUCT_QUEUE"];
        var user = _configuration["RABBITMQ_USER"];
        var password = _configuration["RABBITMQ_PASSWORD"];
        
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
        channel.BasicPublish(exchange: "", routingKey: "test", body: body);
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