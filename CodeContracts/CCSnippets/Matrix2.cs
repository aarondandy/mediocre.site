using System;
using System.Diagnostics.Contracts;

namespace CCSnippets
{

    public class NoInverseException : Exception { }

    public class Matrix2 {

        public double E00;
        public double E01;
        public double E10;
        public double E11;

        public void Invert() {
            Contract.EnsuresOnThrow<NoInverseException>(
                Contract.OldValue(E00).Equals(E00)
                && Contract.OldValue(E01).Equals(E01)
                && Contract.OldValue(E10).Equals(E10)
                && Contract.OldValue(E11).Equals(E11),
                "don't mutate when no inverse");

            var determinant = (E00 * E11) - (E10 * E01);
            if (0 == determinant || Double.IsNaN(determinant) || Double.IsInfinity(determinant))
                throw new NoInverseException();

            var temp = E00;
            E00 = E11 / determinant;
            E11 = temp / determinant;
            temp = -determinant;
            E01 = E01 / temp;
            E10 = E10 / temp;
        }
    }
}
