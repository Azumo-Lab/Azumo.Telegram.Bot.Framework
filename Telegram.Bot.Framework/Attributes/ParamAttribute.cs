using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Framework.InternalImplementation.Params.ParamMsg;

namespace Telegram.Bot.Framework.Attributes
{
    /// <summary>
    /// 用于捕获参数数据
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class ParamAttribute : Attribute
    {
        public string Message { get; set; } = default!;

        public Type MessageClass { get; set; } = typeof(StringMessage);

        public Type ParamCatchClass { get; set; } = default!;
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ParamClassAttribute : Attribute
    {
        public ParamClassAttribute()
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ParamPropertyAttribute : ParamAttribute
    {
        public ParamPropertyAttribute()
        {

        }
    }
}
