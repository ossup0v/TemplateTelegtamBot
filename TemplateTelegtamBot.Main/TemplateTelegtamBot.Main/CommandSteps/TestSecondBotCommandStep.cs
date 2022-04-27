using TelegramBotTemplate.Commands;

namespace TelegramBotTemplate.CommandSteps
{
    public sealed class TestSecondBotCommandStep : IBotCommandStep
    {
        public Task ExecuteAsync(CommandExecutionContext context)
        {
            context.RemoveCommandStep(this);
            
            context.User.IsNeedToShow = !context.User.IsNeedToShow;

            return context.SendAvailableCommands($"Название успешно изменено на {context.RawInput}");
        }
    }
}
