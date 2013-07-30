using System;
using System.Globalization;

namespace SquarePegRoundHole.Dynamic
{
    public static class Coerce
    {
        private static readonly ICoercion[] _coercions = {
                new ParseCoercion(),
                new ConvertCoercion(), 
                new ActivatorCoercion(), 
                new CodeCoercion(), 
            };

        private static class Holder<T>
        {
            public static readonly MultiCoercer<T> Instance = new MultiCoercer<T>(_coercions);
        }

        public static bool TryTo<T>(string input, out T output)
        {
            return Holder<T>.Instance.TryFromString(input, out output, CultureInfo.CurrentCulture);
        }

        public static bool TryTo<T>(string input, out T output, IFormatProvider formatProvider)
        {
            return Holder<T>.Instance.TryFromString(input, out output, formatProvider);
        }

        public static T To<T>(string input)
        {
            return Holder<T>.Instance.FromString(input, CultureInfo.CurrentCulture);
        }

        public static T To<T>(string input, IFormatProvider formatProvider)
        {
            return Holder<T>.Instance.FromString(input, formatProvider);
        }
    }
}
