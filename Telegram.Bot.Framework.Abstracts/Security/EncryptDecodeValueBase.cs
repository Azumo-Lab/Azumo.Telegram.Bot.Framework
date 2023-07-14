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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstracts.Security
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
        private readonly static Dictionary<string, List<PropertyInfo>> __PrivatePropertyInfos = new();
        private readonly Type __PrivateType;
        /// <summary>
        /// 子类的属性信息
        /// </summary>
        private readonly List<PropertyInfo> __PropertyInfos;

        public EncryptDecodeValueBase()
        {
            __PrivateType = typeof(T);
            string typeFullName = __PrivateType.FullName;
            if (!__PrivatePropertyInfos.ContainsKey(typeFullName))
                __PrivatePropertyInfos.Add(typeFullName, __PrivateType.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList());
            if (!__PrivatePropertyInfos.TryGetValue(typeFullName, out __PropertyInfos))
                __PropertyInfos = new();
        }

        /// <summary>
        /// 初始的默认密码
        /// </summary>
        static EncryptDecodeValueBase()
        {
            InternalAESEncrypt.SetPassword("2ux!Z@f!yJ_UKBzF-8TMeWQZG3yKYwTRFCNa_L4uKX4b7W2X.7LcVs2Pazvj_Wcy4tRCsJ@en7DYDJbHfM2JYJ-p!Fc6WW_wKVo7");
        }

        /// <summary>
        /// 设置密码，用于覆盖默认的密码，密码更改是全局的
        /// </summary>
        /// <param name="Password">密码</param>
        public static void SetPassword(string Password)
        {
            InternalAESEncrypt.SetPassword(Password);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="json">加密字符串</param>
        /// <returns>解密后的数据</returns>
        public virtual T Decode(string json)
        {
            Dictionary<string, string> Obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            foreach (PropertyInfo item in __PropertyInfos)
            {
                if (!Obj.TryGetValue(item.Name, out string PassWordStrings))
                    continue;

                string Val = InternalAESEncrypt.StaticDecrypt(Convert.FromBase64String(PassWordStrings));
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
            Dictionary<string, string> Obj = new();
            foreach (PropertyInfo item in __PropertyInfos)
            {
                object val = item.GetValue(this);
                if (val == null)
                    continue;

                Obj.Add(item.Name, Convert.ToBase64String(InternalAESEncrypt.StaticEncrypt(val.ToString())));
            }
            return JsonConvert.SerializeObject(Obj);
        }
    }
}
