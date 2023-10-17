//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
//
//  This file is part of <Telegram.Bot.Framework>: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Pipeline;
using Telegram.Bot.Framework.Pipeline.Abstracts;
using Telegram.Bot.Framework.Pipelines;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    internal class TelegramBot : ITelegramBot, IUpdateHandler
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IServiceProvider ServiceProvider;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger __log;

        /// <summary>
        /// 
        /// </summary>
        private readonly IPipelineController<TGChat> pipelineController;

        /// <summary>
        /// 
        /// </summary>
        private bool __IsEnd;

        private static string LOGO =
@"
████████╗███████╗██╗     ███████╗ ██████╗ ██████╗  █████╗ ███╗   ███╗   ██████╗  ██████╗ ████████╗
╚══██╔══╝██╔════╝██║     ██╔════╝██╔════╝ ██╔══██╗██╔══██╗████╗ ████║   ██╔══██╗██╔═══██╗╚══██╔══╝
   ██║   █████╗  ██║     █████╗  ██║  ███╗██████╔╝███████║██╔████╔██║   ██████╔╝██║   ██║   ██║   
   ██║   ██╔══╝  ██║     ██╔══╝  ██║   ██║██╔══██╗██╔══██║██║╚██╔╝██║   ██╔══██╗██║   ██║   ██║   
   ██║   ███████╗███████╗███████╗╚██████╔╝██║  ██║██║  ██║██║ ╚═╝ ██║██╗██████╔╝╚██████╔╝   ██║██╗
   ╚═╝   ╚══════╝╚══════╝╚══════╝ ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝     ╚═╝╚═╝╚═════╝  ╚═════╝    ╚═╝╚═╝
                                                                                                  
███████╗██████╗  █████╗ ███╗   ███╗███████╗██╗    ██╗ ██████╗ ██████╗ ██╗  ██╗                    
██╔════╝██╔══██╗██╔══██╗████╗ ████║██╔════╝██║    ██║██╔═══██╗██╔══██╗██║ ██╔╝                    
█████╗  ██████╔╝███████║██╔████╔██║█████╗  ██║ █╗ ██║██║   ██║██████╔╝█████╔╝                     
██╔══╝  ██╔══██╗██╔══██║██║╚██╔╝██║██╔══╝  ██║███╗██║██║   ██║██╔══██╗██╔═██╗                     
██║     ██║  ██║██║  ██║██║ ╚═╝ ██║███████╗╚███╔███╔╝╚██████╔╝██║  ██║██║  ██╗                    
╚═╝     ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝ ╚══╝╚══╝  ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝                    
";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ServiceProvider"></param>
        public TelegramBot(IServiceProvider ServiceProvider)
        {
            this.ServiceProvider = ServiceProvider;

            __log = this.ServiceProvider.GetService<ILogger<TelegramBot>>();

            pipelineController = PipelineFactory.CreateIPipelineBuilder<TGChat>()
                .AddProcedure(new ProcessControllerInvoke())
                .CreatePipeline(UpdateType.Message)
                .BuilderPipelineController();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            IChatManager chatManager = ServiceProvider.GetService<IChatManager>();

            TGChat chat = chatManager.Create(botClient, update, ServiceProvider);

            _ = await pipelineController.SwitchTo(update.Type, chat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            __log.LogInformation("Start...");
            ITelegramBotClient telegramBot = ServiceProvider.GetService<ITelegramBotClient>();
            telegramBot.StartReceiving(this);

            User user = await telegramBot.GetMeAsync();
            __log.LogInformation(message: $"@{user.Username} is Running...");

            while (!__IsEnd)
            {
                if (__IsEnd)
                    await telegramBot.CloseAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task StopAsync()
        {
            __IsEnd = true;
            return Task.CompletedTask;
        }
    }
}
