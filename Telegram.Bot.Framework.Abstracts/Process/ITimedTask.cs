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

namespace Telegram.Bot.Framework.Abstracts.Process
{
    /// <summary>
    /// 定时任务
    /// </summary>
    /// <remarks>
    /// 每隔一定时间执行的任务，想要实现定时任务，请继承 <see cref="AbsTimedTask"/> <br></br>
    /// 在 <see cref="AbsTimedTask"/> 内部已经实现了定时任务的机制
    /// </remarks>
    public interface ITimedTask : IExec
    {
        /// <summary>
        /// 停止执行任务
        /// </summary>
        /// <returns>异步</returns>
        public Task StopAsync();
    }
}
