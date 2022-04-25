using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace TelegramBotTemplate.Network.Telegram
{
    public sealed class BotMessageReceiver
    {
        private readonly Func<ITelegramBotClient, Update, CancellationToken, Task> _receiveMessage;
        private readonly Func<ITelegramBotClient, Exception, CancellationToken, Task> _handleError;
        private readonly TelegramBotClient _botClient;

        public BotMessageReceiver(Func<ITelegramBotClient, Update, CancellationToken, Task> receiveMessage,
            Func<ITelegramBotClient, Exception, CancellationToken, Task> handleError)
        {
            _receiveMessage = receiveMessage;
            _handleError = handleError;
            _botClient = new TelegramBotClient(Settings.TelegramBotToken);
        }

        public void StartReceive(CancellationTokenSource cts, ReceiverOptions options = null)
        {
            if (options == null)
            {
                options = new ReceiverOptions { AllowedUpdates = { } };
            }

            _botClient.StartReceiving(_receiveMessage, _handleError, options, cancellationToken: cts.Token);
        }
    }
}
