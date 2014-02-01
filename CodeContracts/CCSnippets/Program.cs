using System;
using System.Diagnostics.Contracts;

namespace CCSnippets
{
    class Program
    {
        static void Main(string[] args) {
            var text = "test";
            Console.WriteLine(NoNull(text));

            Contract.Assume(IsTrimmed("woRd"));
            Console.WriteLine(WordTitleCase("woRd"));

            var m = new Matrix2 {
                E00 = 1,
                E01 = 2,
                E10 = 3,
                E11 = Double.NaN,
            };
            try {
                m.Invert();
            }
            catch (InvalidOperationException) {
                Console.WriteLine("oops!");
            }

            Console.WriteLine(m);

            var a = new Base("123");
            a.Append("!");

            Console.WriteLine(a.Value);

            var b = new WithNewLines("123");
            b.Append("!");
            b.AppendRaw("abc");

            Console.WriteLine(b.Value);

            Console.ReadKey();
        }

        static string NoNull(string input) {
            Contract.Requires(input != null);
            Contract.Ensures(Contract.Result<string>() != null);
            return input;
        }

        
        [Pure]
        public static bool IsTrimmed(string text) {
            if (text == null)
                return false;
            if (text.Length == 0)
                return true;
            if (Char.IsWhiteSpace(text[0]))
                return false;
            return text.Length == 1 || !Char.IsWhiteSpace(text[text.Length - 1]);
        }

        public static string WordTitleCase(string input) {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("Must not be null or empty.", "input");
            if (!IsTrimmed(input))
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
            // must be seperate because I am using
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
