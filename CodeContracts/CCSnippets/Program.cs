using System;
using System.Diagnostics.Contracts;
using System.Speech.Synthesis;
using System.Text;

namespace CCSnippets
{
    class Program
    {

        private static readonly Random _random = new Random();
        private static readonly SpeechSynthesizer _narrator = new SpeechSynthesizer();

        static void Main(string[] args) {
            _narrator.SelectVoiceByHints(VoiceGender.Neutral, VoiceAge.Senior);

            var v = new FictionalCreature("Vertesaur", VoiceGender.Female, VoiceAge.Child) { Synth = { Rate = 2 } };
            var p = new FictionalCreature("Pigeoid", VoiceGender.Female, VoiceAge.Teen) { Synth = { Rate = 2 } }; ;
            Console.WriteLine("Ctrl+C to end.");
            while(true) {
                SpeakLine(v, 4 + _random.Next(4), _random.Next(2) == 0);
                SpeakLine(p, 3 + _random.Next(3), _random.Next(2) == 0);
            }
        }

        public static void SpeakLine(FictionalCreature critter, int words, bool exclaim) {
            Contract.Requires(critter != null);
            Contract.Requires(words > 0);
            var critterText = critter.MakeSentence(words, exclaim);
            Console.WriteLine("{0} says, \"{1}\"", critter.Name, critterText);
            _narrator.Speak(String.Format("{0} says,", critter.Name));
            critter.Synth.Speak(critterText);
        }

        public class FictionalCreature
        {

            public FictionalCreature(string name, VoiceGender gender, VoiceAge age) {
                if (String.IsNullOrEmpty(name)) throw new ArgumentException();
                if (name.Length < 4) throw new ArgumentException();
                Contract.EndContractBlock();
                Name = name;
                Synth = new SpeechSynthesizer();
                Synth.SelectVoiceByHints(gender, age);
            }

            public string Name { get; private set; }
            public SpeechSynthesizer Synth { get; private set; }

            [ContractInvariantMethod]
            private void ObjectInvariants() {
                Contract.Invariant(!String.IsNullOrEmpty(Name));
                Contract.Invariant(Name.Length >= 4);
                Contract.Invariant(Synth != null);
            }

            public string MakeSentence(int words, bool exclaim = true) {
                if (words <= 0) throw new ArgumentException();
                Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));
                var builder = new StringBuilder(WordTitleCase(MakeWord()));
                for (var i = 1; i < words; i++) {
                    builder.Append(' ');
                    builder.Append(MakeWord().ToLower());
                }
                builder.Append(exclaim ? '!' : '.');
                Contract.Assume(!String.IsNullOrEmpty(builder.ToString()));
                return builder.ToString();
            }

            public string MakeWord() {
                Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));
                Contract.Ensures(Contract.Result<string>().Length >= 2);
                var type = _random.Next(3);
                string word;
                if (type == 0) {
                    word = Name;
                }
                else {
                    var pivot = _random.Next(Name.Length - 2);
                    Contract.Assume(pivot >= 0);
                    Contract.Assume(pivot < Name.Length - 2);
                    if (type == 1) {
                        word = Name.Substring(pivot);
                        Contract.Assume(word.Length == Name.Length - pivot);
                    }
                    else {
                        word = Name.Substring(0, pivot + 2);
                        Contract.Assume(word.Length == pivot + 2);
                    }
                }
                return word;
            }

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
