using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azumo.Reflection
{
    public class AzAttribute<T> where T : Attribute
    {

        /// <summary>
        /// 
        /// </summary>
        private static readonly List<Type> __AllTypes;

        /// <summary>
        /// 
        /// </summary>
        static AzAttribute()
        {
            __AllTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
        }

        public List<Type> GetAllType()
        {
            return __AllTypes.Where(x =>
            {
                return Attribute.IsDefined(x, typeof(T));
            }).ToList();
        }
    }
}
