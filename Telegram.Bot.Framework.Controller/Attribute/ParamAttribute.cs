using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.Controller.Attribute
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class ParamAttribute : System.Attribute
    {
        public string Message { get; set; } = default!;

        public Type MessageClass { get; set; } = default!;

        public Type ParamCatchClass { get; set; } = default!;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ParamClassAttribute : System.Attribute
    {
        public ParamClassAttribute()
        {

        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ParamPropertyAttribute : ParamAttribute
    {
        public ParamPropertyAttribute()
        {

        }
    }
}
