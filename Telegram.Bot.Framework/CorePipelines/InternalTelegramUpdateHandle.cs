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

using Telegram.Bot.Exceptions;
using Telegram.Bot.Framework.Abstracts.User;
using Telegram.Bot.Framework.CorePipelines.Proc;
using Telegram.Bot.Framework.Pipeline;
using Telegram.Bot.Framework.Pipeline.Abstracts;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.CorePipelines
{
    /// <summary>
    /// 框架的Handle
    /// </summary>
    internal class InternalTelegramUpdateHandle : IUpdateHandler
    {
        private readonly IServiceScope __BotScopeService;
        private readonly IUserManager __UserManager;
        private readonly IPipelineController<IChat> __PipelineController;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ServiceProvider">服务提供</param>
        public InternalTelegramUpdateHandle(IServiceProvider ServiceProvider)
        {
            __BotScopeService = ServiceProvider.CreateScope();
            __UserManager = __BotScopeService.ServiceProvider.GetService<IUserManager>();

            __PipelineController = PipelineFactory.CreateIPipelineBuilder<IChat>()
                //UpdateType.Unknown
                .AddProcedure(Create<PipelineUnknowType>())
                .CreatePipeline(UpdateType.Unknown)

                //UpdateType.Message
                .AddProcedure(Create<PipelineGetParameters>())
                .CreatePipeline(UpdateType.Message)
                .BuilderPipelineController();
        }

        ~InternalTelegramUpdateHandle()
        {
            __BotScopeService.Dispose();
        }

        #region 一些工具方法

        /// <summary>
        /// 创建新对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T Create<T>()
        {
            return ActivatorUtilities.CreateInstance<T>(__BotScopeService.ServiceProvider, Array.Empty<object>());
        }
        #endregion

        /// <summary>
        /// 错误的执行者
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 正确的执行者
        /// </summary>
        /// <param name="BotClient"></param>
        /// <param name="Update"></param>
        /// <param name="CancellationToken"></param>
        /// <returns></returns>
        public async Task HandleUpdateAsync(ITelegramBotClient BotClient, Update Update, CancellationToken CancellationToken)
        {
            try
            {
                // 创建 ITelegramChat 对象
                IChat chat = __UserManager.CreateIChat(BotClient, Update, __BotScopeService);

                _ = await __PipelineController.SwitchTo(Update.Type, chat);
            }
            catch (UnauthorizedAccessException)
            {
                // 用户被Ban，无视错误
            }
            catch (ApiRequestException)
            {
                // API 错误，网络不好，无视错误
            }
            catch (Exception Ex)
            {
                await HandlePollingErrorAsync(BotClient, Ex, CancellationToken);
            }
        }
    }
}
