using System;
using System.ComponentModel;
using System.Linq;

namespace GitHookProcessor.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T value) where T : Enum
        {
            var descriptions = GetAttributes<T, DescriptionAttribute>(value);
            if (descriptions.Any()) return descriptions.First().Description;

            throw new Exception($"Unable to find {typeof(T).Name} enum description");
        }

        private static TAttribute[] GetAttributes<TEnum, TAttribute>(TEnum enumValue) where TEnum : Enum
        {
            var enumType = typeof(TEnum);
            var memberInfos = enumType.GetMember(enumValue.ToString());
            var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);
            var valueAttributes = enumValueMemberInfo?.GetCustomAttributes(typeof(TAttribute), false);
            if (valueAttributes == null || valueAttributes.Length == 0) return Array.Empty<TAttribute>();

            return valueAttributes.OfType<TAttribute>().ToArray();
        }
    }
}