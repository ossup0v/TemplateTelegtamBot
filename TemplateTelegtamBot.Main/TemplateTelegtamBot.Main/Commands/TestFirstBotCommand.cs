using TelegramBotTemplate.CommandSteps;

namespace TelegramBotTemplate.Commands
{
    public sealed class TestFirstBotCommand : IBotCommand
    {
        public string Key => "Первая тестовая команда";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            context.AddCommandStep(new TestFirstBotCommandStep());
            return context.SendReply("Введите вводные данные");
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return context.User.IsNeedToShow;
        }
    }
}
