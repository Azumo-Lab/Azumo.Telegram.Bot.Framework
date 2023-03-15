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

using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework;
using Telegram.Bot.Upgrader.Bot;
using IConfig = Telegram.Bot.Framework.Abstract.Config.IConfig;

namespace Telegram.Bot.Upgrader
{
    /// <summary>
    /// 配置
    /// </summary>
    public class StartUp : IConfig
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddUserAuthentication();
            _ = services.AddSingleton<IBotManager, BotManager>();
            _ = services.AddSingleton<IProcessManager, ProcessManager>();
        }
    }
}
