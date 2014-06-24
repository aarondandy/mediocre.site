using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace CCSnippets
{
    public class Range
    {

        public static Range Enclose(double a, double b) {
            if (a <= b)
                return new Range(a, b);
            if (b < a)
                return new Range(b, a);
            throw new ArgumentException("bad values");
        }

        public Range(double min, double max) {
            if (!(min <= max)) throw new ArgumentException();
            Contract.EndContractBlock();
            Min = min;
            Max = max;
        }

        public double Min { get; private set; }
        public double Max { get; private set; }

        [ContractInvariantMethod]
        private void ObjectInvariants() {
            Contract.Invariant(Min <= Max);
        }

        public double Magnitude {
            get {
                Contract.Ensures(Contract.Result<double>() >= 0);
                return Max - Min;
            }
        }

    }
}
