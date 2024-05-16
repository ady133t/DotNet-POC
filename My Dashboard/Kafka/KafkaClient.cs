using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace My_Dashboard.Kafka
{
    public class KafkaClient
    {
        private readonly IConfiguration _configuration;
        private const string Topic = "kafka-test";
        private readonly IConsumer<string, string> _consumer;

        public KafkaClient(string topic)
        {
            _configuration = ReadConfig();
            _configuration["group.id"] = "csharp-group-1";
            _configuration["auto.offset.reset"] = "earliest";

            _consumer = new ConsumerBuilder<string, string>(_configuration.AsEnumerable()).Build();
            _consumer.Subscribe(topic);
        }

        public async Task<Dictionary<string, string>> WaitForReceivedDataAsync(string topic, CancellationToken cancellationToken)
        {
            //bool iscreateTopic = await createTopic(topic);

            //if (!iscreateTopic) { return null; }
        
            
            //IConsumer<string, string> _consumer;
            //_consumer = new ConsumerBuilder<string, string>(_configuration.AsEnumerable()).Build();
            //_consumer.Subscribe(topic);

            if (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // Asynchronously poll for Kafka messages
                    var consumeResult = await Task.Run(() => { return _consumer.Consume(TimeSpan.FromSeconds(1)); }, cancellationToken);

                    if (consumeResult != null && consumeResult.Message != null)
                    {
                        var data = JsonConvert.DeserializeObject<dynamic>(consumeResult.Message.Value);
                        if (data != null)
                        {
                            var dictionary = new Dictionary<string, string>
                            {
                                { "label", (string)data?.label },
                                { "usage", (string)data?.usage }
                            };
                            return dictionary;
                        }
                    }

                    // Wait for a short time to avoid tight polling loops
                    //await Task.Delay(1000, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while consuming Kafka: {ex.Message}");
                    // Handle other exceptions, log them, or rethrow
                }
            }

            return null; // Return null if nothing received or cancelled
        }


        public async Task<bool> createTopic(string topic)
        {
           
            using var adminClient = new AdminClientBuilder(_configuration.AsEnumerable()).Build();
            if (TopicExists(adminClient,topic)) return true;
                // Define topic specification


            var topicSpec = new TopicSpecification
            {
                Name = topic,
                NumPartitions = 1,
                ReplicationFactor = 3
            };

            // Create the topic
            try
            {
                await adminClient.CreateTopicsAsync(new List<TopicSpecification> { topicSpec });
                Console.WriteLine($"Topic '{topicSpec.Name}' created successfully.");
                return true;
            }
            catch (CreateTopicsException e)
            {
                foreach (var result in e.Results)
                {
                    Console.WriteLine($"An error occurred creating topic {result.Topic}: {result.Error.Reason}");
                }

                return false;
            }

        }

        private static bool TopicExists(IAdminClient adminClient, string topicName)
        {
            try
            {
                var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
                return metadata.Topics.Any(topic => topic.Topic == topicName);
            }
            catch (KafkaException e)
            {
                Console.WriteLine($"An error occurred fetching metadata: {e.Message}");
                return false;
            }
        }


        private IConfiguration ReadConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddIniFile("client.properties", optional: false)
                .Build();
        }
    }
}
