// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-19 15:23:33
// ========================================================

using System;
using UnityEngine;

namespace LF
{
    public static partial class Utility
    {
        /// <summary>
        /// JSON 相关的实用函数。
        /// </summary>
        public static class Json
        {
            /// <summary>
            /// 将对象序列化为 JSON 字符串。
            /// </summary>
            /// <param name="obj">要序列化的对象。</param>
            /// <returns>序列化后的 JSON 字符串。</returns>
            public static string ToJson(object obj)
            {
                try
                {
                    return JsonUtility.ToJson(obj);
                }
                catch (Exception exception)
                {
                    throw new LufyException(Text.Format("Can not convert to JSON with exception '{0}'.", exception.ToString()), exception);
                }
            }

            /// <summary>
            /// 将对象序列化为 JSON 流。
            /// </summary>
            /// <param name="obj">要序列化的对象。</param>
            /// <returns>序列化后的 JSON 流。</returns>
            public static byte[] ToJsonData(object obj)
            {
                return Converter.GetBytes(ToJson(obj));
            }

            /// <summary>
            /// 将 JSON 字符串反序列化为对象。
            /// </summary>
            /// <typeparam name="T">对象类型。</typeparam>
            /// <param name="json">要反序列化的 JSON 字符串。</param>
            /// <returns>反序列化后的对象。</returns>
            public static T ToObject<T>(string json)
            {
                try
                {
                    return JsonUtility.FromJson<T>(json);
                }
                catch (Exception exception)
                {
                    throw new LufyException(Text.Format("Can not convert to object with exception '{0}'.", exception.ToString()), exception);
                }
            }

            /// <summary>
            /// 将 JSON 字符串反序列化为对象。
            /// </summary>
            /// <param name="objectType">对象类型。</param>
            /// <param name="json">要反序列化的 JSON 字符串。</param>
            /// <returns>反序列化后的对象。</returns>
            public static object ToObject(Type objectType, string json)
            {
                if (objectType == null)
                {
                    throw new LufyException("Object type is invalid.");
                }

                try
                {
                    return JsonUtility.FromJson(json, objectType);
                }
                catch (Exception exception)
                {
                    throw new LufyException(Text.Format("Can not convert to object with exception '{0}'.", exception.ToString()), exception);
                }
            }

            /// <summary>
            /// 将 JSON 流反序列化为对象。
            /// </summary>
            /// <typeparam name="T">对象类型。</typeparam>
            /// <param name="jsonData">要反序列化的 JSON 流。</param>
            /// <returns>反序列化后的对象。</returns>
            public static T ToObject<T>(byte[] jsonData)
            {
                return ToObject<T>(Converter.GetString(jsonData));
            }

            /// <summary>
            /// 将 JSON 字符串反序列化为对象。
            /// </summary>
            /// <param name="objectType">对象类型。</param>
            /// <param name="jsonData">要反序列化的 JSON 流。</param>
            /// <returns>反序列化后的对象。</returns>
            public static object ToObject(Type objectType, byte[] jsonData)
            {
                return ToObject(objectType, Converter.GetString(jsonData));
            }
        }
    }
}
