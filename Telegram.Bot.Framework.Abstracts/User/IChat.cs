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
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts.User
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChat : IDisposable
    {
        #region 用户Session存储
        public ISessionCache SessionCache { get; }
        #endregion

        #region 该次访问范围内的内容
        public IRequest Request { get; }
        #endregion

        #region 用户的信息
        public IChatInfo ChatInfo { get; }
        #endregion

        #region 服务提供器
        public IServiceProvider BotService { get; }

        public IServiceProvider ChatService { get; }
        #endregion

        #region 各种服务
        public ICallbackService CallbackService { get; }
        public ICommandService CommandService { get; }
        public IAuthenticationService AuthenticationService { get; }
        public ITaskService TaskService { get; }
        #endregion
    }
}
