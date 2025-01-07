//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2025>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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

using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.Controller;
using Telegram.Bot.Framework.Controller.Params;

namespace Telegram.Bot.Framework.InternalCore.Params
{
    /// <summary>
    /// 
    /// </summary>
    [TypeFor(typeof(CancellationToken))]
    internal class ParamsCancellationToken : BaseGetParamDirect
    {
        public override Task<object?> GetParam(TelegramActionContext context) =>
            Task.FromResult<object?>(context.ServiceProvider.GetRequiredService<CancellationTokenSource>().Token);
    }
}
