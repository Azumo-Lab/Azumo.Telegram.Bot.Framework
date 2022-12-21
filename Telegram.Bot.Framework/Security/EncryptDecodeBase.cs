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
    /// 加密与解密的基类
    /// </summary>
    public abstract class EncryptDecodeBase<T> : IEncryptDecode<T> where T : EncryptDecodeBase<T>
    {
        private static byte[] KEY;
        private static byte[] IV;

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
