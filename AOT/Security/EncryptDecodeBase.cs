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

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace AOT.Security
{
    /// <summary>
    /// 一个加密实现类，继承后对象会转换成Json字符串后，加密全部数据
    /// </summary>
    public class EncryptDecode<T> : IEncryptDecode<T> where T : class
    {
        private Lazy<T> __Value;

        public T Value => __Value.Value;

        static EncryptDecode()
        {
            AESEncrypt.SetPassword("fdGX*QVvGvbtCdQBW@wrVw3BCBPDCJFDC*rzKFH@raP@hPo6JGy!pN-PMCd2RzsK@PAwho@.ZgqRRzX@_PcyxpskzKWy.4RY3ijb");
        }

        public EncryptDecode()
        {
            __Value = new Lazy<T>();
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
        [RequiresDynamicCode("")]
        [RequiresUnreferencedCode("")]
        public virtual T? Decode(string json)
        {
            string newJson = AESEncrypt.StaticDecrypt(Convert.FromBase64String(json));
            try
            {
                T objT = JsonSerializer.Deserialize<T>(newJson)!;
                if (objT != null)
                {
                    __Value = new Lazy<T>(objT);
                }
                return __Value.Value;
            }
            catch (Exception)
            {
            }
            return null;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <returns></returns>
        public virtual string Encrypt()
        {
            string json = JsonSerializer.Serialize(__Value.Value, typeof(T))!;

            Console.WriteLine(__Value.Value.ToString());
            Console.WriteLine(json);

            return Convert.ToBase64String(AESEncrypt.StaticEncrypt(json));
        }
    }
}
