using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitSender
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "practice",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Person person = JsonConvert.DeserializeObject<Person>(message);
                    Console.WriteLine($" Name: {person.Name} Surname: {person.SurName} [{person.Message}]");
                };
                channel.BasicConsume(queue: "practice",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine("Message Received. Thank you :)");
                Console.ReadLine();
            }
        }
        public class Person
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string SurName { get; set; }
            public DateTime BirthDate { get; set; }
            public string Message { get; set; }
        }
    }
}
