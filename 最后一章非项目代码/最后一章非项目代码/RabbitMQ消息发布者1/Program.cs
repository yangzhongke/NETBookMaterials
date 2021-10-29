using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.HostName = "127.0.0.1";
factory.DispatchConsumersAsync = true;
string exchangeName = "exchange1";
string eventName = "myEvent";
using var conn = factory.CreateConnection();
while(true)
{
    string msg = DateTime.Now.TimeOfDay.ToString();
    using (var channel = conn.CreateModel())//dispose才会发布消息
    {
        var properties = channel.CreateBasicProperties();
        properties.DeliveryMode = 2; // persistent
        channel.ExchangeDeclare(exchange: exchangeName, type: "direct");        
        byte[] body = Encoding.UTF8.GetBytes(msg);
        channel.BasicPublish(exchange: exchangeName,routingKey: eventName,
            mandatory: true,basicProperties: properties,body: body);        
    }
    Console.WriteLine("发布了消息:" + msg);
    Thread.Sleep(1000);
}
