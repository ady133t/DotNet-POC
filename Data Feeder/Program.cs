using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Confluent.Kafka;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;
using Data_Feeder.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<KafkaConsumerService>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders(); // Clears default providers
        logging.AddConsole();    // Adds console logging
    });

var host = builder.Build();

// Start the application, which also starts the background service
await host.RunAsync();

Console.WriteLine("Service run");
Console.ReadLine();