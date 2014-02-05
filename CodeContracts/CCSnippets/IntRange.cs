using System;
using System.Diagnostics.Contracts;

namespace CCSnippets
{
    public class IntRange {

        public static IntRange Enclose(IntRange a, IntRange b) {
            if (a == null) throw new ArgumentNullException("a");
            if (b == null) throw new ArgumentNullException("b");
            Contract.Ensures(Contract.Result<IntRange>().Low <= a.Low);
            Contract.Ensures(Contract.Result<IntRange>().Low <= b.Low);
            Contract.Ensures(Contract.Result<IntRange>().High >= a.High);
            Contract.Ensures(Contract.Result<IntRange>().High >= b.High);
            return new IntRange(Math.Min(a.Low, b.Low), Math.Max(a.High, b.High));
        }

        public static IntRange Create(int a, int b) {
            Contract.Ensures(
                (Contract.Result<IntRange>().Low == a && Contract.Result<IntRange>().High == b)
                || (Contract.Result<IntRange>().Low == b && Contract.Result<IntRange>().High == a));
            if (a > b) {
               var  temp = a;
                a = b;
                b = temp;
            }
            return new IntRange(a, b);
        }

        public IntRange(int low, int high) {
            if (low > high) throw new ArgumentException("low must be less than or equal to high");
            Contract.Ensures(Low == low);
            Contract.Ensures(High == high);
            Low = low;
            High = high;
        }

        public int Low { get; private set; }
        public int High { get; private set; }

        [ContractInvariantMethod]
        private void ObjectInvariants() {
            Contract.Invariant(Low <= High);
        }

        public int Size {
            get {
                Contract.Ensures(Contract.Result<int>() >= 0);
                return High - Low;
            }
        }

        public override string ToString() {
            return String.Format("[{0} : {1}] s:{2}", Low, High, Size);
        }

    }
}
