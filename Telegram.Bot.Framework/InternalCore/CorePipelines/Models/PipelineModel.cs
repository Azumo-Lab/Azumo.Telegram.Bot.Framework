//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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

using Telegram.Bot.Framework.Core;
using Telegram.Bot.Framework.Core.Controller;

namespace Telegram.Bot.Framework.InternalCore.CorePipelines.Models
{
    /// <summary>
    /// 用于流水线输入的参数
    /// </summary>
    internal class PipelineModel
    {
#if NET8_0_OR_GREATER
        /// <summary>
        /// 
        /// </summary>
        public required TelegramContext UserContext { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public required ICommandManager CommandManager { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public required ICommandScopeService CommandScopeService { get; set; }
#else
        /// <summary>
        /// 
        /// </summary>
        public TelegramContext UserContext { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        public ICommandManager CommandManager { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        public ICommandScopeService CommandScopeService { get; set; } = null!;
#endif
    }
}
