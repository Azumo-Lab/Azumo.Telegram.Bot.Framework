//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Framework.TelegramAttributes;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 权限认证的Controller
    /// </summary>
    public class AuthenticationController : TelegramController
    {
        [Command("Admin", CommandInfo = "管理员认证")]
        public virtual async Task AdminAuth([Param("输入或设定密码")] string Password)
        {
            if (File.Exists("PASSWORD"))
            {
                if (await File.ReadAllTextAsync("PASSWORD") == HashPassword(Password))
                {
                    IAuthenticationManager authManager = Context.UserScope.GetService<IAuthenticationManager>();
                    if (authManager.IsNull())
                        return;
                    authManager.SetAuthenticationRole(AuthenticationRole.BotAdmin);
                }
            }
            else
            {
                await File.WriteAllTextAsync("PASSWORD", HashPassword(Password));
                IAuthenticationManager authManager = Context.UserScope.GetService<IAuthenticationManager>();
                if (authManager.IsNull())
                    return;
                authManager.SetAuthenticationRole(AuthenticationRole.BotAdmin);
            }
        }

        protected static string HashPassword(string Password)
        {
            SHA256 hA256 = SHA256.Create();
            byte[] hash = Encoding.UTF8.GetBytes(Password);
            for (int i = 0; i < 765; i++)
            {
                hash = hA256.ComputeHash(hash);
            }
            return Encoding.UTF8.GetString(hash);
        }
    }
}
