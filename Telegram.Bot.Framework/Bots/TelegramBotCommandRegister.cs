using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Interfaces;

namespace Telegram.Bot.Framework.Bots
{
    internal class TelegramBotCommandRegister : ITelegramPartCreator
    {
        public void AddBuildService(IServiceCollection services)
        {

        }

        public void Build(IServiceCollection services, IServiceProvider builderService)
        {
            _ = services.AddSingleton<IStartExec, RegisterBotCommand>();
        }

        private class RegisterBotCommand : IStartExec
        {
            public async Task Exec(ITelegramBotClient bot, IServiceProvider serviceProvider)
            {
                using (IServiceScope serviceScope = serviceProvider.CreateScope())
                {
                    IControllerManager controllerManager = serviceScope.ServiceProvider.GetRequiredService<IControllerManager>();
                    await bot.SetMyCommandsAsync(controllerManager.GetAllCommands().Select(x =>
                    {
                        return new Types.BotCommand()
                        {
                            Command = x.BotCommandName,
                            Description = x.Description,
                        };
                    }).ToList());
                }
            }
        }
    }

    public static partial class TelegramBuilderExtensionMethods
    {
        /// <summary>
        /// 添加基础的服务
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ITelegramBotBuilder RegisterBotCommand(this ITelegramBotBuilder builder)
        {
            return builder.AddTelegramPartCreator(new TelegramBotCommandRegister());
        }
    }
}
