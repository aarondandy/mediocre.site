using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace CCSnippets
{
    class Program
    {
        static void Main(string[] args) {
            var a = Range.Create(1, 2);
            var b = Range.Create(4, 3);
            var c = Range.Enclose(a, b);
            var d = Range.Enclose(b, a);
            var e = Range.Enclose(a, a);

            Contract.Assume(!(1 > Double.NaN));
            Contract.Assume(!(3 > Double.NaN));
            Contract.Assume(!(Double.NaN > 1));

            var fa = new Range(1, Double.NaN);
            var fb = new Range(Double.NaN, 3);
            var f = Range.Enclose(fa, fb);

            Console.WriteLine(a);
            Console.WriteLine(b);
            Console.WriteLine(c);
            Console.WriteLine(d);
            Console.WriteLine(e);
            Console.WriteLine(f);

            var m = new Matrix2 {
                E00 = Double.NaN,
                E01 = 2,
                E10 = 3,
                E11 = Double.NaN
            };
            try {
                m.Invert();
            } catch(NoInverseException){ }

            Console.WriteLine(m.E00);

            Console.ReadKey();

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
