using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Params;
using Telegram.Bot.Framework.Attributes;

namespace Telegram.Bot.Framework.InternalImplementation.Params
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    internal class ParamManager : IParamManager
    {
        private string Command;
        private readonly List<object> Params = new List<object>();
        internal static readonly Dictionary<Type, Type> __ParamType_MakerType = new Dictionary<Type, Type>();

        public bool MakeMode { get; set; }
        public int ParamIndex { get; set; }

        public void AddParam(object param)
        {
            Params.Add(param);
        }

        public void Clear()
        {
            Params.Clear();
            MakeMode = false;
            ParamIndex = 0;
            Command = null;
        }

        public string GetCommand()
        {
            return Command;
        }

        public object[] GetParams()
        {
            return Params.ToArray();
        }

        public void SetCommand(string Command)
        {
            this.Command = Command;
        }

        public IParamMaker GetMaker(IServiceProvider serviceProvider, Type type)
        {
            IParamMaker paramMaker = null;
            if (__ParamType_MakerType.TryGetValue(type, out Type makerType))
                paramMaker = (IParamMaker)ActivatorUtilities.CreateInstance(serviceProvider, makerType, Array.Empty<object>());
            return paramMaker;
        }
    }
}
