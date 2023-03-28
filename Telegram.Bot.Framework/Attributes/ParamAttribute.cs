using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.Controller.Attribute
{
    /// <summary>
    /// 用于捕获参数数据
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class ParamAttribute : System.Attribute
    {
        public string Message { get; set; } = default!;

        public Type MessageClass { get; set; } = default!;

        public Type ParamCatchClass { get; set; } = default!;
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ParamClassAttribute : System.Attribute
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
