using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azumo.Reflection
{
    public class AzumoTypeDescriptorBase
    {
        private readonly object __Type;

        protected AzumoTypeDescriptorBase(object type) => __Type = type;

        public void SetAttributes(Attribute[] attributes) =>
                _ = TypeDescriptor.AddAttributes(__Type, attributes);

        public Attribute[] GetAttributes() =>
            TypeDescriptor.GetAttributes(__Type).Cast<Attribute>().ToArray();

        public TAttribute? GetAttribute<TAttribute>() where TAttribute : Attribute =>
            TypeDescriptor.GetAttributes(__Type).Cast<Attribute>().FirstOrDefault(x => x.GetType().FullName == typeof(TAttribute).FullName) as TAttribute;
    }
}
