using System;
using static Monads.Option;

namespace Monads
{
    public static class OptionExtensions
    {
        public static Option<TValue> Bind<TValue>(this TValue input)
            where TValue : notnull
        {
            return Some(input);
        }
        
        public static Error<TInput, TOutput> Try<TInput, TOutput>(this Option<TInput> input, Func<TInput, TOutput> func)
            where TInput : notnull
            where TOutput : notnull
        {
            switch (input)
            {
                case { } when input.IsSome(out var value):
                    try
                    {
                        return new Error<TInput, TOutput>(Some(func(value)));
                    }
                    catch (Exception e)
                    {
                        return new Error<TInput, TOutput>(value, e);
                    }
                default:
                    return new Error<TInput, TOutput>(None<TOutput>());
            }
        }
    }
}