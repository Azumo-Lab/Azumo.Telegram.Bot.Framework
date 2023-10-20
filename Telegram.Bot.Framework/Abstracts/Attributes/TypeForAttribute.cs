using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstracts.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TypeForAttribute : Attribute
    {
        public Type Type { get; }
        public TypeForAttribute(Type type)
        {
            Type = type;
        }
    }
}
