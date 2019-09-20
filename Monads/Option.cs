using System;

namespace Monads
{
    public struct Option<TValue> : IMonad<TValue>
    {
        public TValue Value { get; }

        public Option(TValue value) => Value = value;

        public static implicit operator TValue(Option<TValue> instance) => instance.Value;
    }

    public static class OptionExtensions
    {
        public static Option<TOutput> Bind<TOutput, TValue>(this IMonad<TValue> input, Func<TValue, TOutput> func)
        {
            if (input.Value == null) return new Option<TOutput>(default);
            TOutput value = func.Invoke(input.Value);
            return new Option<TOutput>(value);
        }

        public static Option<TValue> Bind<TValue>(this TValue input)
        {
            return new Option<TValue>(input);
        }
    }
}