using consumerredis;
using consumerredis.Service;

using StackExchange.Redis;



 RedisClient.initialize();







var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions();
ConfigurationManager configuration = builder.Configuration; 
builder.Services.Configure<ConsumerConfig>(configuration.GetSection("Consumer"));
builder.Services.AddSingleton<consumerContext>();

builder.Services.AddSingleton<IHostedService, ConsumerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
