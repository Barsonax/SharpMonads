namespace Monads
{
    public interface IMonad<TValue>
    {
        TValue Value { get; }
    }
}