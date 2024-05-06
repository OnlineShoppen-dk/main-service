using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace main_service.RabbitMQ;

public interface IRabbitMQProducer
{
    // Admin Actions
    // - Products
    public void SyncProductQueue<T>(T message);
    // TODO: PublishProductQueue is the same as PublishAddProductQueue, so we can remove one of them
    public void PublishProductQueue<T>(T message);
    public void PublishAddProductQueue<T>(T message);
    public void PublishUpdateProductQueue<T>(T message);
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
        var hostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST");
        var productQueue = Environment.GetEnvironmentVariable("RABBITMQ_PRODUCT_REMOVE_QUEUE");
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
}