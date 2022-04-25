using TelegramBotTemplate.CommandSteps;

namespace TelegramBotTemplate.Commands
{
    public sealed class TestSecondCardCommandBot : IBotCommand
    {
        public string Key => "Обновить название";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            context.AddCommandStep(new TestSecondBotCommandStep());
            return context.SendReply("Напишите новое название");
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return true;
        }
    }
}
