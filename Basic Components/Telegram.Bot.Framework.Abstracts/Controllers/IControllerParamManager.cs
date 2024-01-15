//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Abstracts.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IControllerParamManager : IDisposable
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns></returns>
        public object[] GetParams();

        /// <summary>
        /// 进行获取参数的执行
        /// </summary>
        /// <param name="tGChat"></param>
        /// <returns></returns>
        public Task<ResultEnum> NextParam(TelegramUserChatContext tGChat);

        /// <summary>
        /// 获取命令
        /// </summary>
        /// <returns></returns>
        public BotCommand GetBotCommand();

        /// <summary>
        /// 设置命令
        /// </summary>
        /// <param name="botCommand"></param>
        public void NewBotCommandParamScope(BotCommand botCommand);
    }
}
