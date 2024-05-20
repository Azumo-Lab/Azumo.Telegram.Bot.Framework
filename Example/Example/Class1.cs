using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Example
{
    internal static class Class1
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static Func<object, object[], Task<object?>> BuildTaskFunc(this MethodInfo methodInfo)
        {
            // 创建对象创建工厂
            var instanceType = methodInfo.DeclaringType;
            var isStatic = methodInfo.IsStatic;

            if (!isStatic && instanceType == null)
                throw new InvalidOperationException("Cannot create factory for static method with null declaring type.");

            // 参数
            var instance = Expression.Parameter(typeof(object), "instance");
            var param = Expression.Parameter(typeof(object[]), "param");
            var paramList = methodInfo.GetParameters().BuildParamters(param);

            // 调用方法
            RuntimeHelpers.PrepareMethod(methodInfo.MethodHandle);

            var method = Expression.Call(instance, methodInfo, paramList);
            Expression func;
            if (methodInfo.ReturnType.FullName == typeof(void).FullName)
            {
                Expression<Func<object?>> expression = () => null;
                func = Expression.Block(method, expression);
            }
            else
            {
                var resultType = methodInfo.ReturnType;
                if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(Task<>))
                {
                    var result11 = Expression.Convert(method, typeof(Task<object?>));
                    func = Expression.Block(result11);
                }
                else
                    func = Expression.Block(method);
            }

            var result = Expression.Lambda<Func<object, object[], Task<object?>>>(func, null, param).Compile();
            RuntimeHelpers.PrepareDelegate(result);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterInfos"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static Expression[] BuildParamters(this ParameterInfo[] parameterInfos, ParameterExpression parameter)
        {
            var paramList = new Expression[parameterInfos.Length];
            for (var i = 0; i < paramList.Length; i++)
                paramList[i] = Expression.Convert(Expression.ArrayIndex(parameter, Expression.Constant(i)), parameterInfos[i].ParameterType);
            return paramList;
        }
    }
}
