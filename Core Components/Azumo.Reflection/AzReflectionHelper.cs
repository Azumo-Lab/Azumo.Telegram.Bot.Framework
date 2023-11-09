using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azumo.Reflection
{
    public static class AzReflectionHelper
    {
        private static readonly List<Type> __AllTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
        public static List<Type> GetAllTypes()
        {
            return __AllTypes;
        }
    }
}
