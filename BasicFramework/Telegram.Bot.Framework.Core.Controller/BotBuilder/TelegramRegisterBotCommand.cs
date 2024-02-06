using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.BotBuilder;
using Telegram.Bot.Framework.Core.Controller.Controller;
using Telegram.Bot.Framework.Core.Execs;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core.Controller.BotBuilder;
internal class TelegramRegisterBotCommand : ITelegramModule
{
    public void AddBuildService(IServiceCollection services)
    {

    }
    public void Build(IServiceCollection services, IServiceProvider builderService) =>
        services.AddSingleton<IStartTask, RegisterBotCommandExec>();

    private class RegisterBotCommandExec(IServiceProvider serviceProvider) : IStartTask
    {
        private readonly IServiceProvider serviceProvider = serviceProvider;
        public async Task Exec()
        {
            var commandManager = serviceProvider.GetRequiredService<ICommandManager>();
            var botCommand = commandManager.GetExecutorList()
                .SelectMany(x => x.Attributes)
                .Where(x => x is BotCommandAttribute)
                .Select(x => (BotCommandAttribute)x)
                .Select(x => new BotCommand { Command = x.BotCommand, Description = x.Description })
                .ToList();

            var botClient = serviceProvider.GetRequiredService<ITelegramBotClient>();
            await botClient.SetMyCommandsAsync(botCommand);
        }
    }
}

public static class TelegramRegisterBotCommandExtensions
{
    public static ITelegramModuleBuilder RegisterBotCommand(this ITelegramModuleBuilder builder) =>
        builder.AddModule<TelegramRegisterBotCommand>();
}
