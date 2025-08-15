using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DotNetEnv;

Env.Load("../.env");
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://activity-frontend.s3-website.eu-north-1.amazonaws.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

builder.Services.AddSingleton<IProducer<Null, string>>(sp => {
    var config = new ProducerConfig
    {
        BootstrapServers = "redpanda:9092"
    };
    return new ProducerBuilder<Null, string>(config).Build();
});


builder.Services.AddDbContext<ActivityDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    
builder.Services.AddHostedService<ActivityConsumerService>();

builder.Services.AddSingleton<EmailService>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ActivityDbContext>();
    db.Database.Migrate();
}

app.UseCors("AllowFrontend");


app.UseAuthorization();
app.MapControllers();

app.Run();
