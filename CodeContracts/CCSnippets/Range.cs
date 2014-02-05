using System;
using System.Diagnostics.Contracts;

namespace CCSnippets
{
    public class Range
    {

        public static Range Enclose(Range a, Range b) {
            if (a == null) throw new ArgumentNullException("a");
            if (b == null) throw new ArgumentNullException("b");
            Contract.Ensures(!(Contract.Result<Range>().Low > a.Low));
            Contract.Ensures(!(Contract.Result<Range>().Low > b.Low));
            Contract.Ensures(!(Contract.Result<Range>().High < a.High));
            Contract.Ensures(!(Contract.Result<Range>().High < b.High));
            return new Range(
                a.Low <= b.Low ? a.Low : b.Low,
                a.High > b.High ? a.High : b.High);
        }

        public static Range Create(double a, double b) {
            Contract.Ensures(
                (Contract.Result<Range>().Low.Equals(a) &&Contract.Result<Range>().High.Equals(b))
                || (Contract.Result<Range>().Low.Equals(b) && Contract.Result<Range>().High.Equals(a)));
            if (a > b) {
                var temp = a;
                a = b;
                b = temp;
            }
            return new Range(a, b);
        }

        public Range(double low, double high) {
            Contract.Requires(!(low > high) || Double.IsNaN(low) || Double.IsNaN(high));
            Contract.Ensures(Low.Equals(low));
            Contract.Ensures(High.Equals(high));
            Contract.EndContractBlock();
            // this is the real precondition because CC does not understand Double.NaN
            if (low > high) throw new ArgumentException("low must be less or equal to high");
            Low = low;
            High = high;
        }

        public double Low { get; private set; }

        public double High { get; private set; }

        [ContractInvariantMethod]
        private void ObjectInvariants() {
            Contract.Invariant(!(Low > High));
        }

        public double Size {
            get {
                Contract.Ensures(Contract.Result<double>() >= 0 || Double.IsNaN(Contract.Result<double>()));
                return High - Low;
            }
        }

        public override string ToString() {
            return String.Format("[{0} : {1}] s:{2}", Low, High, Size);
        }

    }
}
