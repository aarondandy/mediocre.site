using System;
using System.Diagnostics.Contracts;

namespace CCSnippets
{
    public class AxisAlignedRectangle
    {

        public static AxisAlignedRectangle Enclose(AxisAlignedRectangle a, AxisAlignedRectangle b) {
            if (a == null) throw new ArgumentNullException("a");
            if (b == null) throw new ArgumentNullException("b");
            return new AxisAlignedRectangle(
                Range.Enclose(a.X,b.X),
                Range.Enclose(a.Y,b.Y));
        }

        public static AxisAlignedRectangle Create(double xa, double xb, double ya, double yb) {
            Contract.Ensures(Contract.Result<AxisAlignedRectangle>() != null);
            return new AxisAlignedRectangle(Range.Create(xa, xb), Range.Create(ya, yb));
        }

        public AxisAlignedRectangle(double xMin, double xMax, double yMin, double yMax)
        : this(new Range(xMin,xMax),new Range(yMin,yMax)) {
            Contract.Requires(!(xMin > xMax) || Double.IsNaN(xMin) || Double.IsNaN(xMax));
            Contract.Requires(!(yMin > yMax) || Double.IsNaN(yMin) || Double.IsNaN(yMax));
        }

        public AxisAlignedRectangle(Range x, Range y) {
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            Contract.Ensures(X == x);
            Contract.Ensures(Y == y);
            X = x;
            Y = y;
        }

        public Range X { get; private set; }
        public Range Y { get; private set; }

        [ContractInvariantMethod]
        private void ObjectInvariants() {
            Contract.Invariant(X != null);
            Contract.Invariant(Y != null);
        }

        public double Width {
            get {
                Contract.Ensures(Contract.Result<double>() >= 0 || Double.IsNaN(Contract.Result<double>()));
                return X.Size;
            }
        }

        public double Height {
            get {
                Contract.Ensures(Contract.Result<double>() >= 0 || Double.IsNaN(Contract.Result<double>()));
                return Y.Size;
            }
        }

        public override string ToString() {
            return String.Format("X:{0} y:{1}", X, Y);
        }

    }
}
