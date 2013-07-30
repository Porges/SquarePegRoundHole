using System;

namespace SquarePegRoundHole
{
    public class ConvertCoercion : ICoercion
    {
        public ICoercer<T> GetCoercer<T>()
        {
            return new ConvertCoercer<T>();
        }

        private class ConvertCoercer<T> : ICoercer<T> 
        {
            public CoercionResult AttemptCoercion(string input, out T output, IFormatProvider formatProvider)
            {
                try
                {
                    output = (T) Convert.ChangeType(input, typeof (T), formatProvider);
                    return CoercionResult.Success;
                }
                catch (FormatException)
                {
                }
                catch (InvalidCastException)
                {
                }

                output = default(T);
                return CoercionResult.Failure;
            }
        }
    }
}