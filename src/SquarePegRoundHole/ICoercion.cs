namespace SquarePegRoundHole
{
    public interface ICoercion
    {
        ICoercer<T> GetCoercer<T>();
    }
}