using TelegramBotTemplate.Commands;

namespace TelegramBotTemplate.CommandSteps
{
    public interface IBotCommandStep
    {
        Task ExecuteAsync(CommandExecutionContext context);
    }
}
