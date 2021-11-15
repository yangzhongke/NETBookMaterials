using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.HostName = "127.0.0.1";//RabbitMQ服务器地址
factory.DispatchConsumersAsync = true;
string exchangeName = "exchange1";//交换机的名字
string eventName = "myEvent";// routingKey的值
using var conn = factory.CreateConnection();
while (true)
{
    string msg = DateTime.Now.TimeOfDay.ToString();//待发送消息
    using (var channel = conn.CreateModel())//创建信道
    {
        var properties = channel.CreateBasicProperties();
        properties.DeliveryMode = 2;
        channel.ExchangeDeclare(exchange: exchangeName, type: "direct");//声明交换机
        byte[] body = Encoding.UTF8.GetBytes(msg);
        channel.BasicPublish(exchange: exchangeName, routingKey: eventName,
            mandatory: true, basicProperties: properties, body: body);//发布消息        
    }
    Console.WriteLine("发布了消息:" + msg);
    Thread.Sleep(1000);
}