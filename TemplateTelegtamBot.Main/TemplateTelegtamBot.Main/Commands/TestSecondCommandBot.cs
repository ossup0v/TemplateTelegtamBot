using TelegramBotTemplate.CommandSteps;

namespace TelegramBotTemplate.Commands
{
    public sealed class TestSecondCommandBot : IBotCommand
    {
        public string Key => "Вторая тестовая команда";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            context.AddCommandStep(new TestSecondBotCommandStep());
            return context.SendReply("Напишите что-нибудь");
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return true;
        }
    }
}
