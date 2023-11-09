using Azumo.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Abstracts.InternalInterface
{
    [DependencyInjection(ServiceLifetime.Singleton, typeof(IControllerParamMaker))]
    internal class ControllerParamMaker : IControllerParamMaker
    {
        private readonly Dictionary<Type, Type> __AllType = new();
        private readonly IServiceProvider ServiceProvider;
        public ControllerParamMaker(IServiceProvider serviceProvider) 
        {
            ServiceProvider = serviceProvider;
            AzAttribute<TypeForAttribute> azAttribute = new AzAttribute<TypeForAttribute>();
            foreach (Type item in azAttribute.GetAllType())
            {
                TypeForAttribute typeForAttribute = (TypeForAttribute)Attribute.GetCustomAttribute(item, typeof(TypeForAttribute))!;
                __AllType.TryAdd(typeForAttribute.Type, item);
            }
        }
        public IControllerParam Make(Type paramType)
        {
            if(__AllType.TryGetValue(paramType, out Type? IControllerParamType))
            {
                IControllerParamType ??= typeof(NullControllerParam);
                return (IControllerParam)ActivatorUtilities.CreateInstance(ServiceProvider, IControllerParamType, Array.Empty<object>());
            }
            return new NullControllerParam();
        }

        private class NullControllerParam : IControllerParam
        {
            public Task<object> CatchObjs(TGChat tGChat)
            {
                return Task.FromResult<object>(null!);
            }

            public Task SendMessage()
            {
                return Task.CompletedTask;
            }
        }
    }
}
