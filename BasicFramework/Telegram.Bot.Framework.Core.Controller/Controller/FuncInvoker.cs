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

using Telegram.Bot.Framework.Core.Controller.Storage;
using Telegram.Bot.Framework.Core.Storage;

namespace Telegram.Bot.Framework.Core.Controller.Controller;
internal class FuncInvoker(Delegate func, List<IGetParam> paramList, Attribute[] attributes)
    : IExecutor
{
    public IReadOnlyList<IGetParam> Parameters { get; } = new List<IGetParam>(paramList);

    public Attribute[] Attributes { get; } = attributes;

    public ISession Session { get; } = new SessionStorage();

    public Task Invoke(IServiceProvider serviceProvider, object[] param) =>
        func.DynamicInvoke(param) is Task task ? task : Task.CompletedTask;
}
