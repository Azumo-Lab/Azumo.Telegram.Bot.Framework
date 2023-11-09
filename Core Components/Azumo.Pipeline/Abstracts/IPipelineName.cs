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

namespace Azumo.Pipeline.Abstracts
{
    /// <summary>
    /// 流水线名称
    /// </summary>
    /// <remarks>
    /// 实现这个接口，调用 <see cref="IPipelineController{T}.NextPipelineName"/> 时，可以显示名称
    /// </remarks>
    public interface IPipelineName
    {
        /// <summary>
        /// 想要显示的名称
        /// </summary>
        public string Name { get; }
    }
}
