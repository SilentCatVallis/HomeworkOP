using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorMathematic
{
    public class Vector
    {
        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector(Angle angle, double length)
        {
            X = length * Math.Cos(angle.Radians);
            Y = length * Math.Sin(angle.Radians);
        }

        public double X { get; private set; }
        public double Y { get; private set; }

        public Vector Subtract(Vector v)
        {
            return new Vector(X - v.X, Y - v.Y);
        }

        public Vector Add(Vector v)
        {
            return new Vector(X + v.X, Y + v.Y);
        }

        public Vector Multiply(double coefficient)
        {
            return new Vector(X * coefficient, Y * coefficient);
        }

        public Vector Rotate(Vector centre, Angle angle)
        {
            var xn = -Math.Sin(angle.Radians) * (Y - centre.Y) + Math.Cos(angle.Radians) * (X - centre.X) + centre.X;
            var yn = Math.Cos(angle.Radians) * (Y - centre.Y) + Math.Sin(angle.Radians) * (X - centre.X) + centre.Y; 
            //double dX = (X - centre.X);
            //double dY = (Y - centre.Y);
            //double sin = Math.Sin(angle.Radians);
            //double cos = Math.Cos(angle.Radians);
            return new Vector(xn, yn);
            //return new Vector(centre.X + dX * cos - dY * sin, centre.Y + dX * sin + dX * cos);
        }

        public double Length { get { return Math.Sqrt(X * X + Y * Y); } }

        public override string ToString()
        {
            return X.ToString() + " ; " + Y.ToString();
        }

        public override bool Equals(object obj)
        {
            Vector vector = obj as Vector;
            if (vector == null)
                return false;
            return Math.Abs(X - vector.X) == 0 && Math.Abs(Y - vector.Y) == 0;
        }

        public static bool operator ==(Vector vector1, Vector vector2)
        {
            if ((object)vector1 == null || (object)vector2 == null)
                return false;
            return Math.Abs(vector1.X - vector2.X) == 0 && Math.Abs(vector1.Y - vector2.Y) == 0;
        }

        public static bool operator !=(Vector vector1, Vector vector2)
        {
            return !(vector1 == vector2);
        }

        public override int GetHashCode()
        {
            return (X.GetHashCode() * 397 + Y.GetHashCode() * 397) * 13;
        }

        public Vector Copy()
        {
            return new Vector(X, Y);
        }
    }

    public class Angle
    {
        private double rad;

        public Angle(double radians)
        {
            Radians = radians;
        }

        public Angle(Vector vector)
        {
            Radians = Math.Atan2(vector.Y, vector.X);
        }

        public double Radians { get { return rad; } private set { rad = Normalize(value); } }

        public double Normalize(double value)
        {
            var val = value % (2 * Math.PI);
            if (val > 0 && val > Math.PI)
                val -= Math.PI * 2;
            if (val <= 0 && val < -Math.PI)
                val += Math.PI * 2;
            if (Math.Abs(val + Math.PI) < 1e-12)
                val = Math.PI;
            return val;
        }

        public Angle Add(Angle angle)
        {
            return new Angle(Radians + angle.Radians);
        }

        public Angle Substract(Angle angle)
        {
            return new Angle(Radians - angle.Radians);
        }

        public override string ToString()
        {
            return rad.ToString();
        }

        public override bool Equals(object obj)
        {
            Angle angle = obj as Angle;
            if (angle == null)
                return false;
            return Math.Abs(rad - angle.rad) == 0;
        }

        public static bool operator >(Angle angle1, Angle angle2)
        {
            if ((object)angle1 == null || (object)angle2 == null)
                return false;
            return angle1.rad > angle2.rad;
        }

        public static bool operator <(Angle angle1, Angle angle2)
        {
            if ((object)angle1 == null || (object)angle2 == null)
                return false;
            return angle1.rad < angle2.rad;
        }

        public static bool operator ==(Angle angle1, Angle angle2)
        {
            if ((object)angle1 == null || (object)angle2 == null)
                return false;
            return Math.Abs(angle1.rad - angle2.rad) < 1e-6;
        }

        public static bool operator !=(Angle angle1, Angle angle2)
        {
            return !(angle1 == angle2);
        }

        public override int GetHashCode()
        {
            return (int)((rad * 1005000 + 17) * 19);
        }

        public Angle Copy()
        {
            return new Angle(rad);
        }
    }
}
