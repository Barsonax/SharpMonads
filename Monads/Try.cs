using System;
using System.Runtime.ExceptionServices;

namespace Monads
{
    public struct Try<TValue> : IMonad<TValue>
    {
        private readonly TValue _value;

        public TValue Value
        {
            get
            {
                if (Exception == null) return _value;
                ExceptionDispatchInfo.Capture(Exception).Throw();
                throw new NotImplementedException(); //Never happens
            }
        }

        public Exception Exception { get; }

        public static implicit operator TValue(Try<TValue> instance) => instance.Value;

        internal Try(Exception exception)
        {
            Exception = exception;
            _value = default;
        }

        internal Try(TValue value)
        {
            _value = value;
            Exception = null;
        }

        public Option<TValue> Catch(Func<Exception, TValue> func) => new Option<TValue>(func.Invoke(Exception));

        public Try<TValue> Catch<TException>(Func<TException, TValue> func)
            where TException : Exception =>
            Exception is TException e ? new Try<TValue>(func.Invoke(e)) : this;
    }

    public static class TryExtensions
    {
        public static Try<TOutput> Try<TOutput, TValue>(this IMonad<TValue> input, Func<TValue, TOutput> func)
        {
            try
            {
                TOutput value = func.Invoke(input.Value);
                return new Try<TOutput>(value);
            }
            catch (Exception e)
            {
                return new Try<TOutput>(e);
            }
        }
    }
}