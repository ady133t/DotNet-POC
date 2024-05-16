

using Microsoft.AspNetCore.SignalR;
using My_Dashboard.Hubs;
using My_Dashboard.Kafka;
using My_Dashboard.Models.DB;
using System.Threading;

namespace My_Dashboard.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly IHubContext<SignalHub> _hubContext;
        //private readonly KafkaClient _kafkaClient;
        private MachineUtilizationContext dbContext;

        public KafkaConsumerService(IHubContext<SignalHub> hubContext)
        {
            _hubContext = hubContext;
            //_kafkaClient = new KafkaClient();
            dbContext = new MachineUtilizationContext();
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            

            if (!stoppingToken.IsCancellationRequested)
            {
                var machines = dbContext.Machines.Select(x => x).Where(x => x.IsActive);

                foreach ( var machine in machines )
                {
                    KafkaClient _kafkaClient = new KafkaClient(machine.MachineId);
                    await _kafkaClient.createTopic(machine.MachineId);
                    subWorker(_kafkaClient,machine.MachineId,stoppingToken);


                }
                //var result = await _kafkaClient.WaitForReceivedDataAsync(stoppingToken);

                //if (result != null)
                //{
                //    await _hubContext.Clients.All.SendAsync("onReceiveCPU", result["label"], result["usage"]);
                //}
                
                // Wait for a short time to avoid tight polling loops
                await Task.Delay(100, stoppingToken);
            }
        }


        async Task subWorker(KafkaClient _kafkaClient,  string topic, CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await _kafkaClient.WaitForReceivedDataAsync(topic,stoppingToken);

                if (result != null)
                {
                    await _hubContext.Clients.All.SendAsync($"onReceiveCPU_{topic}", result["label"], result["usage"]);
                }

                // Wait for a short time to avoid tight polling loops
                await Task.Delay(100, stoppingToken);

            }

        }
    }
}
