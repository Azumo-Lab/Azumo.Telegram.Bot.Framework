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
    public interface IUserManager
    {
        public event EventHandler<SignInArgs>? OnSignIn;
        public event EventHandler? OnSignInSuccess;
        public event EventHandler? OnSignInFailure;

        public event EventHandler<SignupArgs>? OnSignUP;

        public event EventHandler<UserDeleteArgs>? OnUserDelete;

        public bool IsSignIn(IUser user);

        public bool VerifyRole(IUser user, AuthenticateAttribute authenticateAttribute);

        public Task<bool> UserSignIn(IUser user, string password);

        public Task<bool> UserSignUp(IUser user, string password, List<string> roles);

        public Task<bool> UserRole(IUser user, string role);

        public Task<bool> UserSignOut(IUser user);

        public Task<bool> UserBan(IUser user);

        public Task<bool> UserUnBan(IUser user);

        public Task<bool> UserDeleteInfo(IUser user);
    }

    public class SignInArgs : EventArgs
    {
        public string? PasswordHash { get; set; }

        public List<string> UserRoles { get; } = [];
    }

    public class SignupArgs : EventArgs
    {
        public User? User { get; set; }
        public string? Password { get; set; }
        public List<string> UserRoles { get; } = [];
    }

    public class UserDeleteArgs : EventArgs
    {
        public IUser? User { get; set; }
    }
}
