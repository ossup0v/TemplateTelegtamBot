using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotTemplate.CommandSteps;

namespace TelegramBotTemplate.Commands
{
    public sealed class CommandExecutionContext
    {
        public TelegramUser? User;
        public ITelegramBotClient? BotClient;
        public Update? Update;
        public string? RawInput;
        private readonly ILogger _logger;
        
        private int _userMessageId => Update?.Message?.MessageId ?? Update?.CallbackQuery?.Message?.MessageId ?? 0;
        private long _chatId => Update?.Message?.Chat?.Id ?? Update.CallbackQuery.Message.Chat.Id;
        private string _userName => Update?.Message?.Chat?.Username ?? Update?.CallbackQuery?.Message?.Chat?.Username ?? "John Doe";


        private int _lastSendedMessageId;
        public CommandExecutionContext(ILogger logger)
        {
            _logger = logger;
        }

        public void Fill(string input, TelegramUser owner, ITelegramBotClient botClient, Update update)
        { 
            RawInput = input;
            User = owner;
            BotClient = botClient;
            Update = update;
        }

        public async Task SendReply(string titleText, params string[] textButtons)
        {
            _logger.LogInformation($"User: {_userName} recieved keyboard: \'{titleText}\' with buttons:{Environment.NewLine}{string.Join(Environment.NewLine, textButtons)}");

            try
            {
                await RemoveMessage();
                await UpdateReply(titleText, textButtons);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Bad Request: message is not modified: specified new message content and reply markup are exactly the same as a current content and reply markup of the message")
                {
                    //it's okay
                    return;
                }

                await SendCallbacks(titleText, textButtons);
            }
        }

        public Task RemoveMessage()
        {
            if (_userMessageId == _lastSendedMessageId)
                return Task.CompletedTask;

            return BotClient.DeleteMessageAsync(
                    chatId: _chatId,
                    messageId: _userMessageId);
        }
        public async Task SendCallbacks(string text, params string[] callbacks)
        {
            var buttonCallbackData = callbacks.Select(x => new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(x) });
            _logger.LogInformation($"Send message to {_chatId} - {_userName} message:" + Environment.NewLine + text + Environment.NewLine + "and callbacks" + Environment.NewLine + String.Join(Environment.NewLine, callbacks));

            var sendedMessage = await BotClient.SendTextMessageAsync(
                    chatId: _chatId,
                    text: text,
                    parseMode: ParseMode.Markdown,
                    disableNotification: true,
                    replyMarkup: new InlineKeyboardMarkup(buttonCallbackData)
            );

            _lastSendedMessageId = sendedMessage.MessageId;
        }

        private Task UpdateReply(string titleText, params string[] textButtons)
        {
            var buttons = textButtons
                .Select(x => new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(x) })
                .ToList();

            var inlineKeyboard = new InlineKeyboardMarkup(buttons);

            return BotClient.EditMessageTextAsync(
                        chatId: _chatId,
                        text: titleText,
                        messageId: _lastSendedMessageId,
                        replyMarkup: inlineKeyboard,
                        parseMode: ParseMode.MarkdownV2
                        );
        }
        public void RemoveCommandStep(IBotCommandStep step)
        {
            User.CommandStepsQueue.Remove(step);
        }

        public void AddCommandStep(IBotCommandStep step)
        {
            User.CommandStepsQueue.Add(step);
        }

        public Task SendAvailableCommands(params string[] titleText)
        {
            return SendReply(String.Join(Environment.NewLine, titleText), AllCommandsHelper.GetCommandKeysToShow(this));
        }
    }
}
