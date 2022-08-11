using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace consumerredis.Controllers;

[ApiController]
[Route("[controller]")]
public class ConsumerRedisController : ControllerBase
{


    private readonly ILogger<ConsumerRedisController> _logger;

    public ConsumerRedisController(ILogger<ConsumerRedisController> logger)
    {
        _logger = logger;
    }

    [HttpGet("/Stop", Name = "GetConsumerRedis")]
    public string Get()
    {
        RedisClient.tokenSource.CancelAfter(TimeSpan.FromSeconds(20));
        return "stopped";
    }

    [HttpPost("/AddEvents", Name = "AddEvent")]
    public void AddProducts(EntityPayload graphPayload)
    {
        producer(graphPayload);
    }


    public static async void producer(EntityPayload graphPayload)
    {

        var producerTask = Task.Run(async () =>
        {
            var random = new Random();
            await RedisClient.db.StreamAddAsync(RedisClient.streamName,
                new NameValueEntry[]
                    {new("Key", graphPayload.id), new NameValueEntry("Name",
                         graphPayload.name),
                            new NameValueEntry("Data", graphPayload.data)});
            await Task.Delay(2000);
        });

    }
}
