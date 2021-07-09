using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static Monads.Option;

namespace Monads
{
    public static class Option
    {
        
        public static Option<T> Some<T>(T value) where T : notnull => new Option<T>(value);
        public static Option<T> None<T>() where T : notnull => OptionCache<T>.None;

        private static class OptionCache<T>
            where T : notnull
        {
            public static readonly Option<T> None = new Option<T>();
        }
    }
    
    /// <summary>
    /// Can be in 2 states:
    /// 1. None state
    /// 2. Some state
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public readonly struct Option<TValue> : IEnumerable<TValue>
        where TValue : notnull
    {
        private TValue Value { get; }
        private readonly bool _isSome;

        public Option(TValue value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
            _isSome = true;
        }

        public Option<TOutput> Map<TOutput>(Func<TValue, TOutput> func)
            where TOutput : notnull =>
            _isSome ? new Option<TOutput>(func(Value)) : None<TOutput>();

        public Option<TOutput> Apply<TOutput>(Option<Func<TValue, TOutput>> option)
            where TOutput : notnull =>
            option.IsSome(out var func) ? new Option<TOutput>(func(Value)) : None<TOutput>();
        
        public Option<TOutput> Bind<TOutput>(Func<TValue, Option<TOutput>> func)
            where TOutput : notnull =>
            _isSome ? func(Value) : None<TOutput>();

        public TValue Reduce(TValue value) =>
            _isSome ? Value : value;

        public bool IsSome([MaybeNullWhen(false)]out TValue value)
        {
            value = this.Value;
            return _isSome;
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            if(_isSome)
                yield return Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return _isSome ? Value.ToString() : "None";
        }
    }
}