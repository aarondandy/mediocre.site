using System;
using System.Diagnostics.Contracts;

namespace CCSnippets
{
    public class Matrix2
    {
        public double E00;
        public double E01;
        public double E10;
        public double E11;

        public double CalculateDeterminant() {
            return (E00 * E11) - (E10 * E01);
        }

        [Pure]
        private static bool AreUnchanged(double then, double now) {
            if (Double.IsNaN(then)) return Double.IsNaN(now);
            if (Double.IsPositiveInfinity(then)) return Double.IsPositiveInfinity(now);
            if (Double.IsNegativeInfinity(then)) return Double.IsNegativeInfinity(now);
            return then == now;
        }

        public void Invert() {
            Contract.EnsuresOnThrow<InvalidOperationException>(
                AreUnchanged(Contract.OldValue(E00),E00)
                && AreUnchanged(Contract.OldValue(E01),E01)
                && AreUnchanged(Contract.OldValue(E10),E10)
                && AreUnchanged(Contract.OldValue(E11),E11));

            var determinant = CalculateDeterminant();
            if (0 == determinant
                || Double.IsNaN(determinant)
                || Double.IsInfinity(determinant))
                throw new InvalidOperationException();

            var temp = E00;
            E00 = E11 / determinant;
            E11 = temp / determinant;
            temp = -determinant;
            E01 = E01 / temp;
            E10 = E10 / temp;
        }

        public override string ToString() {
            Contract.Ensures(!String.IsNullOrWhiteSpace(Contract.Result<string>()));
            return String.Concat(E00, ' ', E01, "\n", E10, ' ', E11);
        }

    }
}
