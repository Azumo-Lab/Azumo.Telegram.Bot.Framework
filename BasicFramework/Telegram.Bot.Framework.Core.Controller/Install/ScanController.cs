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

using Azumo.SuperExtendedFramework;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.BotBuilder;
using Telegram.Bot.Framework.Core.Controller.Controller;
using Telegram.Bot.Framework.Core.I18N;
using Telegram.Bot.Framework.Core.Users;

namespace Telegram.Bot.Framework.Core.Controller.Install;

/// <summary>
/// 
/// </summary>
internal class ScanController : ITelegramModule
{
    private readonly IServiceCollection ScopeServices = new ServiceCollection();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    public void AddBuildService(IServiceCollection services)
    {
        ScopeServices.AddSingleton<ICommandManager, CommandManager>();

        var serviceProvider = ScopeServices.BuildServiceProvider();

        // 获取参数实现类
        var getparamTypeList = typeof(IGetParam).GetAllSameType()
            .Where(x => Attribute.IsDefined(x, typeof(TypeForAttribute)))
            .Select(x => (x, (TypeForAttribute)Attribute.GetCustomAttribute(x, typeof(TypeForAttribute))!))
            .ToList();

        var commandManager = serviceProvider.GetRequiredService<ICommandManager>();

        var controllerTypeList = typeof(TelegramControllerAttribute).GetHasAttributeType();
        foreach ((var controller, var _) in controllerTypeList)
        {
            foreach ((var method, var attr) in controller.GetAttributeMethods<BotCommandAttribute>(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                var attributes = new List<Attribute>
                {
                    attr
                };
                attributes.AddRange(method.GetCustomAttributes());
                var executor = Factory.GetExecutorInstance(EnumCommandType.BotCommand,
                    ActivatorUtilities.CreateFactory(controller, []),
                    method.BuildFunc(),
                    method.GetParameters().Select(x =>
                    {
                        Type iGetParamType = null!;

                        // 从参数上获取 ParamAttribute 标签
                        var paramAttribute = Attribute.GetCustomAttribute(x, typeof(ParamAttribute)) as ParamAttribute;
                        if (paramAttribute != null) // 能获取到就使用获取到的类型
                            if (paramAttribute.IGetParmType != null)
                                iGetParamType = paramAttribute.IGetParmType;

                        // 使用默认逻辑
                        if (iGetParamType == null)
                        {
                            var paramval = getparamTypeList
                                .Where(y => y.Item2.ForType.FullName == x.ParameterType.FullName)
                                .Select(y => y.x)
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

                    }).ToList(),
                    attributes.ToArray());
                commandManager.AddExecutor(executor);
            }
        }

        services.AddSingleton(commandManager);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builderService"></param>
    /// <exception cref="Exception"></exception>
    public void Build(IServiceCollection services, IServiceProvider builderService) => 
        services.AddSingleton(builderService.GetRequiredService<ICommandManager>());
}

/// <summary>
/// 一个空的参数获取实现类
/// </summary>
internal class NullParam : IGetParam
{
    public ParamAttribute? ParamAttribute { get; set; }

    public Task<object> GetParam(TelegramUserContext context) =>
        Task.FromResult<object>(null!);
    public Task<bool> SendMessage(TelegramUserContext context) =>
        Task.FromResult(true);
}
