using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Azumo.Reflection.MethodReflections
{
    public class AzumoMethodReflectionInfo : AzumoTypeDescriptorBase
    {
        public AzumoMethodReflectionInfo(MethodInfo methodInfo) : base(methodInfo) => 
            MethodInfo = methodInfo;

        public AzumoMethodReflectionInfo(Func<object[], object> func) : base(func.Method) 
        { 
            MethodInfo = func.Method;
            Func = func;
        }

        public Func<object[], object?>? Func { get; private set; }

        public MethodInfo MethodInfo { get; }

        public Func<object[], object?> CreateFunc(object obj)
        {
            if (Func != null)
                return Func;

            object? func(object[] objs) => MethodInfo.Invoke(obj, objs);
            Func = func;
            return Func;
        }
    }
}
