using System;
using Roslyn.Scripting.CSharp;

namespace SquarePegRoundHole.Dynamic
{
    public class CodeCoercion : ICoercion
    {
        private readonly bool _allowNonDelegateTypes;

        public CodeCoercion() : this (false)
        {
        }

        public CodeCoercion(bool allowNonDelegateTypes)
        {
            _allowNonDelegateTypes = allowNonDelegateTypes;
        }

        public ICoercer<T> GetCoercer<T>()
        {
            if (!typeof (Delegate).IsAssignableFrom(typeof (T)) && !_allowNonDelegateTypes)
            {
                return null;
            }

            return new CodeCoercer<T>();
        }

        class CodeCoercer<T> : ICoercer<T>
        {
            public CoercionResult AttemptCoercion(string input, out T output, IFormatProvider formatProvider)
            {
                var engine = new ScriptEngine();
                var session = engine.CreateSession();
                session.ImportNamespace("System");

                try
                {
                    output = session.Execute<T>(input);
                    return CoercionResult.Success;
                }
                catch
                {
                    output = default(T);
                    return CoercionResult.Failure;
                }
            }
        }
    }

}
