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

namespace Telegram.Bot.Framework.Security
{
    /// <summary>
    /// 一个加密实现类，继承后对象会转换成Json字符串后，加密全部数据
    /// </summary>
    public abstract class EncryptDecodeBase<T> : IEncryptDecode<T> where T : EncryptDecodeBase<T>
    {
        private static byte[] KEY;
        private static byte[] IV;

        static EncryptDecodeBase()
        {
            AESHelper.SetPassword("fdGX*QVvGvbtCdQBW@wrVw3BCBPDCJFDC*rzKFH@raP@hPo6JGy!pN-PMCd2RzsK@PAwho@.ZgqRRzX@_PcyxpskzKWy.4RY3ijb", out KEY, out IV);
        }

        public static void SetPassword(string Password)
        {
            AESHelper.SetPassword(Password, out KEY, out IV);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual T Decode(string json)
        {
            string newJson = AESHelper.DecryptStringFromBytes_Aes(Convert.FromBase64String(json), KEY, IV);
            return JsonConvert.DeserializeObject<T>(newJson);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <returns></returns>
        public virtual string Encrypt()
        {
            string json = JsonConvert.SerializeObject(this);
            return Convert.ToBase64String(AESHelper.EncryptStringToBytes_Aes(json, KEY, IV));
        }
    }
}
