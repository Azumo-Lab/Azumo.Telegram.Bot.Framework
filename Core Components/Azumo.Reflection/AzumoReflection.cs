using Azumo.Reflection.MethodReflections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Azumo.Reflection
{
    public class AzumoReflection<T> : AzumoTypeDescriptorBase
    {
        public AzumoReflection() : base(typeof(T))
        {
        }

        public Type Type { get; } = typeof(T);

        public static AzumoReflection<T> Reflection() => new();

        public AzumoMethodReflectionInfo[] GetMethods(BindingFlags bindingFlags) => 
            Type.GetMethods(bindingFlags)
                .Select(x => new AzumoMethodReflectionInfo(x))
                .ToArray();
    }

    public class AzumoReflection
    {
        public static List<Type> AllTypes { get; } = 
            AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
    }
}
