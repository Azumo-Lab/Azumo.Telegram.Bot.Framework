using Azumo.ReflectionEnhancementPack;
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    public void AddBuildService(IServiceCollection services)
    {
        services.AddSingleton<ICommandManager, CommandManager>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builderService"></param>
    /// <exception cref="Exception"></exception>
    public void Build(IServiceCollection services, IServiceProvider builderService)
    {
        var i18n = builderService.GetService<II18NManager>();

        // 获取参数实现类
        var getparamTypeList = typeof(IGetParam).GetAllSameType()
            .Where(x => Attribute.IsDefined(x, typeof(TypeForAttribute)))
            .Select(x => (x, (TypeForAttribute)Attribute.GetCustomAttribute(x, typeof(TypeForAttribute))!))
            .ToList();

        var commandManager = builderService.GetRequiredService<ICommandManager>();

        var controllerTypeList = typeof(TelegramControllerAttribute).GetHasAttributeType();
        foreach ((var controller, var _) in controllerTypeList)
        {
            foreach ((var method, var attr) in controller.GetAttributeMethods<BotCommandAttribute>(BindingFlags.Public | BindingFlags.Instance))
            {
                var executor = Factory.GetExecutorInstance(EnumCommandType.BotCommand,
                    ActivatorUtilities.CreateFactory(controller, []),
                    method.BuildFunc(),
                    method.GetParameters().Select(x =>
                    {
                        var paramval = getparamTypeList
                            .Where(y => y.Item2.ForType.FullName == x.ParameterType.FullName)
                            .Select(y => y.x)
                            .FirstOrDefault() ?? typeof(NullParam);

                        ConstructorInfo? constructorInfo;
                        if ((constructorInfo = paramval.GetConstructors().OrderBy(x => x.GetParameters().Length).FirstOrDefault()) == null)
                            throw new Exception(i18n?.Current[""] ?? "没有找到对应的初始化方法");

                        if (constructorInfo.GetParameters().Length != 0)
                            throw new Exception(i18n?.Current[""] ?? "无法生成带有参数的类");

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
                    new List<Attribute> { attr });
                commandManager.AddExecutor(executor);
            }
        }

        services.AddSingleton<ICommandManager>(builderService.GetRequiredService<ICommandManager>());
    }
}

internal class NullParam : IGetParam
{
    public ParamAttribute? ParamAttribute { get; set; }

    public Task<object> GetParam(TelegramUserContext context) =>
        Task.FromResult<object>(null!);
    public Task SendMessage(TelegramUserContext context) =>
        Task.CompletedTask;
}
