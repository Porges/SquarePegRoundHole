using System;
using System.Globalization;

namespace SquarePegRoundHole
{
    public static class CoerceExtensions
    {
        public static T FromString<T>(this ICoercer<T> me, string input)
        {
            return FromString(me, input, CultureInfo.CurrentCulture);
        }

        public static bool TryFromString<T>(this ICoercer<T> me, string input, out T output)
        {
            return TryFromString(me, input, out output, CultureInfo.CurrentCulture);
        }

        public static bool TryFromString<T>(this ICoercer<T> me, string input, out T output, IFormatProvider formatProvider)
        {
            switch (me.AttemptCoercion(input, out output, formatProvider))
            {
                case CoercionResult.Success:
                    return true;

                default:
                    return false;
            }
        }

        public static T FromString<T>(this ICoercer<T> me, string input, IFormatProvider formatProvider)
        {
            T result;
            if (!me.TryFromString(input, out result, formatProvider))
            {
                throw new InvalidCastException(string.Format("Cannot convert the string '{0}' to the type '{1}'.", input, typeof(T)));
            }
            return result;
        }
    }
}       