﻿using Microsoft.EntityFrameworkCore;
using MyChannel.DataBaseContext;
using MyChannel.DataBaseContext.DBModels;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Users;

namespace MyChannel.Controllers
{
    internal class BlockUserAuth(MyDBContext __Context) : IAuthenticate
    {
        private readonly MyDBContext __Context = __Context;

        public HashSet<Enum> RoleName { get; set; } = [];

        public async Task<bool> IsAuthenticated(TelegramUserChatContext tGChat, AuthenticateAttribute authenticateAttribute)
        {
            if (RoleName.Count == 0)
            {
                var userInfo = await __Context.UserInfoEntity.Where(x => x.ChatID == tGChat.UserChatID).FirstOrDefaultAsync();
                if (userInfo == null || userInfo.Blocked)
                    _ = RoleName.Add(AuthEnum.NONE);
            }
            foreach (var role in RoleName)
                if (authenticateAttribute.RoleName.Contains(role))
                    return true;
            return false;
        }
    }
}
