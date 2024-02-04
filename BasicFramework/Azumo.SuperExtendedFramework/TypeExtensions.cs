using System.Linq.Expressions;
using System.Reflection;

namespace Azumo.SuperExtendedFramework;

public static class TypeExtensions
{
    public static List<Type> AllTypes { get; } =
        AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();

    public static List<Type> GetAllSameType(this Type type) =>
        AllTypes.Where(x => type.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();

    public static List<(Type, Attribute)> GetHasAttributeType(this Type type) =>
        AllTypes.Where(x => Attribute.IsDefined(x, type))
            .Select(x => (x, Attribute.GetCustomAttribute(x, type)!))
            .ToList();

    public static List<(MethodInfo, Attribute)> GetAttributeMethods<Attr>(this Type type, BindingFlags bindingFlags) where Attr : Attribute =>
        type.GetMethods(bindingFlags)
            .Where(x => Attribute.IsDefined(x, typeof(Attr)))
            .Select(x => (x, Attribute.GetCustomAttribute(x, typeof(Attr))!))
            .ToList();

    public static Func<object, object[], object> BuildFunc(this MethodInfo methodInfo)
    {
        var parameters = methodInfo.GetParameters();

        var instance = Expression.Parameter(typeof(object), "instance");
        var param = Expression.Parameter(typeof(object[]), "param");

        var paramList = new Expression[parameters.Length];

        for (var i = 0; i < paramList.Length; i++)
            paramList[i] = Expression.Convert(Expression.ArrayIndex(param, Expression.Constant(i)), parameters[i].ParameterType);

        var method = Expression.Call(instance, methodInfo, param);
        Expression func;
        if (methodInfo.ReturnType.FullName == typeof(void).FullName)
        {
            Expression<Func<object>> expression = () => null!;
            func = Expression.Block(method, expression);
        }
        else
        {
            func = Expression.Block(method);
        }
        return Expression.Lambda<Func<object, object[], object>>(func, instance, param).Compile();
    }
}
