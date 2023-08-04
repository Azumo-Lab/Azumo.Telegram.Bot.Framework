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

using Telegram.Bot.Framework.Abstracts.Services;

namespace Telegram.Bot.Framework.Abstracts.User
{
    /// <summary>
    /// 用户的聊天信息接口
    /// </summary>
    /// <remarks>
    /// 这个接口里面包含了所有能用到的数据
    /// </remarks>
    public interface IChat : IDisposable
    {
        #region 用户Session存储
        /// <summary>
        /// 存储
        /// </summary>
        public ISessionCache SessionCache { get; }
        #endregion

        #region 该次访问范围内的内容
        /// <summary>
        /// 请求
        /// </summary>
        public IRequest Request { get; }
        #endregion

        #region 用户的信息
        /// <summary>
        /// 聊天信息
        /// </summary>
        public IChatInfo ChatInfo { get; }
        #endregion

        #region 服务提供器
        /// <summary>
        /// Bot范围的服务提供
        /// </summary>
        public IServiceProvider BotService { get; }

        /// <summary>
        /// Chat范围的服务提供
        /// </summary>
        public IServiceProvider ChatService { get; }
        #endregion

        #region 各种服务
        public ITelegramBotClient BotClient { get; }

        /// <summary>
        /// 回调的管理
        /// </summary>
        public ICallbackService CallbackService { get; }

        /// <summary>
        /// Bot指令的管理
        /// </summary>
        public ICommandService CommandService { get; }

        /// <summary>
        /// 权限管理
        /// </summary>
        public IAuthenticationService AuthenticationService { get; }

        /// <summary>
        /// 任务管理
        /// </summary>
        public ITaskService TaskService { get; }

        // GUN协议的软件工程
        #endregion
    }
}
