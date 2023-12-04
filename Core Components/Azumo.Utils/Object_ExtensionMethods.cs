using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Azumo.Utils
{
    /// <summary>
    /// 对象扩展方法工具
    /// </summary>
    /// <remarks>
    /// <see cref="object"/> 的扩展方法
    /// </remarks>
    public static class Object_ExtensionMethods
    {
        /// <summary>
        /// 将对象的数据复制到另一个对象实例中
        /// </summary>
        /// <remarks>
        /// 本方法，仅仅解析例如下面这样的数值复制
        /// <code>
        /// public string Text { get; set; }
        /// </code>
        /// 公开方法，且可实例化，如果有更多的要求，请使用其他方法
        /// </remarks>
        /// <typeparam name="T">进行复制的对象</typeparam>
        /// <param name="t">复制元对象</param>
        /// <param name="target">复制目标对象</param>
        public static void CopyTo<T>(this T t, T target) where T : class
        {
            Type tType = typeof(T);
            PropertyInfo[] value = StaticCache<string, PropertyInfo[]>.GetCache(tType.FullName!, () => tType.GetProperties(BindingFlags.Instance | BindingFlags.Public));

            foreach (PropertyInfo p in value)
                p.SetValue(target, p.GetValue(t));
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <typeparam name="T">复制对象的类型</typeparam>
        /// <param name="t">复制对象的实例</param>
        /// <returns>返回复制的对象</returns>
        public static T? Copy<T>(this T t) where T : class
        {
            Type tType = typeof(T);

            object? newT;
            try
            {
                newT = Activator.CreateInstance(tType);

                PropertyInfo[] value = StaticCache<string, PropertyInfo[]>.GetCache(tType.FullName!, () => tType.GetProperties(BindingFlags.Instance | BindingFlags.Public));

                foreach (PropertyInfo p in value)
                    p.SetValue(newT, p.GetValue(t));
            }
            catch (Exception)
            {
                throw;
            }

            return newT as T;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="target"></param>
        /// <param name="copyOption"></param>
        /// <param name="bindingFlags"></param>
        public static void DeepCopyTo<T>(this T t, T target, CopyOption copyOption = CopyOption.Default, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public) where T : class
        {
            object? result = DeepCopy(t, copyOption, bindingFlags);
            if (result == null)
                return;
            result.CopyTo(target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="copyOption"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static T? DeepCopy<T>(this T t, CopyOption copyOption = CopyOption.Default, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public) where T : class
        {
            object? result = DeepCopy(t, typeof(T), copyOption, bindingFlags);
            return result == null ? default : result as T;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="type"></param>
        /// <param name="copyOption"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static object? DeepCopy(this object t, Type type, CopyOption copyOption, BindingFlags bindingFlags)
        {
            object? newT = null;
            try
            {
                newT = Activator.CreateInstance(type);
            }
            catch (Exception)
            {
                return newT;
            }

            List<FieldInfo> fields = [];
            List<PropertyInfo> properties = [];

            string fieldInfoKey = $"{type.FullName}.{nameof(fieldInfoKey)}";
            string propertyInfoKey = $"{type.FullName}.{nameof(propertyInfoKey)}";

            switch (copyOption)
            {
                case CopyOption.Fields:
                    GetFieldInfo(fieldInfoKey, out fields, type, bindingFlags);
                    break;
                case CopyOption.Fields | CopyOption.Properties:
                    GetFieldInfo(fieldInfoKey, out fields, type, bindingFlags);
                    GetPropertyInfo(propertyInfoKey, out properties, type, bindingFlags);
                    break;
                default:
                    GetPropertyInfo(propertyInfoKey, out properties, type, bindingFlags);
                    break;
            }

            foreach (PropertyInfo p in properties ?? [])
                if (Type.GetTypeCode(p.PropertyType) == TypeCode.Object)
                    p.SetValue(newT, p.GetValue(t)?.DeepCopy(p.PropertyType, copyOption, bindingFlags));
                else
                    p.SetValue(newT, p.GetValue(t));

            foreach (FieldInfo f in fields ?? [])
                if (Type.GetTypeCode(f.FieldType) == TypeCode.Object)
                    f.SetValue(newT, f.GetValue(t)?.DeepCopy(f.FieldType, copyOption, bindingFlags));
                else
                    f.SetValue(newT, f.GetValue(t));

            return newT;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="KeyType"></typeparam>
        /// <param name="keyType"></param>
        /// <param name="valueType"></param>
        /// <param name="type"></param>
        /// <param name="bindingFlags"></param>
        private static void GetFieldInfo<KeyType>(KeyType keyType, out List<FieldInfo> valueType, Type type, BindingFlags bindingFlags) where KeyType : notnull
        {
            valueType = StaticCache<KeyType, List<FieldInfo>>.GetCache(keyType, () => type.GetFields(bindingFlags).ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="KeyType"></typeparam>
        /// <param name="keyType"></param>
        /// <param name="valueType"></param>
        /// <param name="type"></param>
        /// <param name="bindingFlags"></param>
        private static void GetPropertyInfo<KeyType>(KeyType keyType, out List<PropertyInfo> valueType, Type type, BindingFlags bindingFlags) where KeyType : notnull
        {
            valueType = StaticCache<KeyType, List<PropertyInfo>>.GetCache(keyType, () => type.GetProperties(bindingFlags).ToList());
        }
    }

    /// <summary>
    /// 复制选项
    /// </summary>
    public enum CopyOption
    {
        /// <summary>
        /// 默认的选项
        /// </summary>
        Default = 1,

        /// <summary>
        /// 属性
        /// </summary>
        Properties = 2,

        /// <summary>
        /// 字段
        /// </summary>
        Fields = 4,
    }
}
