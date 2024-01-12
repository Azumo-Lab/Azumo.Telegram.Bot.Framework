using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azumo.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public class AzumoTypeDescriptorBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly object __Type;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        protected AzumoTypeDescriptorBase(object type) => __Type = type;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributes"></param>
        public void SetAttributes(params Attribute[] attributes) =>
            _ = TypeDescriptor.AddAttributes(__Type, attributes);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Attribute[] GetAttributes() =>
            TypeDescriptor.GetAttributes(__Type).Cast<Attribute>().ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public TAttribute[] GetAttributes<TAttribute>() where TAttribute : Attribute =>
            TypeDescriptor.GetAttributes(__Type).Cast<Attribute>().Where(x => x is TAttribute).Select(x => (TAttribute)x).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public TAttribute? GetAttribute<TAttribute>() where TAttribute : Attribute =>
            GetAttributes<TAttribute>().FirstOrDefault();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public TAttribute? GetAttribute<TAttribute>(Func<TAttribute, bool> filter) where TAttribute : Attribute =>
            GetAttributes<TAttribute>().FirstOrDefault(filter);
    }
}
