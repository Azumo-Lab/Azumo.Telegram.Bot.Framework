using Azumo.SuperExtendedFramework;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.BotBuilder;
using Telegram.Bot.Framework.Core.Controller.Controller;
using Telegram.Bot.Framework.Core.Controller.Install;

namespace Telegram.Bot.Framework.Core.Controller.BotBuilder;
internal class TelegramCommand(Delegate func) : ITelegramModule
{
    private readonly Delegate _func = func;

    private readonly static List<(Type x, TypeForAttribute)> getparamTypeList =
        typeof(IGetParam).GetAllSameType()
            .Where(x => Attribute.IsDefined(x, typeof(TypeForAttribute)))
            .Select(x => (x, (TypeForAttribute)Attribute.GetCustomAttribute(x, typeof(TypeForAttribute))!))
            .ToList();

    public void AddBuildService(IServiceCollection services)
    {

    }
    public void Build(IServiceCollection services, IServiceProvider builderService)
    {
        var manager = builderService.GetRequiredService<ICommandManager>();
        var attr = Attribute.GetCustomAttribute(_func.Method, typeof(BotCommandAttribute)) ?? throw new NullReferenceException();

        var exec = Factory.GetExecutorInstance(EnumCommandType.Func,
            _func,
            _func.Method.GetParameters().Select(x =>
            {
                var paramval = getparamTypeList
                    .Where(y => y.Item2.ForType.FullName == x.ParameterType.FullName)
                    .Select(y => y.x)
                    .FirstOrDefault() ?? typeof(NullParam);

                ConstructorInfo? constructorInfo;
                if ((constructorInfo = paramval.GetConstructors().OrderBy(x => x.GetParameters().Length).FirstOrDefault()) == null)
                    throw new Exception("没有找到对应的初始化方法");

                if (constructorInfo.GetParameters().Length != 0)
                    throw new Exception("无法生成带有参数的类");

                try
                {
                    var result = constructorInfo.Invoke([]);
                    return (IGetParam)result;
                }
                catch (Exception)
                {
                    throw;
                }
            }),
            Attribute.GetCustomAttributes(_func.Method));

        manager.AddExecutor(exec);
    }
}

public static class TelegramCommandExtensions
{
    public static ITelegramModuleBuilder AddCommand(this ITelegramModuleBuilder builder, Delegate func) =>
        builder.AddModule<TelegramCommand>(func);
}
