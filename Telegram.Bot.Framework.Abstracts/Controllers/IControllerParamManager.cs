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
    internal interface IControllerParamManager
    {
        /// <summary>
        /// 要执行的参数获取接口
        /// </summary>
        public List<IControllerParam> ControllerParams { get; set; }

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
        public Task<ResultEnum> NextParam(TGChat tGChat);

        /// <summary>
        /// 获取命令
        /// </summary>
        /// <returns></returns>
        public BotCommand GetBotCommand();

        /// <summary>
        /// 设置命令
        /// </summary>
        /// <param name="botCommand"></param>
        public void SetBotCommand(BotCommand botCommand);

        /// <summary>
        /// 全部清理
        /// </summary>
        public void Clear();
    }
}
