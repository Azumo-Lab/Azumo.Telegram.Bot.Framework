//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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

using System.Reflection;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Controller;

namespace Telegram.Bot.Framework;

/// <summary>
/// 
/// </summary>
public static partial class Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameterInfo"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public static IGetParam GetParams(this ParameterInfo parameterInfo)
    {
        Type iGetParamType = null!;

        // 从参数上获取 ParamAttribute 标签
        var paramAttribute = Attribute.GetCustomAttribute(parameterInfo, typeof(ParamAttribute)) as ParamAttribute;
        if (paramAttribute != null) // 能获取到就使用获取到的类型
            if (paramAttribute.IGetParmType != null)
                iGetParamType = paramAttribute.IGetParmType;

        // 使用默认逻辑
        if (iGetParamType == null)
        {
            var paramval = IGetParamTypeList
                .Where(y => y.ForType.ForType.FullName == parameterInfo.ParameterType.FullName)
                .Select(y => y.classType)
                .FirstOrDefault() ?? typeof(NullParam);
            iGetParamType = paramval;
        }

        // 获取构造函数
        ConstructorInfo? constructorInfo;
        if ((constructorInfo = iGetParamType.GetConstructors().OrderBy(x => x.GetParameters().Length).FirstOrDefault()) == null)
            throw new Exception("没有找到对应的初始化方法");

        // 判断是否有参数
        if (constructorInfo.GetParameters().Length != 0)
            throw new Exception("无法生成带有参数的类");

        // 实例化
        var result = constructorInfo.Invoke([]);
        if (result is IGetParam getParam)
            getParam.ParamAttribute = paramAttribute;
        else if (result == null)
            throw new NullReferenceException($"类型：{iGetParamType.FullName} 无法实例化");
        else
            throw new Exception($"类型：{iGetParamType.FullName} 未实现接口 {nameof(IGetParam)}");
        return getParam;
    }
}
