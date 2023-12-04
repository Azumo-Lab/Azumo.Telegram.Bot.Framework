using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Azumo.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class List_ExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static List<object> ToObjectList(this IEnumerable values)
        {
            return values.Cast<object>().ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static List<object> ToObjectList(this IEnumerator values)
        {
            List<object> list = [];
            while (values.MoveNext())
                list.Add(values.Current);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this IEnumerable values)
        {
            return values.Cast<T>().ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this IEnumerator values)
        {
            List<T> list = [];
            try
            {
                while (values.MoveNext())
                    list.Add((T)values.Current);
            }
            catch (Exception)
            { }
            return list;
        }
    }
}
