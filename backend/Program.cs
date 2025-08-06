using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

builder.Services.AddSingleton<IProducer<Null, string>>(sp => {
    var config = new ProducerConfig
    {
        BootstrapServers = "localhost:9092"
    };
    return new ProducerBuilder<Null, string>(config).Build();
});


builder.Services.AddDbContext<ActivityDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    
builder.Services.AddHostedService<ActivityConsumerService>();

var app = builder.Build();

// Use the defined CORS policy
app.UseCors("AllowFrontend");


app.UseAuthorization();
app.MapControllers();

app.Run();
