using System;

namespace Monads
{
    public readonly struct ErrorInfo<TInput>
        where TInput : notnull
    {
        public readonly Exception Error;
        public readonly TInput Input;

        public ErrorInfo(TInput input, Exception error)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Error = error ?? throw new ArgumentNullException(nameof(error));
        }
    }
}