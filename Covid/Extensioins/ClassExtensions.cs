using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Covid.Extensioins
{
    public static class ClassExtensions
    {
        #region Extension

        public static NameValueCollection ToNameValueCollection<T>(this T dynamicObject)
        {
            var nameValueCollection = new NameValueCollection();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(dynamicObject))
            {
                var t = TypeDescriptor.GetReflectionType(dynamicObject);
                var pi = t.GetProperty(propertyDescriptor.Name);
                var hasJsonIgnore = Attribute.IsDefined(pi, typeof(JsonIgnoreAttribute));
                if (hasJsonIgnore)
                {
                    continue;
                }
                string value = propertyDescriptor.GetValue(dynamicObject).ToString();
                nameValueCollection.Add(propertyDescriptor.Name, value);
            }
            return nameValueCollection;
        }

        public static NameValueCollection ToJsonPropertyNameValueCollection<T>(this T dynamicObject)
        {
            var nameValueCollection = new NameValueCollection();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(dynamicObject))
            {
                var t = TypeDescriptor.GetReflectionType(dynamicObject);
                var pi = t.GetProperty(propertyDescriptor.Name);
                var hasJsonIgnore = Attribute.IsDefined(pi, typeof(JsonIgnoreAttribute));
                if (hasJsonIgnore)
                {
                    continue;
                }
                string value = propertyDescriptor.GetValue(dynamicObject).ToString();
                var customAttribute = (JsonPropertyAttribute)pi.GetCustomAttribute(typeof(JsonPropertyAttribute), false);
                nameValueCollection.Add(customAttribute.PropertyName, value);
            }
            return nameValueCollection;
        }

        public static string ToParameterString<T>(this T parameters) where T : class
        {
            var properties = parameters.GetType()
                .GetProperties()
                .Where(p => p.GetValue(parameters, null) != null && p.GetCustomAttributes(typeof(JsonIgnoreAttribute), false).Length == 0)
                .Select(p => p.Name.ToLower() + "=" + HttpUtility.UrlEncode(GetParameterValue(parameters, p)));

            return string.Join("&", properties.ToArray());
        }

        public static Dictionary<string, string> ToParameterDictionary<T>(this T parameters) where T : class
        {
            return parameters.ToParameterDictionary(false);
        }

        public static Dictionary<string, string> ToParameterDictionary<T>(this T parameters, bool isCamelCase) where T : class
        {
            var enumerable = parameters.GetType()
                .GetProperties()
                .Where(p => p.GetValue(parameters, null) != null &&
                            p.GetCustomAttributes(typeof(JsonIgnoreAttribute), false).Length == 0);
            var dictionary = new Dictionary<string, string>();
            foreach (var p in enumerable)
            {
                dictionary.Add(isCamelCase ? (char.ToLowerInvariant(p.Name[0]) + p.Name.Substring(1)) : p.Name,
                    GetParameterValue(parameters, p).ToString());
            }
            return dictionary;
        }

        public static bool IsParameterNotNull(this object instance)
        {
            // Or false, or throw an exception
            if (Object.ReferenceEquals(null, instance))
                return true;

            //TODO: elaborate - do you need public as well as non public properties? Static ones?
            var properties = instance.GetType().GetProperties(
                BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.Public |
                BindingFlags.NonPublic);

            foreach (var prop in properties)
            {
                if (!prop.CanRead) // <- exotic write-only properties
                    continue;
                else if (prop.PropertyType.IsValueType) // value type can't be null
                    continue;

                Object value = prop.GetValue(prop.GetGetMethod().IsStatic ? null : instance);

                if (Object.ReferenceEquals(null, value))
                    return false;

                //TODO: if you don't need check STRING properties for being empty, comment out this fragment
                String str = value as String;

                if (null != str)
                    if (str.Equals(""))
                        return false;
            }

            return true;
        }

        #endregion Extension

        #region Private Method

        private static string GetParameterValue<T>(T parameters, PropertyInfo property) where T : class
        {
            var value = property.GetValue(parameters, null);
            var result = value.ToString();

            return result;
        }

        #endregion Private Method
    }
}
