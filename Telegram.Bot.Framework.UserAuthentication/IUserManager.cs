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

using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.UserAuthentication
{
    /// <summary>
    /// 用户管理相应的接口
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// 在用户登录时触发的事件
        /// </summary>
        /// <remarks>
        /// 用户登录时触发的事件，使用 <see cref="SignInArgs"/> 参数获取用户的密码Hash和用户的角色信息
        /// </remarks>
        public event EventHandler<SignInArgs>? OnSignIn;

        /// <summary>
        /// 在用户登录成功时触发的事件
        /// </summary>
        public event EventHandler? OnSignInSuccess;

        /// <summary>
        /// 在用户登录失败时触发的事件
        /// </summary>
        public event EventHandler? OnSignInFailure;

        /// <summary>
        /// 在用户注册时触发的事件
        /// </summary>
        /// <remarks>
        /// 用户登录时触发，使用 <see cref="SignupArgs"/> 参数，传递到外部，进行信息的保存处理
        /// </remarks>
        public event EventHandler<SignupArgs>? OnSignUP;

        /// <summary>
        /// 在用户信息删除时触发的事件
        /// </summary>
        /// <remarks>
        /// 用户信息删除，使用 <see cref="UserDeleteArgs"/> 参数向外部传递要删除的用户信息
        /// </remarks>
        public event EventHandler<UserDeleteArgs>? OnUserDelete;

        /// <summary>
        /// 验证用户是否已经登陆
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>是否登录</returns>
        public bool IsSignIn(IUser user);

        /// <summary>
        /// 验证用户的角色是否可以访问
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="authenticateAttribute">指令的权限信息</param>
        /// <returns>是否可以访问</returns>
        public EnumVerifyRoleResult VerifyRole(IUser user, AuthenticateAttribute authenticateAttribute);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="password">用户密码</param>
        /// <returns>是否登录成功</returns>
        public Task<bool> UserSignIn(IUser user, string password);

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="password">用户密码</param>
        /// <param name="roles">用户角色信息</param>
        /// <returns>是否注册成功</returns>
        public Task<bool> UserSignUp(IUser user, string password, List<string> roles);

        /// <summary>
        /// 用户角色权限设定
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="role">角色信息</param>
        /// <returns>是否设定成功</returns>
        public Task<bool> UserRole(IUser user, string role);

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>用户是否登出成功</returns>
        public Task<bool> UserSignOut(IUser user);

        /// <summary>
        /// 屏蔽用户
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>是否屏蔽成功</returns>
        public Task<bool> UserBan(IUser user);

        /// <summary>
        /// 解除用户屏蔽
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>是否解除成功</returns>
        public Task<bool> UserUnBan(IUser user);

        /// <summary>
        /// 删除用户的信息
        /// </summary>
        /// <param name="user">要删除的用户信息</param>
        /// <returns>用户信息是否已经删除成功</returns>
        public Task<bool> UserDeleteInfo(IUser user);
    }

    /// <summary>
    /// 用户登录参数
    /// </summary>
    /// <remarks>
    /// 用户登录用参数，这个参数的数据需要从外部进行读取，读取密码Hash信息，读取对应用户的用户角色权限信息，
    /// 将信息放入这个参数之中
    /// </remarks>
    public class SignInArgs : EventArgs
    {
        /// <summary>
        /// 密码Hash
        /// </summary>
        public string? PasswordHash { get; set; }

        /// <summary>
        /// 用户权限角色信息
        /// </summary>
        public List<string> UserRoles { get; } = [];
    }

    /// <summary>
    /// 用户登录参数
    /// </summary>
    /// <remarks>
    /// 这个参数会将信息向外部传递，由外部来对这些信息进行保存处理
    /// </remarks>
    public class SignupArgs : EventArgs
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public User? User { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// 用户角色权限信息
        /// </summary>
        public List<string> UserRoles { get; } = [];
    }

    /// <summary>
    /// 用户删除参数
    /// </summary>
    /// <remarks>
    /// 这个参数用来将用户信息传递到外部，由外部进行处理
    /// </remarks>
    public class UserDeleteArgs : EventArgs
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public IUser? User { get; set; }
    }
}
