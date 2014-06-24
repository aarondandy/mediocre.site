using System;
using System.Diagnostics.Contracts;

namespace CCSnippets
{
    internal class Program
    {
        private static void Main(string[] args) {
            var a = Range.Enclose(1, 2);
            var b = Range.Enclose(2, 1);
            Console.WriteLine("{0},{1}", a.Magnitude, b.Magnitude);
            Console.ReadKey();
        }

        private static int[] RandomIndices(int exclusiveLimit, int count) {
            Contract.Requires(exclusiveLimit > 0);
            Contract.Requires(count >= 0);
            Contract.Ensures(Contract.Result<int[]>() != null);
            Contract.Ensures(Contract.Result<int[]>().Length == count);
            Contract.Ensures(Contract.ForAll(Contract.Result<int[]>(), x => x >= 0));
            Contract.Ensures(Contract.ForAll(Contract.Result<int[]>(), x => x < exclusiveLimit));
            var randomGenerator = new Random();
            var result = new int[count];
            for (int i = 0; i < result.Length; i++)
                result[i] = randomGenerator.Next(exclusiveLimit);

            Contract.Assume(Contract.ForAll(result, x => x < exclusiveLimit));
            return result;
        }

    }


}