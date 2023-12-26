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
using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.UserAuthentication;

namespace Telegram.Bot.Framework.UserAuthentication
{
    internal class TelegramUserAuthentication : ITelegramPartCreator
    {
        public void AddBuildService(IServiceCollection services)
        {

        }
        public void Build(IServiceCollection services, IServiceProvider builderService)
        {
            _ = services.AddScoped<IControllerFilter, UserAuthenticationFilter>();
            _ = services.AddScoped<IUserManager, UserManager>();
            _ = services.AddSingleton<IGlobalBlackList, GlobalBlackList>(x =>
            {
                var blackList = new GlobalBlackList();
                blackList.Init();
                return blackList;
            });
        }
    }

    public static class UserAuthenticationInstall
    {
        public static ITelegramBotBuilder AddUserAuthentication(this ITelegramBotBuilder telegramBotBuilder) =>
            telegramBotBuilder.AddTelegramPartCreator(new TelegramUserAuthentication());
    }
}
