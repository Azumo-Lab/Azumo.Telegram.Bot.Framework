using System.Reflection;

namespace Telegram.Bot.Framework.Reflections.FrameworkInstall
{
    public static class GetMethod
    {
        public static List<MethodInfo> GetMethodInfos(this object baseType, Func<MethodInfo, bool> whereFilter)
        {
            List<MethodInfo> methodInfos = new();
            foreach (Type type in baseType.FindTypeOf())
                methodInfos.AddRange(type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default | BindingFlags.Static));

            return methodInfos.Where(whereFilter).ToList();
        }

        public static List<MethodInfo> GetMethodInfos<Attr>(this object baseType) where Attr : Attribute
        {
            return GetMethodInfos(baseType, x => Attribute.IsDefined(x, typeof(Attr)));
        }

        public static List<ParameterInfo> GetParameterInfos(this MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            return parameters.ToList();
        }

        public static List<Attr> GetAttribute<Attr>(this MethodInfo MethodInfo) where Attr : Attribute
        {
            return Attribute.GetCustomAttributes(MethodInfo, typeof(Attr)).Select(x => (Attr)x).ToList();
        }
    }
}
