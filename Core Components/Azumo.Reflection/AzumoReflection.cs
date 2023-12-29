using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Azumo.Reflection
{
    public class AzumoReflection<T>
    {
        public Type Type { get; } = typeof(T);

        public static AzumoReflection<T> Reflection() => 
            new();

        public void SetAttributes(Attribute[] attributes) => 
            _ = TypeDescriptor.AddAttributes(Type, attributes);

        public Attribute[] GetAttributes() =>
            TypeDescriptor.GetAttributes(Type).Cast<Attribute>().ToArray();

        public TAttribute? GetAttribute<TAttribute>() where TAttribute : Attribute => 
            TypeDescriptor.GetAttributes(Type).Cast<Attribute>().FirstOrDefault(x => x.GetType().FullName == typeof(TAttribute).FullName) as TAttribute;
    }

    public class AzumoReflection
    {
        public static List<Type> AllTypes { get; } = 
            AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
    }
}
