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

using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.UserAuthentication
{
    internal class GlobalBlackList : IGlobalBlackList
    {
        public event EventHandler<UserIDArgs>? OnLoadUserID;
        public event EventHandler<UserIDArgs>? OnSaveUserID;

        private readonly HashSet<long> __BlackList = [long.MinValue];

        public void Add(long userID) =>
            __BlackList.Add(userID);

        public void Remove(long userID) =>
            __BlackList.Remove(userID);

        public bool Verify(long userID) =>
            __BlackList.Contains(userID);

        public void Init()
        {
            var userIDArgs = new UserIDArgs();
            OnLoadUserID?.Invoke(null, userIDArgs);
            foreach (var id in userIDArgs.UserIDs ?? [])
                _ = __BlackList.Add(id);
        }
        public List<long> GetList() => __BlackList.ToList();
        public bool FilterInvoke(Update update) => !Verify(update.GetRequestUser()?.Id ?? long.MinValue);
    }
}
