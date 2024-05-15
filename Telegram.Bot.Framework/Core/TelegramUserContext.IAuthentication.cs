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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Args;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Authentication;
using Telegram.Bot.Framework.Core.Controller;
using Telegram.Bot.Framework.Helpers;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core
{
    public sealed partial class TelegramContext : IAuthentication
    {
        private readonly HashSet<string> __Roles =
#if NET8_0_OR_GREATER
            [];
#else
            new HashSet<string>();
#endif

        /// <summary>
        /// 
        /// </summary>
        public static event EventHandler<RoleEventArgs>? OnInitRoles;

        /// <summary>
        /// 
        /// </summary>
        public static event EventHandler<RoleEventArgs>? OnRoleChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task ApplyChangesAsync()
        {
            var botCommands = new List<BotCommand>();

            static List<T> Get<T>(IExecutor executor) where T : Attribute => executor.Attributes.Where(x => x is T).Select(x => (T)x).ToList();

            OnRoleChanged?.Invoke(this, new RoleEventArgs
            {
                Roles = __Roles,
            });

            var commandManager = UserServiceProvider.GetRequiredService<ICommandManager>();
            var list = commandManager.GetExecutorList();
            foreach (var item in list)
                if (!(item.Cache.TryGetValue(Extensions.RolesKey, out var roles) && roles is List<string> rolesList))
                {
                    rolesList =
#if NET8_0_OR_GREATER
                        [];
#else
                        new List<string>();
#endif
                    var authList = Get<AuthenticationAttribute>(item);
                    var botcommandAttr = Get<BotCommandAttribute>(item);
                    foreach (var auth in authList)
                        foreach (var role in auth.RoleNames)
                            if (__Roles.Contains(role))
                                botCommands.Add(new BotCommand
                                {
                                    Command = botcommandAttr.First().BotCommand,
                                    Description = botcommandAttr.First().Description,
                                });
                }
            var botClient = UserServiceProvider.GetRequiredService<ITelegramBotClient>();
            await botClient.SetMyCommandsAsync(botCommands, BotCommandScope.Chat(RequestChatID));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public async Task BanThisChat()
        {
            var banList = UserServiceProvider.GetService<IBanList>();
            banList?.ChatIds?.Add(RequestChatID);
            await Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void ClearRoles() =>
            __Roles.Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string[] GetRoles() =>
#if NET8_0_OR_GREATER
            [.. __Roles];
#else
            __Roles.ToArray();
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task InitRoles()
        {
            OnInitRoles?.Invoke(this, new RoleEventArgs
            {
                Roles = __Roles,
            });
            await Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roles"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void RemoveRoles(params string[] roles) =>
            __Roles.RemoveWhere(x => roles.Contains(x));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roles"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetRoles(params string[] roles)
        {
            foreach (var item in roles)
                _ = __Roles.Add(item);
        }
    }
}
