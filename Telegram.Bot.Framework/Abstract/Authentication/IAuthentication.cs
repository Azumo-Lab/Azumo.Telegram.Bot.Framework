﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.InternalImplementation.Sessions;

namespace Telegram.Bot.Framework.Authentication.Interface
{
    /// <summary>
    /// 认证用户
    /// </summary>
    public interface IAuthentication
    {
        public Task<bool> AuthUser(ITelegramSession telegramSession);

        public Task ErrorMessage(ITelegramSession telegramSession);
    }
}