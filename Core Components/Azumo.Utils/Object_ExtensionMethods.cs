using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Azumo.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class Object_ExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="target"></param>
        public static void CopyTo<T>(this T t, T target) where T : class
        {
            Type tType = typeof(T);
            if(!StaticCache<Type, PropertyInfo[]>.GetCache(tType, out PropertyInfo[] value))
            {
                value = tType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                StaticCache<Type, PropertyInfo[]>.SetCache(tType, value);
            }

            foreach(PropertyInfo p in value)
                p.SetValue(target, p.GetValue(t));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T Copy<T>(this T t) where T : class, new()
        {
            T newT = new T();
            Type tType = typeof(T);
            if (!StaticCache<Type, PropertyInfo[]>.GetCache(tType, out PropertyInfo[] value))
            {
                value = tType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                StaticCache<Type, PropertyInfo[]>.SetCache(tType, value);
            }

            foreach (PropertyInfo p in value)
                p.SetValue(newT, p.GetValue(t));

            return newT;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="target"></param>
        public static void DeepCopyTo<T>(this T t, T target) where T : class, new()
        {
            DeepCopy(t).CopyTo(target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(this T t) where T : class, new()
        {
            return (T)DeepCopy(t, t.GetType());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DeepCopy(this object t, Type type)
        {
            if (!StaticCache<Type, PropertyInfo[]>.GetCache(type, out PropertyInfo[] value))
            {
                value = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                StaticCache<Type, PropertyInfo[]>.SetCache(type, value);
            }

            object newT = Activator.CreateInstance(type);

            foreach (PropertyInfo p in value)
                if (Type.GetTypeCode(p.PropertyType) == TypeCode.Object)
                    p.SetValue(newT, p.GetValue(t).DeepCopy(p.PropertyType));
                else
                    p.SetValue(newT, p.GetValue(t));

            return newT;
        }
    }
}
