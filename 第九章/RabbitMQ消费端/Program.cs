using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.HostName = "127.0.0.1";
factory.DispatchConsumersAsync = true;
string exchangeName = "exchange1";
string eventName = "myEvent";
using var conn = factory.CreateConnection();
using var channel = conn.CreateModel();
string queueName = "queue1";
channel.ExchangeDeclare(exchange: exchangeName, type: "direct");
channel.QueueDeclare(queue: queueName, durable: true,
        exclusive: false, autoDelete: false, arguments: null);
channel.QueueBind(queue: queueName,
    exchange: exchangeName, routingKey: eventName);
var consumer = new AsyncEventingBasicConsumer(channel);
consumer.Received += Consumer_Received;
channel.BasicConsume(queue: queueName,autoAck: false, consumer: consumer);
Console.ReadLine();
async Task Consumer_Received(object sender, BasicDeliverEventArgs args)
{
    try
    {
        var bytes = args.Body.ToArray();
        string msg = Encoding.UTF8.GetString(bytes);
        Console.WriteLine(DateTime.Now + "收到了消息" + msg);
        channel.BasicAck(args.DeliveryTag, multiple: false);
        await Task.Delay(800);
    }
    catch (Exception ex)
    {
        channel.BasicReject(args.DeliveryTag, true);
        Console.WriteLine("处理收到的消息出错" + ex);
    }
}