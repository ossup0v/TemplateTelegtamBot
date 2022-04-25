namespace TelegramBotTemplate.Commands
{
    public interface IBotCommand
    {
        string Key { get; }
        bool IsCanExecute(CommandExecutionContext context);
        Task ExecuteAsync(CommandExecutionContext context);
    }
}
