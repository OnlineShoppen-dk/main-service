using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace main_service.RabbitMQ;

public interface IRabbitMQProducer
{
    // Admin Actions
    // - Products
    public void SyncProductQueue<T>(T message);
    public void PublishProductQueue<T>(T message);
    public void PublishRemoveProductQueue<T>(T message);
    
}

public class RabbitMQProducer : IRabbitMQProducer
{
    // Configurations
    private readonly string _host;
    private readonly string _user;
    private readonly string _pass;
    
    // Queues
    private readonly string _syncProductQueue;
    private readonly string _productQueue;
    private readonly string _removeProductQueue;
    
    private readonly IConfiguration _configuration;
    
    public RabbitMQProducer(IConfiguration configuration)
    {
        _configuration = configuration;

        // Setup configurations
        _host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "";
        _user = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "";
        _pass = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "";
        
        // Setup queues
        _productQueue = Environment.GetEnvironmentVariable("RABBITMQ_PRODUCT_QUEUE") ?? "";
        _syncProductQueue = Environment.GetEnvironmentVariable("RABBITMQ_PRODUCT_SYNC_QUEUE") ?? "";
        _removeProductQueue = Environment.GetEnvironmentVariable("RABBITMQ_PRODUCT_REMOVE_QUEUE") ?? "";
    }
    
    public void SyncProductQueue<T>(T message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _host,
            UserName = _user,
            Password = _pass
        };
        
        // Fetch Products
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(queue: _syncProductQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        
        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(exchange: "", routingKey: _syncProductQueue, body: body);
        channel.Close();
        connection.Close();
    }

    public void PublishProductQueue<T>(T message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _host,
            UserName = _user,
            Password = _pass
        };
        
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(queue: _productQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        
        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(exchange: "", routingKey: _productQueue, body: body);
        channel.Close();
        connection.Close();
    }

    public void PublishRemoveProductQueue<T>(T message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _host,
            UserName = _user,
            Password = _pass
        };
        
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(queue: _removeProductQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        
        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(exchange: "", routingKey: _removeProductQueue, body: body);
        channel.Close();
        connection.Close();
    }
}