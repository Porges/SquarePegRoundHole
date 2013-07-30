using System;

namespace SquarePegRoundHole
{
    public interface ICoercer<T>
    {
        CoercionResult AttemptCoercion(string input, out T output, IFormatProvider formatProvider);
    }
}