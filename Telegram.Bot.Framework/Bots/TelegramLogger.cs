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

using System.Diagnostics;
using Telegram.Bot.Framework.Abstracts.Bots;

namespace Telegram.Bot.Framework.Bots;

[DebuggerDisplay("安装自定义Log")]
internal class TelegramLogger(Action<ILoggingBuilder, IServiceProvider> action) : ITelegramPartCreator
{
    private readonly Action<ILoggingBuilder, IServiceProvider> _LogAction = action;

    public void AddBuildService(IServiceCollection services) => _ = services.AddSingleton(_LogAction);

    public void Build(IServiceCollection services, IServiceProvider builderService)
    {

    }
}

public static partial class TelegramBuilderExtensionMethods
{
    /// <summary>
    /// 添加Log设置
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static ITelegramBotBuilder AddLogger(this ITelegramBotBuilder builder, Action<ILoggingBuilder, IServiceProvider> action) => builder.AddTelegramPartCreator(new TelegramLogger(action));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static ITelegramBotBuilder AddSimpleConsole(this ITelegramBotBuilder builder)
    {
        return builder.AddTelegramPartCreator(new TelegramLogger((logbuilder, service) => logbuilder.AddSimpleConsole())); ;
    }
}
