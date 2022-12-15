using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Covid.Exceptions;

namespace Covid.Helper
{
    public static class EnumHelper
    {
        public static T ToEnum<T>(this string value)
        {
            try
            {
                var names = System.Enum.GetNames(typeof(T));
                if (names.Select(x => x.ToLower()).ToList().All(x => x != value.ToLower()))
                {
                    throw new InvalidCastException();
                }

                var result = (T)System.Enum.Parse(typeof(T), value, true);
                return result;
            }
            catch (Exception e)
            {
                throw new ApiException(e.Message, Enums.EnumError.GeneralError);
            }
        }

        public static string GetEnumDescription(System.Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        public static T ToEnumWithDefault<T>(this string value, T defaultData)
        {
            try
            {
                var result = defaultData;
                var names = System.Enum.GetNames(typeof(T));
                if (names.Select(x => x.ToLower()).ToList().Any(x => x == value.ToLower()))
                {
                    result = (T)System.Enum.Parse(typeof(T), value, true);
                }

                return result;
            }
            catch (Exception e)
            {
                throw new ApiException(e.Message, Enums.EnumError.GeneralError);
            }
        }

        public static string Description<T>(this T source)
        {
            var field = source.GetType().GetField(source.ToString());
            if (field == null)
            {
                return source.ToString();
            }
            var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : source.ToString();
        }

        public static T ParseToEnumValue<T>(this string str) where T : struct
        {
            return System.Enum.TryParse<T>(str, true, out var enumValue)
                ? enumValue
                : throw new InvalidEnumArgumentException();
        }
    }
}