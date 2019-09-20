using System;
using System.Runtime.ExceptionServices;

namespace Monads
{
    public struct Option<TValue> : IMonad<TValue>
    {
        public TValue Value { get; }

        public Option(TValue value)
        {
            Value = value;
        }

        public static implicit operator TValue(Option<TValue> instance)
        {
            return instance.Value;
        }

        public static implicit operator Option<TValue>(TValue instance)
        {
            return new Option<TValue>(instance);
        }
    }

    public struct Try<TValue> : IMonad<TValue>
    {
        private readonly TValue _value;

        public TValue Value
        {
            get
            {
                if (Exception != null)
                {
                    ExceptionDispatchInfo.Capture(Exception).Throw();
                    throw new NotImplementedException(); //Never happens
                }
                else
                    return _value;
            }
        }

        public Exception Exception { get; }

        public static implicit operator TValue(Try<TValue> instance)
        {
            return instance.Value;
        }

        public static implicit operator Try<TValue>(TValue instance)
        {
            return new Try<TValue>(instance);
        }

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

        public Try<TValue> Catch(Func<Exception, TValue> func)
        {
            try
            {
                return Value;
            }
            catch (Exception e)
            {
                return func.Invoke(e);
            }
        }

        public Try<TValue> Catch<TException>(Func<TException, TValue> func)
            where TException : Exception
        {
            try
            {
                return Value;
            }
            catch (TException e)
            {
                return func.Invoke(e);
            }
            catch
            {
                return this;
            }
        }
    }

    public static class Extensions
    {
        public static Try<TOutput> Try<TOutput, TValue>(this TValue input, Func<TValue, TOutput> func)
        {
            try
            {
                TOutput value = func.Invoke(input);
                return value;
            }
            catch (Exception e)
            {
                return new Try<TOutput>(e);
            }
        }

        public static Option<TOutput> Bind<TOutput, TValue>(this TValue input, Func<TValue, TOutput> func)
        {
            if (input == null) return new Option<TOutput>(default);
            TOutput value = func.Invoke(input);
            return value;
        }

        public static Option<TOutput> Bind<TOutput, TValue>(this IMonad<TValue> input, Func<TValue, TOutput> func)
        {
            if (input.Value == null) return new Option<TOutput>(default);
            TOutput value = func.Invoke(input.Value);
            return value;
        }
    }

    public interface IMonad<TValue>
    {
        TValue Value { get; }
    }
}