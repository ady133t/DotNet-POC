using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Data_Feeder.Kafka
{
    public class KafkaClient
    {
        private readonly IConfiguration _configuration;
        private const string topic = "qwert";
        //private readonly IConsumer<string, string> _consumer;

        public KafkaClient()
        {
            _configuration = ReadConfig();
            //_configuration["group.id"] = "csharp-group-1";
            //_configuration["auto.offset.reset"] = "earliest";

            //_consumer = new ConsumerBuilder<string, string>(_configuration.AsEnumerable()).Build();
            //_consumer.Subscribe(topic);
        }

        public void sendProduce(string label, int cpu_usage)
        {
            // creates a new producer instance
            using (var _producer = new ProducerBuilder<string, string>(_configuration.AsEnumerable()).Build())
            {
                // produces a sample message to the user-created topic and prints
                // a message when successful or an error occurs
              

                    string newlabel = label;
                    int newDataPoint = cpu_usage;

                    string jsonData = JsonConvert.SerializeObject(new { label = newlabel, usage = newDataPoint });

                    _producer.Produce(topic, new Message<string, string> { Key = "cpu_usage", Value = Convert.ToString(jsonData) },
                      (deliveryReport) =>
                      {
                          if (deliveryReport.Error.Code != ErrorCode.NoError)
                          {
                              Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
                          }
                          else
                          {
                              Console.WriteLine($"Produced event to topic {topic}: key = {deliveryReport.Message.Key,-10} value = {deliveryReport.Message.Value}");
                          }
                      }
                    );

                    // send any outstanding or buffered messages to the Kafka broker
                    _producer.Flush(TimeSpan.FromSeconds(10));
                   
                
            }

        }

        private IConfiguration ReadConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddIniFile("client.properties", optional: false)
                .Build();
        }
    }
}
