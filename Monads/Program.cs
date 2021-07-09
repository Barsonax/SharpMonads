using System;
using System.Linq;
using static Monads.Option;

namespace Monads
{
    class Program
    {
        static void Main(string[] args)
        {
            var foo = new Foo();
            
            foo.Set(1);
            foo.Set(2);
            
            string text = "Hello World!";
            //var foo = text.Bind(x => x);


            var value = Some(text)
                .Bind(x => None<string>())
                .Map(x => "before " + x)
                .Map(x => x + " after")
                .Try(Parse)
                .Match<string>(x => "Some error", x => "")
                .Map(x => x + "1")
                .Reduce("default value");
                

            
            //(exception, s) => exception is InvalidOperationException ? exception.ToString() : "catch all"


            Console.WriteLine(value);
        }

        public static string Parse(string text)
        {
            //return text;
            throw new NotImplementedException();
        }

    }

    public class Foo
    {
        public Option<int> Grade { get; private set; }

        public void Set(int i)
        {
            SetOnlyOnce(Grade, i => this.Grade = Some(i), i);
        }

        public void SetOnlyOnce<T>(Option<T> property, Action<T> setter, T value)
        {
           property
                .Map<Action<T>>(_ => throw new InvalidOperationException())
                .Reduce(setter)
                .Invoke(value);
        }
    }
}
