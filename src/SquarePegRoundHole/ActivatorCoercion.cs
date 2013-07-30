using System;
using System.Reflection;

namespace SquarePegRoundHole
{
    public class ActivatorCoercion : ICoercion
    {
        public ICoercer<T> GetCoercer<T>()
        {
            if (IsValidType<T>())
            {
                return new ActivatorCoercer<T>();
            }
            return null;
        }

        private bool IsValidType<T>()
        {
            return typeof (T).GetConstructor(new[] {typeof (string)}) != null;
        }

        private class ActivatorCoercer<T> : ICoercer<T>
        {
            public CoercionResult AttemptCoercion(string input, out T output, IFormatProvider formatProvider)
            {
                try
                {
                    output = (T)Activator.CreateInstance(typeof(T), input);
                    return CoercionResult.Success;
                }
                catch (TargetInvocationException)
                {
                    output = default(T);
                    return CoercionResult.Failure;
                }
                
            }
        }
    }
}
