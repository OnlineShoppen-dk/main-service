using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace main_service.RabbitMQ;

public interface IRabbitMQProducer
{
    // Admin Actions
    // - Products
    // - - Sync
    public void SyncProductQueue<T>(T message);
    
    public void PublishProductQueue<T>(T message);
    
    // addProductQueue
    public void PublishAddProductQueue<T>(T message);
    // updateProductQueue
    public void PublishUpdateProductQueue<T>(T message);
    // removeProductQueue
    public void PublishRemoveProductQueue<T>(T message);
}

public class RabbitMQProducer : IRabbitMQProducer
{
    private readonly IConfiguration _configuration;
    
    public RabbitMQProducer(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void SyncProductQueue<T>(T message)
    {
        var hostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST");
        var productQueue = Environment.GetEnvironmentVariable("RABBITMQ_PRODUCT_QUEUE");
        var user = Environment.GetEnvironmentVariable("RABBITMQ_USER");
        var password = Environment.GetEnvironmentVariable("RABBITMQ_PASS");
        
        var factory = new ConnectionFactory()
        {
            HostName = hostName,
            UserName = user,
            Password = password
        };
        
        // Fetch Products
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
    
    public void PublishProductQueue<T>(T message)
    {
        var hostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
        var productQueue = Environment.GetEnvironmentVariable("RABBITMQ_PRODUCT_QUEUE") ?? "productQueue";
        var user = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "user";
        var password = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "userpass";
        
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

    public void PublishAddProductQueue<T>(T message)
    {
        var hostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST");
        var productQueue = Environment.GetEnvironmentVariable("RABBITMQ_PRODUCT_ADD_QUEUE");
        var user = Environment.GetEnvironmentVariable("RABBITMQ_USER");
        var password = Environment.GetEnvironmentVariable("RABBITMQ_PASS");
        
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

    public void PublishUpdateProductQueue<T>(T message)
    {
        var hostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST");
        var productQueue = Environment.GetEnvironmentVariable("RABBITMQ_PRODUCT_UPDATE_QUEUE");
        var user = Environment.GetEnvironmentVariable("RABBITMQ_USER");
        var password = Environment.GetEnvironmentVariable("RABBITMQ_PASS");
        
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

    public void PublishRemoveProductQueue<T>(T message)
    {
        var hostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
        var productQueue = Environment.GetEnvironmentVariable("RABBITMQ_PRODUCT_QUEUE") ?? "productQueue";
        var user = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "user";
        var password = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "userpass";
        
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