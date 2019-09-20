using System;

namespace Monads
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = "Hello World!";

            //var foo = text.Bind(x => x);

            var value = text
                .Bind()
                .Try(x => x)
                .Bind(x => x)
                .Try(x => Parse(x))
                .Catch<InvalidOperationException>(x => x.ToString())
                .Catch(x => "catch all")
                .Bind(x => (string) null)
                .Bind(x => Parse(x));

            Console.WriteLine(value);
        }

        public static string Parse(string text)
        {
            //return text;
            throw new NotImplementedException();
        }

    }
}
