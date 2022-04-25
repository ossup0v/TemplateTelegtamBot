using TelegramBotTemplate.CommandSteps;

namespace TelegramBotTemplate.Commands
{
    public sealed class TestFirstBotCommand : IBotCommand
    {
        public string Key => "Установить API ключ";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            context.AddCommandStep(new TestFirstBotCommandStep());
            return context.SendReply("Введите API ключ");
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return context.User.IsNeedToShow;
        }
    }
}
