using System;
using static Monads.Option;

namespace Monads
{
    /// <summary>
    /// Can be in 3 states
    /// 1. Error state
    /// 2. None state
    /// 3. Some state
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public readonly struct Error<TInput, TResult>
        where TInput : notnull
        where TResult : notnull
    {
        private readonly ErrorInfo<TInput> _errorInfo;
        
        private readonly Option<TResult> _result;
        
        private readonly bool _isError;

        public Error(TInput input, Exception error)
        {
            _errorInfo = new ErrorInfo<TInput>(input, error);
            _result = default;
            _isError = true;
        }

        public Error(Option<TResult> result)
        {
            _result = result;
            _errorInfo = default;
            _isError = false;
        }
        
        public Option<T> Match<T>(Func<ErrorInfo<TInput>, T> leftFunc, Func<TResult, T> rightFunc)
            where T : notnull =>
            _isError ? Some(leftFunc(_errorInfo)) : _result.IsSome(out var value) ? Some(rightFunc(value)) : None<T>();
    }
}