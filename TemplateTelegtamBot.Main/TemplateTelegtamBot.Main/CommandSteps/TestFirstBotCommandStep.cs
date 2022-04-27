using TelegramBotTemplate.Commands;

namespace TelegramBotTemplate.CommandSteps
{
    public sealed class TestFirstBotCommandStep : IBotCommandStep
    {
        public Task ExecuteAsync(CommandExecutionContext context)
        {
            context.RemoveCommandStep(this);
            return context.SendAvailableCommands($"Ваши вводные данные! '{context.RawInput}'");
        }
    }
}
