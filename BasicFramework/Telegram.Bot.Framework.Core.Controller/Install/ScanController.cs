using Azumo.ReflectionEnhancementPack;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.BotBuilder;
using Telegram.Bot.Framework.Core.Controller.Controller;
using Telegram.Bot.Framework.Core.Users;

namespace Telegram.Bot.Framework.Core.Controller.Install;
internal class ScanController : ITelegramModule
{
    public void AddBuildService(IServiceCollection services)
    {

    }
    public void Build(IServiceCollection services, IServiceProvider builderService)
    {
        // 获取参数实现类
        var getparamTypeList = typeof(IGetParam).GetAllSameType()
            .Where(x => Attribute.IsDefined(x, typeof(TypeForAttribute)))
            .Select(x => (x, (TypeForAttribute)Attribute.GetCustomAttribute(x, typeof(TypeForAttribute))!))
            .ToList();

        var controllerTypeList = typeof(TelegramControllerAttribute).GetHasAttributeType();
        foreach ((var controller, var _) in controllerTypeList)
        {
            foreach ((var method, var attr) in controller.GetAttributeMethods<BotCommandAttribute>(BindingFlags.Public | BindingFlags.Instance))
            {

                var parameters = method.GetParameters().Select(x =>
                {
                    var paramval = getparamTypeList
                        .Where(y => y.Item2.ForType.FullName == x.ParameterType.FullName)
                        .Select(y => y.x)
                        .FirstOrDefault() ?? typeof(NullParam);

                    var result = Activator.CreateInstance(paramval) ?? throw new Exception();
                    return (IGetParam)result;
                });
            }
        }
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
