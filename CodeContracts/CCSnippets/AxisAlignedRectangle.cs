using System;
using System.Diagnostics.Contracts;

namespace CCSnippets
{
    public class AxisAlignedRectangle
    {

        public static AxisAlignedRectangle Enclose(AxisAlignedRectangle a, AxisAlignedRectangle b) {
            if (a == null) throw new ArgumentNullException("a");
            if (b == null) throw new ArgumentNullException("b");
            Contract.Ensures(Contract.Result<AxisAlignedRectangle>().XMin <= a.XMin);
            Contract.Ensures(Contract.Result<AxisAlignedRectangle>().XMin <= b.XMin);
            Contract.Ensures(Contract.Result<AxisAlignedRectangle>().XMax >= a.XMax);
            Contract.Ensures(Contract.Result<AxisAlignedRectangle>().XMax >= b.XMax);
            Contract.Ensures(Contract.Result<AxisAlignedRectangle>().YMin <= a.YMin);
            Contract.Ensures(Contract.Result<AxisAlignedRectangle>().YMin <= b.YMin);
            Contract.Ensures(Contract.Result<AxisAlignedRectangle>().YMax >= a.YMax);
            Contract.Ensures(Contract.Result<AxisAlignedRectangle>().YMax >= b.YMax);
            return new AxisAlignedRectangle(
                a.XMin <= b.XMin ? a.XMin : b.XMin,
                a.XMax >= b.XMax ? a.XMax : b.XMax,
                a.YMin <= b.YMin ? a.YMin : b.YMin,
                a.YMax >= b.YMax ? a.YMax : b.YMax);
        }

        public static AxisAlignedRectangle Create(double xa, double xb, double ya, double yb) {
            if (Double.IsNaN(xa)) throw new ArgumentException("NaN", "xa");
            if (Double.IsNaN(xb)) throw new ArgumentException("NaN", "xb");
            if (Double.IsNaN(ya)) throw new ArgumentException("NaN", "ya");
            if (Double.IsNaN(yb)) throw new ArgumentException("NaN", "yb");
            Contract.EndContractBlock();
            double temp;
            if (!(xa <= xb)) {
                temp = xa;
                xa = xb;
                xb = temp;
            }
            if (!(ya <= yb)) {
                temp = ya;
                ya = yb;
                yb = temp;
            }
            return new AxisAlignedRectangle(xa, xb, ya, yb);
        }

        public AxisAlignedRectangle(double xMin, double xMax, double yMin, double yMax) {
            if (Double.IsNaN(xMin)) throw new ArgumentException("NaN", "xMin");
            if (Double.IsNaN(xMax)) throw new ArgumentException("NaN", "xMax");
            if (Double.IsNaN(yMin)) throw new ArgumentException("NaN", "yMin");
            if (Double.IsNaN(yMax)) throw new ArgumentException("NaN", "yMax");
            if (!(xMin <= xMax)) throw new ArgumentException("xMin must be less or equal to xMax");
            if (!(yMin <= yMax)) throw new ArgumentException("yMin must be less or equal to yMax");
            Contract.Ensures(XMin == xMin);
            Contract.Ensures(YMin == yMin);
            Contract.Ensures(XMax == xMax);
            Contract.Ensures(YMax == yMax);
            XMin = xMin;
            XMax = xMax;
            YMin = yMin;
            YMax = yMax;
        }

        public double XMin { get; private set; }
        public double XMax { get; private set; }
        public double YMin { get; private set; }
        public double YMax { get; private set; }

        [ContractInvariantMethod]
        private void ObjectInvariants() {
            Contract.Invariant(!Double.IsNaN(XMin));
            Contract.Invariant(!Double.IsNaN(XMax));
            Contract.Invariant(!Double.IsNaN(YMin));
            Contract.Invariant(!Double.IsNaN(YMax));
            Contract.Invariant(XMin <= XMax);
            Contract.Invariant(YMin <= YMax);
        }

        public double Width { get {
            Contract.Ensures(Contract.Result<double>() >= 0);
            return XMax - XMin;
        } }

        public double Height { get {
            Contract.Ensures(Contract.Result<double>() >= 0);
            return YMax - YMin;
        } }

        public override string ToString() {
            return String.Format("({0},{1};{2},{3}) w:{4} h{5}", XMin, YMin, XMax, YMax, Width, Height);
        }

    }
}
