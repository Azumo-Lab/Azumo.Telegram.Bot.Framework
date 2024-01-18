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
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.UserAuthentication;

internal class UserAuthenticationFilter : IControllerFilter
{
    public async Task<bool> Execute(TelegramUserChatContext tGChat, BotCommand botCommand)
    {
        AuthenticateAttribute authenticateAttribute;

        // 指令没有权限标志（或者不是指令）
        if (botCommand?.AuthenticateAttribute == null)
            return true;

        // 指令有权限标志
        var userManager = tGChat.UserScopeService.GetRequiredService<IUserManager>();
        if (!userManager.IsSignIn(tGChat.User))
            return false;

        authenticateAttribute = botCommand.AuthenticateAttribute;

        // 开始验证
        var result = userManager.VerifyRole(tGChat.User, authenticateAttribute);
        return await Task.FromResult(result == EnumVerifyRoleResult.Success);
    }
}
