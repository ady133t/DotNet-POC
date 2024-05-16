using Data_Feeder.Kafka;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data_Feeder.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly KafkaClient _kafkaClient;
        private readonly PerformanceCounter _cpuCounter;

        public KafkaConsumerService()
        {
            _kafkaClient = new KafkaClient();
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                _kafkaClient.sendProduce(DateTime.Now.ToString("HH:mm:ss") , (int)Math.Round(GetCurrentCpuUsage()));

                Task.Delay(1000).Wait();

            }
        }

        private float GetCurrentCpuUsage()
        {
            // The first call may not return accurate results due to initialization, so you might want to call it once to "prime" it.
            _cpuCounter.NextValue(); // Prime the counter
            Task.Delay(100).Wait();  // Give some time for the counter to gather data
            return _cpuCounter.NextValue(); // Retrieve the actual value
        }
    }

 }
