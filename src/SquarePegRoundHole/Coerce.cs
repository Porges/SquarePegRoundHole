using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace SquarePegRoundHole
{
    public class MultiCoercer<T> : ICoercer<T>
    {
        private readonly ReadOnlyCollection<ICoercer<T>> _coercers;
        
        public MultiCoercer(IEnumerable<ICoercion> validCoercions)
        {
            _coercers = new ReadOnlyCollection<ICoercer<T>>(validCoercions.Select(x => x.GetCoercer<T>()).Where(x => x != null).ToArray());
        }

        public MultiCoercer(params ICoercion[] validCoercions) : this((IEnumerable<ICoercion>)validCoercions)
        {}

        public CoercionResult AttemptCoercion(string input, out T output, IFormatProvider formatProvider)
        {
            foreach (var coercer in _coercers)
            {
                switch (coercer.AttemptCoercion(input, out output, formatProvider))
                {
                    case CoercionResult.Success:
                        return CoercionResult.Success;

                    case CoercionResult.Malformed:
                        return CoercionResult.Malformed;

                    case CoercionResult.Failure:
                        continue;
                }
            }

            output = default(T);
            return CoercionResult.Failure;
        }
    }

    public static class Coerce
    {
        private static readonly ICoercion[] _coercions = {
                new ParseCoercion(),
                new ConvertCoercion(), 
                new ActivatorCoercion(), 
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