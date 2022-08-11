
using StackExchange.Redis;

public static class RedisClient
{
    public static CancellationTokenSource tokenSource;
    public static CancellationToken token;

    public static ConnectionMultiplexer muxer;
    public static IDatabase db;
    public static string streamName = "telemetry";
    public static string groupName = "avg";

    public static async void initialize()
    {

    //     tokenSource = new CancellationTokenSource();
    //     token = tokenSource.Token;

    //     muxer = ConnectionMultiplexer.Connect("172.21.0.3:6379");
    //     db = muxer.GetDatabase();
    //     if (!(await db.KeyExistsAsync(streamName)) ||
    //  (await db.StreamGroupInfoAsync(streamName)).All(x => x.Name != groupName))
    //     {
    //         await db.StreamCreateConsumerGroupAsync(streamName, groupName, "0-0", true);
    //     }

    }

}