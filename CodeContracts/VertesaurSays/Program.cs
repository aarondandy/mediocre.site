using System;
using System.Diagnostics.Contracts;
using System.Speech.Synthesis;
using System.Text;

namespace VertesaurSays
{
    class Program
    {

        private static readonly Random Random = new Random();
        private static readonly SpeechSynthesizer Narrator = new SpeechSynthesizer();

        static void Main(string[] args) {
            Narrator.SelectVoiceByHints(VoiceGender.Neutral, VoiceAge.Senior);
            var v = new FictionalCreature("Vertesaur", VoiceGender.Female, VoiceAge.Child) {
                Synth = { Rate = 2 } };
            var p = new FictionalCreature("Pigeoid", VoiceGender.Female, VoiceAge.Teen) {
                Synth = { Rate = 2 } };
            Console.WriteLine("Ctrl+C to end.");
            while (true) {
                SpeakLine(v, 4 + Random.Next(4), Random.Next(2) == 0);
                SpeakLine(p, 3 + Random.Next(3), Random.Next(2) == 0);
            }
        }

        public static void SpeakLine(FictionalCreature critter, int words, bool exclaim) {
            Contract.Requires(critter != null);
            Contract.Requires(words > 0);
            var critterText = critter.MakeSentence(words, exclaim);
            Console.WriteLine("{0} says, \"{1}\"", critter.Name, critterText);
            Narrator.Speak(String.Format("{0} says,", critter.Name));
            critter.Synth.Speak(critterText);
        }

        public static string WordTitleCase(string input) {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException();
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(Contract.Result<string>().Length == input.Length);

            var builder = new StringBuilder(input.Length);
            builder.Append(Char.ToUpper(input[0]));
            for (var i = 1; i < input.Length; i++)
                builder.Append(Char.ToLower(input[i]));
            Contract.Assume(builder.ToString().Length == input.Length);
            return builder.ToString();
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
                var type = Random.Next(3);
                if (type == 0)
                    return Name;
                var pivot = Random.Next(Name.Length - 2);
                return type == 1
                    ? Name.Substring(pivot)
                    : Name.Substring(0, pivot + 2);
            }

        }
    }
}
