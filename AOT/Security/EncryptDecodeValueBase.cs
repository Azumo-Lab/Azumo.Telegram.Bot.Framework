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
using System.Reflection;
using System.Text.Json;

namespace AOT.Security
{
    /// <summary>
    /// 一个加密实现类，继承后用于加密属性的值
    /// </summary>
    /// <remarks>
    /// 仅仅加密字段的值例如：
    /// 加密后：
    /// <code>
    /// class XXClash
    /// {
    ///     "AA": "ad0g7908r7g"
    /// }
    /// </code>
    /// 加密前：
    /// <code>
    /// class XXClash
    /// {
    ///     "AA": "123"
    /// }
    /// </code>
    /// </remarks>
    public abstract class EncryptDecodeValueBase<T> : IEncryptDecode<T> where T : EncryptDecodeValueBase<T>
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly Dictionary<string, List<PropertyInfo>> __PrivatePropertyInfos = new();

        private readonly Type __PrivateType;
        /// <summary>
        /// 子类的属性信息
        /// </summary>
        private readonly List<PropertyInfo> __PropertyInfos;

        public EncryptDecodeValueBase()
        {
            //Console.WriteLine("开始分析");

            //__PrivateType = typeof(T);
            //Console.WriteLine($"类型：{__PrivateType}");

            //string typeFullName = __PrivateType.FullName;
            //Console.WriteLine($"类型全名：{typeFullName}");

            //if (!__PrivatePropertyInfos.ContainsKey(typeFullName))
            //{
            //    PropertyInfo[] infos = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            //    foreach (PropertyInfo item in infos)
            //    {
            //        __PropertyInfos.Add(item);
            //    }
            //    __PrivatePropertyInfos.Add(typeFullName, __PropertyInfos);
            //}
            //if (!__PrivatePropertyInfos.TryGetValue(typeFullName, out __PropertyInfos))
            //    __PropertyInfos = new();

            //Console.WriteLine($"字段个数：{__PropertyInfos.Count}");
        }

        /// <summary>
        /// 初始的默认密码
        /// </summary>
        static EncryptDecodeValueBase()
        {
            AESEncrypt.SetPassword("2ux!Z@f!yJ_UKBzF-8TMeWQZG3yKYwTRFCNa_L4uKX4b7W2X.7LcVs2Pazvj_Wcy4tRCsJ@en7DYDJbHfM2JYJ-p!Fc6WW_wKVo7");
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
        /// <param name="json">加密字符串</param>
        /// <returns>解密后的数据</returns>
        public virtual T Decode(string json)
        {
            Dictionary<string, string> Obj = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            foreach (PropertyInfo item in __PropertyInfos)
            {
                if (!Obj.TryGetValue(item.Name, out string PassWordStrings))
                    continue;

                string Val = AESEncrypt.StaticDecrypt(Convert.FromBase64String(PassWordStrings));
                object objVal = Convert.ChangeType(Val, item.PropertyType);
                item.SetValue(this, objVal);
            }

            return (T)this;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <returns>加密字符串</returns>
        public virtual string Encrypt()
        {
            Console.WriteLine("开始加密");
            Dictionary<string, string> Obj = new();
            foreach (PropertyInfo item in __PropertyInfos)
            {
                Console.WriteLine($"字段个数：{__PropertyInfos.Count}");
                object val = item.GetValue(this);
                if (val == null)
                    continue;

                Console.WriteLine($"字段名称：{item.Name}");
                Obj.Add(item.Name, Convert.ToBase64String(AESEncrypt.StaticEncrypt(val.ToString())));
            }
            foreach (KeyValuePair<string, string> item in Obj)
            {
                Console.WriteLine(item.ToString());
            }
            string json = JsonSerializer.Serialize(this);
            Console.WriteLine(json);
            return json;
        }
    }
}
