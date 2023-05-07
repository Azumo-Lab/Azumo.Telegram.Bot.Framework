using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.Abstracts.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ParamMakerTypeAttribute : Attribute
    {
        public Type ParamType { get; }
        public ParamMakerTypeAttribute(Type paramType) 
        {
            ParamType = paramType;
        }
    }
}
