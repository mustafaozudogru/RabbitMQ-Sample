using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Person person = new Person() { Name = "Mustafa", SurName = "Özüdoğru", ID = 1, BirthDate = new DateTime(1989, 2, 16), Message = "Sample message" };
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "practice",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = JsonConvert.SerializeObject(person);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "practice",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine($"Sent Person: {person.Name}-{person.SurName}");
            }

            Console.WriteLine(" Contact has been sent...");
            Console.ReadLine();
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