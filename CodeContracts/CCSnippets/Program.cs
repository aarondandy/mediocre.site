using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace CCSnippets
{
    class Program
    {
        static void Main(string[] args) {
            var a = AxisAlignedRectangle.Create(1, 2, 3, 4);
            var b = AxisAlignedRectangle.Create(4, 3, 2, 1);
            var c = AxisAlignedRectangle.Enclose(a, b);
            var d = AxisAlignedRectangle.Enclose(b, a);
            var e = AxisAlignedRectangle.Enclose(a, a);
            var f = AxisAlignedRectangle.Enclose(
                new AxisAlignedRectangle(1, 3, 0, 1),
                new AxisAlignedRectangle(3, 4, 0, 1));

            Console.WriteLine(a);
            Console.WriteLine(b);
            Console.WriteLine(c);
            Console.WriteLine(d);
            Console.WriteLine(e);
            Console.WriteLine(f);

            Console.ReadKey();
        }

        internal static string Multiply(string s, int n) {
            Contract.Requires(n > 0);
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

            var builder = new System.Text.StringBuilder(input.Length);
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
