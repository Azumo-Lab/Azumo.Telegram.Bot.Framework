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

using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Bot.Framework.Core.Controller.Controller;
internal class BotCommandInvoker(ObjectFactory objectFactory, Func<object, object[], object> func, List<IGetParam> paramList, Attribute[] attributes)
    : IExecutor
{
    private readonly ObjectFactory _objectFactory = objectFactory;

    private readonly Func<object, object[], object> _func = func;

    public IReadOnlyList<IGetParam> Parameters { get; } = new List<IGetParam>(paramList);

    public Attribute[] Attributes { get; } = attributes;

    public Task Invoke(IServiceProvider serviceProvider, object[] param)
    {
        var obj = _objectFactory(serviceProvider, []);
        return _func(obj, param) is Task task ? task : Task.CompletedTask;
    }
}
