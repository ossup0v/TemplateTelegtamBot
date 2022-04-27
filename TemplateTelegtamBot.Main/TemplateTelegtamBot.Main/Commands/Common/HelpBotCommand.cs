namespace TelegramBotTemplate.Commands
{
    [NotAvailableCommand]
    public sealed class HelpBotCommand : IBotCommand
    {
        public string Key => "Help";

        public Task ExecuteAsync(CommandExecutionContext context)
        {
            return context.SendCallbacks("Все доступные команды", AllCommandsHelper.GetCommandKeysToShow(context));
        }

        public bool IsCanExecute(CommandExecutionContext context)
        {
            return true;
        }
    }
}
