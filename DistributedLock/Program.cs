using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

// Build configuration
var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var redisConnectionString = config["ConnectionStrings:Redis"];

// Instantiate ItemProcessor
var itemProcessor = new ItemProcessor(redisConnectionString);

// Fetch item from the database (replace with your own logic)
int itemId = FetchNextItemId();

// Process the item
itemProcessor.ProcessItem(itemId);

static int FetchNextItemId()
{
    // Fetch the next item ID from the database or any other data source
    // Replace this with your own implementation
    return 1;
}

public class ItemProcessor
{
    private readonly ConnectionMultiplexer redisConnection;

    public ItemProcessor(string redisConnectionString) => redisConnection = ConnectionMultiplexer.Connect(redisConnectionString);

    public void ProcessItem(int itemId)
    {
        string lockKey = $"item-lock:{itemId}";

        var redisDb = redisConnection.GetDatabase();

        bool lockAcquired = redisDb.LockTake(lockKey, itemId, TimeSpan.FromSeconds(30));

        if (lockAcquired)
        {
            try
            {
                // Process the item
                Console.WriteLine($"Processing item {itemId}...");

                // Perform your processing steps here

                // Simulate processing time
                Thread.Sleep(TimeSpan.FromSeconds(5));

                Console.WriteLine($"Item {itemId} processed successfully.");
            }
            finally
            {
                // Release the lock
                redisDb.LockRelease(lockKey, Environment.MachineName);
            }

            return;
        }

        Console.WriteLine($"Skipping item {itemId} as it is being processed by another job instance.");
    }
}
