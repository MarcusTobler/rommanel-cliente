using System.Reflection;

namespace DevPack.Core.Enum
{
    public static class EnumerationExtensions
    {
        public static bool IsSmartEnum(this Type? type) =>
            IsEnumeration(type, out var _);

        private static bool IsEnumeration(this Type? type, out Type[]? genericArguments)
        {
            if (type is null || type.IsAbstract || type.IsGenericTypeDefinition)
            {
                genericArguments = null;
                return false;
            }

            do
            {
                if (type.IsGenericType &&
                    type.GetGenericTypeDefinition() == typeof(Enumeration<,>))
                {
                    genericArguments = type.GetGenericArguments();
                    return true;
                }

                type = type.BaseType;
            }
            while (!(type is null));

            genericArguments = null;
            return false;
        }

        public static bool TryGetValues(this Type? type, out IEnumerable<object>? enums)
        {
            while (type != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Enumeration<,>))
                {
                    var listPropertyInfo = type.GetProperty("List", BindingFlags.Public | BindingFlags.Static);
                    enums = (IEnumerable<object>)listPropertyInfo?.GetValue(type, null)!;
                    return true;
                }
                type = type.BaseType;
            }

            enums = null;
            return false;
        }
    }
}