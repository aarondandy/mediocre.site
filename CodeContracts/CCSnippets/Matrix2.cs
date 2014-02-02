using System;
using System.Diagnostics.Contracts;

namespace CCSnippets
{

    public class NoInverseException : Exception { }

    public class Matrix2
    {
        public double E00;
        public double E01;
        public double E10;
        public double E11;

        public void Invert() {
            if (Double.IsNaN(E00)) throw new InvalidOperationException();
            if (Double.IsNaN(E01)) throw new InvalidOperationException();
            if (Double.IsNaN(E10)) throw new InvalidOperationException();
            if (Double.IsNaN(E11)) throw new InvalidOperationException();
            Contract.EnsuresOnThrow<NoInverseException>(
                Contract.OldValue(E00) == E00
                && Contract.OldValue(E01) == E01
                && Contract.OldValue(E10) == E10
                && Contract.OldValue(E11) == E11);

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
