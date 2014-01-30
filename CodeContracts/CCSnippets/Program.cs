using System;
using System.Diagnostics.Contracts;

namespace CCSnippets
{
    class Program
    {
        static void Main(string[] args) {
            var text = "test";
            Console.WriteLine(NoNull(text));
        }

        static string NoNull(string input) {
            Contract.Requires(input != null);
            Contract.Ensures(Contract.Result<string>() != null);
            return input;
        }

    }
}
