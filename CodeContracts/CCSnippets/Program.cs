using System;
using System.Diagnostics.Contracts;

namespace CCSnippets
{
    class Program
    {
        static void Main(string[] args) {
            var text = "test";
            Console.WriteLine(NoNull(text));

            Console.WriteLine(WordTitleCase("woRd"));
            Console.WriteLine(new[]{1,2,3}.QuickFirstOrDefault());
            Console.ReadKey();
        }

        static string NoNull(string input) {
            Contract.Requires(input != null);
            Contract.Ensures(Contract.Result<string>() != null);
            return input;
        }

        public static string WordTitleCase(string input) {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("Must not be null or empty.", "input");
            if (Char.IsWhiteSpace(input[0]) || Char.IsWhiteSpace(input[input.Length - 1]))
                throw new ArgumentException("Must be trimmed.", "input");
            Contract.EndContractBlock();

            var builder = new System.Text.StringBuilder(input.Length);
            builder.Append(Char.ToUpper(input[0]));
            for (int i = 1; i < input.Length; i++)
                builder.Append(Char.ToLower(input[i]));
            return builder.ToString();
        }

    }

    internal static class EnumerableHelpers
    {
        internal static T QuickFirstOrDefault<T>(this T[] array) {
            Contract.Requires(array != null);
            return array.Length == 0 ? default(T) : array[0];
        }
    }

}
