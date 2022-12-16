//  <Telegram.Bot.Framework>
//  Copyright (C) <2022>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Managers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    internal class TelegramUserManager : IUserManager
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IUserScopeManager _userScopeManager;

        #region 存储用户
        private readonly Dictionary<string, UserInfo> _UserNameUser = new();
        private readonly Dictionary<long, UserInfo> _UserIDUser = new();
        private readonly Dictionary<long, UserInfo> _ChatIDUser = new();
        private readonly List<UserInfo> _AllUser = new();
        private class UserInfo
        {
            public TelegramUser TelegramUser { get; set; }
            public UserStats UserStats { get; set; }

            public bool Del_Flag { get; set; }
        }
        private enum UserStats
        {
            /// <summary>
            /// 一般用户状态
            /// </summary>
            General,
            /// <summary>
            /// 被屏蔽的用户状态
            /// </summary>
            Block,
        }
        private UserInfo FindUserInfoByChatID(long ChatID)
        {
            if (_ChatIDUser.TryGetValue(ChatID, out UserInfo userInfo))
                if (!userInfo.Del_Flag)
                    return userInfo;
                else
                    _ChatIDUser.Remove(ChatID);
                
            userInfo = _AllUser.Where(x => x.TelegramUser.ChatID == ChatID).FirstOrDefault();
            if (userInfo != null)
            {
                if (userInfo.Del_Flag)
                    return null;
                _ChatIDUser.TryAdd(ChatID, userInfo);
            }
            return userInfo;
        }
        private UserInfo FindUserInfoByUserName(string UserName)
        {
            if (_UserNameUser.TryGetValue(UserName, out UserInfo userInfo))
                if (!userInfo.Del_Flag)
                    return userInfo;
                else
                    _UserNameUser.Remove(UserName);

            userInfo = _AllUser.Where(x => x.TelegramUser.Username == UserName).FirstOrDefault();
            if (userInfo != null)
            {
                if (userInfo.Del_Flag)
                    return null;
                _UserNameUser.TryAdd(UserName, userInfo);
            }
            return userInfo;
        }
        private UserInfo FindUserInfoByUserID(long UserID)
        {
            if (_UserIDUser.TryGetValue(UserID, out UserInfo userInfo))
                if (!userInfo.Del_Flag)
                    return userInfo;
                else
                    _UserIDUser.Remove(UserID);

            userInfo = _AllUser.Where(x => x.TelegramUser.Id == UserID).FirstOrDefault();
            if (userInfo != null)
            {
                if (userInfo.Del_Flag)
                    return null;
                _UserIDUser.TryAdd(UserID, userInfo);
            }
            return userInfo;
        }
        private void AddUserInfo(TelegramUser telegramUser)
        {
            _AllUser.Add(new UserInfo
            {
                TelegramUser = telegramUser,
                UserStats = UserStats.General,
            });
        }
        private void Delete(TelegramUser telegramUser)
        {
            UserInfo userInfo = FindUserInfoByUserID(telegramUser.Id);
            userInfo.Del_Flag = true;
        }
        #endregion

        public TelegramUserManager(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            _userScopeManager = this.serviceProvider.GetService<IUserScopeManager>();
        }

        public TelegramUser Me => throw new NotImplementedException();

        public List<TelegramUser> Admin => throw new NotImplementedException();

        public void Block(TelegramUser user)
        {
            UserInfo userInfo = FindUserInfoByUserID(user.Id);
            if (userInfo == null)
                return;
            
            userInfo.UserStats = UserStats.Block;
            userInfo.TelegramUser.IsBlock = true;
        }

        public TelegramUser FindUserByChatID(long ChatID)
        {
            return FindUserInfoByChatID(ChatID)?.TelegramUser;
        }

        public TelegramUser FindUserByID(long ID)
        {
            return FindUserInfoByUserID(ID)?.TelegramUser;
        }

        public List<TelegramUser> FindUserByName(string FristName, string LastName)
        {
            throw new NotImplementedException();
        }

        public TelegramUser FindUserByUserName(string UserName)
        {
            return FindUserInfoByUserName(UserName)?.TelegramUser;
        }

        public List<TelegramUser> GetUsers()
        {
            return _AllUser.Where(x => x.Del_Flag != true).Select(x => x.TelegramUser).ToList();
        }

        public IServiceScope GetUserScope(TelegramUser telegramUser)
        {
            if (telegramUser == null)
                return null;

            IUserScope userScope = _userScopeManager.GetUserScope(telegramUser);
            return userScope.GetUserServiceScope();
        }

        public IServiceScope GetUserScope(string Username)
        {
            return GetUserScope(FindUserByUserName(Username));
        }

        public IServiceScope GetUserScope(long ChatID)
        {
            return GetUserScope(FindUserByChatID(ChatID));
        }

        public TelegramUser RandomUser()
        {
            List<TelegramUser> allUsers = GetUsers();
            Random random = new(Guid.NewGuid().GetHashCode());
            int Index = random.Next(0, allUsers.Count);
            return allUsers[Index];
        }

        public void RegisterUser(TelegramUser user)
        {
            AddUserInfo(user);
        }

        public void RemoveUser(TelegramUser user)
        {
            Delete(user);
        }

        public void RemoveUser(long ID)
        {
            Delete(FindUserByID(ID));
        }

        public void Restore(TelegramUser user)
        {
            UserInfo userInfo = FindUserInfoByUserID(user.Id);
            if (userInfo == null)
                return;

            userInfo.UserStats = UserStats.General;
            userInfo.TelegramUser.IsBlock = false;
        }

        public void RegisterUser(User user, long ChatID)
        {
            TelegramUser telegramUser = new(user, ChatID);
            RegisterUser(telegramUser);
        }
    }
}
