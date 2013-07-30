using System;
using System.Linq;
using System.Reflection;

namespace SquarePegRoundHole
{
    public class ParseCoercion : ICoercion
    {
        public ICoercer<T> GetCoercer<T>()
        {
            var tryParseMethod = typeof (T).GetMethods().FirstOrDefault(x =>
            {
                var ps = x.GetParameters();
                return ps.Length == 2 && x.Name == "TryParse" && x.IsStatic &&
                       ps[0].ParameterType == typeof (string) &&
                       ps[1].ParameterType == typeof (T) && ps[1].IsOut;
            });
            if (tryParseMethod != null)
            {
                try
                {
                    return new TryParseCoercer<T>(tryParseMethod);
                }
                catch (InvalidCastException)
                {
                    // wasn't a valid TryParse method after all
                }
            }

            var parseMethod = typeof(T).GetMethods().FirstOrDefault(x =>
            {
                var ps = x.GetParameters();
                return ps.Length == 1 && x.Name == "Parse" && x.IsStatic &&
                       ps[0].ParameterType == typeof (string);
            });
            if (parseMethod != null)
            {
                try
                {
                    return new ParseCoercer<T>(parseMethod);
                }
                catch (InvalidCastException)
                {
                    // wasn't a valid Parse method after all
                }
            }

            return null;
        }

        private class ParseCoercer<T> : ICoercer<T>
        {
            private delegate T Parse(string input);

            private readonly Parse _parseMethod;

            public ParseCoercer(MethodInfo parseMethod)
            {
                _parseMethod = (Parse)Delegate.CreateDelegate(typeof(Parse), parseMethod);
            }

            public CoercionResult AttemptCoercion(string input, out T output, IFormatProvider formatProvider)
            {
                try
                {
                    output = _parseMethod(input);
                    return CoercionResult.Success;
                }
                catch (FormatException)
                {
                    output = default(T);
                    return CoercionResult.Failure;
                }
            }
        }

        private class TryParseCoercer<T> : ICoercer<T>
        {
            private delegate bool TryParse(string input, out T output);

            private readonly TryParse _tryParseMethod;

            public TryParseCoercer(MethodInfo tryParseMethod)
            {
                _tryParseMethod = (TryParse)Delegate.CreateDelegate(typeof(TryParse), tryParseMethod);
            }

            public CoercionResult AttemptCoercion(string input, out T output, IFormatProvider formatProvider)
            {
                if (_tryParseMethod(input, out output))
                {
                    return CoercionResult.Success;
                }
                return CoercionResult.Failure;
            }
        }
    }
}