using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotTemplate.Commands;
using TelegramBotTemplate.CommandSteps;

namespace TelegramBotTemplate
{
    public sealed class TelegramUser
    {
        public readonly long UserId;
        public readonly List<IBotCommandStep> CommandStepsQueue = new List<IBotCommandStep>();

        private CommandExecutionContext _context;
        private IReadOnlyDictionary<string, IBotCommand> _availableCommands;

        public bool IsNeedToShow = true;

        public TelegramUser(long telegramUserId, ILogger logger, IReadOnlyDictionary<string, IBotCommand> availableCommands)
        {
            UserId = telegramUserId;
            _availableCommands = availableCommands;
            _context = new CommandExecutionContext(logger);
        }

        public Task ProccessMessageInput(string input, ITelegramBotClient botClient, Update update)
        {
            _context.Fill(input, this, botClient, update);

            if (CommandStepsQueue.Count > 0)
            {
                return ProcessCommandStep();
            }

            var commandKey = input;
            _availableCommands.TryGetValue(commandKey, out var command);

            if (command == null || !command.IsCanExecute(_context))
                command = new HelpBotCommand();

            return command.ExecuteAsync(_context);
        }

        public Task ProccessCallbackQueryInput(string input, ITelegramBotClient botClient, Update update)
        {
            return ProccessMessageInput(input, botClient, update);
        }

        private Task ProcessCommandStep()
        {
            return CommandStepsQueue.First().ExecuteAsync(_context);
        }
    }
}
