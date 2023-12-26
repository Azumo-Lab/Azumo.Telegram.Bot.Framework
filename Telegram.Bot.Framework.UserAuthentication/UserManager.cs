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
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.UserAuthentication;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.UserAuthentication
{
    internal class UserManager(IServiceProvider serviceProvider) : IUserManager
    {
        private readonly IGlobalBlackList? blackList = serviceProvider.GetService<IGlobalBlackList>();

        private static readonly string SignInFlag = Guid.NewGuid().ToString();
        private static readonly string RoleFlag = Guid.NewGuid().ToString();
        private static readonly string UserBlockFlag = Guid.NewGuid().ToString();

        public event EventHandler<SignInArgs>? OnSignIn;
        public event EventHandler? OnSignInSuccess;
        public event EventHandler? OnSignInFailure;

        public event EventHandler<SignupArgs>? OnSignUP;
        public event EventHandler<UserDeleteArgs>? OnUserDelete;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsSignIn(IUser user) =>
            user.Session.HasVal(SignInFlag);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> UserBan(IUser user)
        {
            blackList?.Add(user.User.Id);
            return Task.FromResult(user.Session.Set(UserBlockFlag, true));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> UserDeleteInfo(IUser user)
        {
            try
            {
                OnUserDelete?.Invoke(null, new UserDeleteArgs { User = user });
                return true;
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<bool> UserRole(IUser user, string role)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentException.ThrowIfNullOrEmpty(role, nameof(role));

            // 未登录
            if (!IsSignIn(user))
                return false;

            // 添加权限角色
            if (user.Session.TryGetValue(RoleFlag, out List<string>? roles))
            {
                roles!.Add(role);
            }
            else
            {
                roles = [role];
                _ = user.Session.Set(RoleFlag, roles);
            }
            return await Task.FromResult(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> UserSignIn(IUser user, string password)
        {
            var signInArgs = new SignInArgs();
            OnSignIn?.Invoke(null, signInArgs);
            if (signInArgs.PasswordHash != null)
            {
                if (signInArgs.PasswordHash == password)
                {
                    _ = user.Session.Set(SignInFlag, true);
                    _ = user.Session.Set(RoleFlag, new List<string>(signInArgs.UserRoles));
                    OnSignInSuccess?.Invoke(null, EventArgs.Empty);
                    return true;
                }
                else
                {
                    OnSignInFailure?.Invoke(null, EventArgs.Empty);
                }
            }
            return await Task.FromResult(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> UserSignOut(IUser user)
        {
            _ = user.Session.Remove(SignInFlag);
            _ = user.Session.Remove(RoleFlag);
            return Task.FromResult(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public async Task<bool> UserSignUp(IUser user, string password, List<string> roles)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentException.ThrowIfNullOrEmpty(password, nameof(password));

            var signupArgs = new SignupArgs();
            try
            {
                signupArgs.User = user.User;
                signupArgs.Password = password;
                signupArgs.UserRoles.AddRange(roles ?? []);

                OnSignUP?.Invoke(null, signupArgs);
                return true;
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="authenticateAttribute"></param>
        /// <returns></returns>
        public bool VerifyRole(IUser user, AuthenticateAttribute authenticateAttribute)
        {
            var flag = false;
            // 已屏蔽用户
            if (user.Session.HasVal(UserBlockFlag))
                return flag;
            // 检查权限
            if (user.Session.TryGetValue(RoleFlag, out List<string>? role))
                foreach (var item in role!)
                    if (flag = authenticateAttribute.RoleName.Contains(item))
                        return flag;
            return flag;
        }

        public Task<bool> UserUnBan(IUser user)
        {
            blackList?.Remove(user.User.Id);
            return Task.FromResult(user.Session.Remove(UserBlockFlag));
        }
    }
}
