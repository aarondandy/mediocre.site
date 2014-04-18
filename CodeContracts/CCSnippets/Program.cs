using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CCSnippets
{
    class Program
    {

        private static readonly string[] FoodList = new [] {
            "apple", "bread", "duck", "goat milk", "jelly",
            "kale", "lamb", "orange", "radish", "salmon"};

        public class Response {
            public bool Success { get; set; }
            public string[] Foods { get; set; }
        }

        public static Response Request(string search) {
            //Contract.Ensures(!(Contract.Result<Response>().Success && Contract.Result<Response>().Foods == null));
            Contract.Ensures(!Contract.Result<Response>().Success || Contract.Result<Response>().Foods != null);
            Contract.Ensures(Contract.Result<Response>().Success ? Contract.Result<Response>().Foods != null : true);
            if (!String.IsNullOrWhiteSpace(search)) {
                Func<string, bool> test =
                    new Regex(search, RegexOptions.IgnoreCase).IsMatch;
                var foodsFound = FoodList.Where(test).ToArray();
                if (foodsFound.Length != 0)
                    return new Response {Foods = foodsFound, Success = true};
            }
            return new Response { Success = false };
        }

        static void Main(string[] args) {
            var response = Request("e");
            if (response.Success) {
                Console.WriteLine(String.Join(
                    Environment.NewLine,
                    response.Foods // warning : CodeContracts: requires unproven: value != null
                ));
            }
            Console.ReadKey();
        }

        static string FindLongest(string[] items) {
            Contract.Requires(items != null);
            Contract.Requires(Contract.ForAll(items, i => i != null));
            //Contract.Ensures(Contract.Response<string>() != null);
            if (items.Length == 0)
                return null;
            var largest = items[0];
            for (int i = 1; i < items.Length; i++)
                if (items[i].Length > largest.Length)
                    largest = items[i];
            return largest;
        }

        static string SwapLetters(string text, int a, int b) {
            Contract.Requires(text != null);
            Contract.Requires(a >= 0 && a < text.Length);
            Contract.Requires(b >= 0 && b < text.Length);
            var builder = new StringBuilder(text, text.Length);
            var c = builder[a];
            builder[a] = builder[b];
            builder[b] = c;
            return builder.ToString();
        }

        internal static string Multiply(string s, int n) {
            Contract.Requires(n >= 0);
            Contract.Requires(s != null);
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(Contract.Result<string>().Length == s.Length * n);
            var builder = new StringBuilder(s, s.Length * n);
            for (int i = 1; i < n; i++)
                builder.Append(s);
            Contract.Assume(builder.ToString().Length == s.Length * n);
            return builder.ToString();
        }

        static string NoNull(string input) {
            Contract.Requires(input != null);
            Contract.Ensures(Contract.Result<string>() != null);
            return input;
        }

        public static string WordTitleCase(string input) {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("Must not be null or empty.", "input");
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(Contract.Result<string>().Length == input.Length);

            var builder = new StringBuilder(input.Length);
            builder.Append(Char.ToUpper(input[0]));
            for (int i = 1; i < input.Length; i++)
                builder.Append(Char.ToLower(input[i]));
            Contract.Assume(builder.ToString().Length == input.Length);
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


    public class Base {
        public string Value { get; set; }
        public Base(string value) {
            if (value == null)
                throw new ArgumentNullException("value");
            Contract.EndContractBlock();
            Value = value;
        }
        public virtual void Append(string text) {
            Contract.Requires(text != null);
            Contract.EndContractBlock(); // a hint
            // not a part of the contract block
            // but will be in all builds to enforce.
            // must be separate because I am using
            // Custom Parameter Validation
            if (text == null)
                throw new ArgumentNullException("text");
            Value += text;
        }
    }

    public class WithNewLines : Base {
        public WithNewLines(string value)
            : base(value) { // enforced in here
            Contract.Requires(value != null);
        }
        public override void Append(string text) {
            // inherits preconditions automatically
            base.Append(text); // enforced in here
            Value += Environment.NewLine;
        }
        public void AppendRaw(string text) {
            Contract.Requires(text != null);
            base.Append(text); // enforced in here
        }
    }

}
