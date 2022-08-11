using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace consumerredis.Service
{
    public class ConsumerService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IOptions<ConsumerConfig> _config;
        private readonly consumerContext _consumerContext;


        private static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(
                    new ConfigurationOptions
                    {
                        EndPoints = { "127.0.0.1:5489" }
                    });
        public ConsumerService(ILogger<ConsumerService> logger, IOptions<ConsumerConfig> config ,
        consumerContext consumerContext)
        {
            _logger = logger;
            _config = config;
            _consumerContext = consumerContext;
        }


        static Dictionary<string, string> ParseResult(StreamEntry entry) => entry.Values.ToDictionary(x => x.Name.ToString(), x => x.Value.ToString());

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting Consumer: " + _config.Value.name);
            //consumer
            var consumerGroupReadTask = Task.Run(async () =>
            {
                string id = string.Empty;
                double count = default;
                double total = default;
                while (!RedisClient.token.IsCancellationRequested)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        await RedisClient.db.StreamAcknowledgeAsync(RedisClient.streamName, RedisClient.groupName, id);
                        id = string.Empty;
                    }
                    var result = await RedisClient.db.StreamReadGroupAsync(RedisClient.streamName, RedisClient.groupName, "avg-1", ">", 1);
                    if (result.Any())
                    {

                         var dict = ParseResult(result.First());
                         int key = 9999;
                         int.TryParse(dict["Key"], out key);
                        _consumerContext.Testtables.Add(new Testtable(){Key=key,
                        Name=dict["Name"],
                        Data=dict["Data"],});
                        _consumerContext.SaveChanges();
                        id = result.First().Id;
                        count++;
                       
                        // total += double.Parse(dict["temp"]);
                        Console.WriteLine($"Group read result: temp: {dict["Key"]}, time: {dict["Name"]}, current average: {total / count:00.00}");
                    }
                    await Task.Delay(1000);
                }
            });

            return Task.CompletedTask;
        }





        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Consumer.");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing....");

        }
    }


}
