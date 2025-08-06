using Confluent.Kafka;
using backend.Data;
using backend.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using System;

public class ActivityConsumerService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConsumer<Ignore, string> _consumer;

    public ActivityConsumerService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "activity-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        _consumer.Subscribe("activity-events");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var cr = _consumer.Consume(stoppingToken);
                    var activity = JsonSerializer.Deserialize<ActivityEvent>(cr.Message.Value);

                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<ActivityDbContext>();
                        db.ActivityEvents.Add(activity);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Kafka Consumer Error: {ex.Message}");
                }
            }
        }, stoppingToken);
    }

    public override void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
        base.Dispose();
    }
}
