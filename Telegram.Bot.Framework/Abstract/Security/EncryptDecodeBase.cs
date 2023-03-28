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

using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalImplementation.Security;

namespace Telegram.Bot.Framework.Abstract.Security
{
    /// <summary>
    /// 一个加密实现类，继承后对象会转换成Json字符串后，加密全部数据
    /// </summary>
    public abstract class EncryptDecodeBase<T> : IEncryptDecode<T> where T : EncryptDecodeBase<T>
    {
        static EncryptDecodeBase()
        {
            AESEncrypt.SetPassword("fdGX*QVvGvbtCdQBW@wrVw3BCBPDCJFDC*rzKFH@raP@hPo6JGy!pN-PMCd2RzsK@PAwho@.ZgqRRzX@_PcyxpskzKWy.4RY3ijb");
        }

        /// <summary>
        /// 设置密码，用于覆盖默认的密码，密码更改是全局的
        /// </summary>
        /// <param name="Password">密码</param>
        public static void SetPassword(string Password)
        {
            AESEncrypt.SetPassword(Password);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual T Decode(string json)
        {
            string newJson = AESEncrypt.StaticDecrypt(Convert.FromBase64String(json));
            return JsonConvert.DeserializeObject<T>(newJson);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <returns></returns>
        public virtual string Encrypt()
        {
            string json = JsonConvert.SerializeObject(this);
            return Convert.ToBase64String(AESEncrypt.StaticEncrypt(json));
        }
    }
}
