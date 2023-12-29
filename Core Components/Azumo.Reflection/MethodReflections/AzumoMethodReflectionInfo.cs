using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Azumo.Reflection.MethodReflections
{
    public class AzumoMethodReflectionInfo(MethodInfo methodInfo)
    {
        public MethodInfo MethodInfo { get; } = methodInfo;

        public Func<object[], Task> CreateFunc()
        {
            Func<object[], Task> func = async (paramObjs) => await Task.CompletedTask;
            if (func != null)
            {
                return func;
            }
            Console.WriteLine();
            return null!;
        }
    }
}
