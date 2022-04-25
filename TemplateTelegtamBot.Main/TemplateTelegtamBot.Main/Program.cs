using Microsoft.Extensions.Logging;
using TelegramBotTemplate.Network.Telegram;

public class Progam
{
    public static void Main(string[] args)
    {

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
            .AddFilter("Microsoft", LogLevel.Warning)
            .AddConsole();
        });

        var botManager = new BotManager(loggerFactory.CreateLogger<BotManager>());

        botManager.Start();

        Console.ReadLine();

        botManager.Stop();
    }


    public sealed class BotManager
    {
        private readonly BotMessageHandler _handler;
        private readonly BotMessageReceiver _receiver;
        private readonly ILogger<BotManager> _logger;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public BotManager(ILogger<BotManager> logger)
        {
            _handler = new BotMessageHandler(logger);
            _receiver = new BotMessageReceiver(_handler.HandleMessage, _handler.HandleError);
            _logger = logger;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            _logger.LogInformation("Bot started..");
            _receiver.StartReceive(_cancellationTokenSource);
        }

        public void Stop()
        {
            _logger.LogInformation("Bot stopped..");
            _cancellationTokenSource.Cancel();
        }
    }
}