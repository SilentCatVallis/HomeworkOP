using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manipulator
{
    /// <summary>
    /// Для лучшего понимания условий задачи см. чертеж.
    /// </summary>
    public class RobotMathematics
    {
        const double UpperArm=15;
        const double Forearm=12;
        const double Palm=10;

        //public static enum Arms : int { UpperArm , Forearm , Palm };

        public static double Arm(int i)
        {
            if (i == 0)
                return UpperArm;
            if (i == 1)
                return Forearm;
            return Palm;
        }



        static double Sqr(double a)
        {
            return a * a;
        }

        /// <summary>
        /// Возвращает угол (в радианах) между сторонами A и B в треугольнике со сторонами A,B,C 
        /// </summary>
        /// <param name="C"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static double GetAngle(double C, double A, double B)
        {
            if (A + B < C || A + C < B || B + C < A)
                return double.NaN;
            return Math.Acos((Sqr(A) + Sqr(B) - Sqr(C)) / (2 * B * A));
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Возвращает массив углов (Shoulder,Elbow,Wrist),
        /// необходимых для приведения эффектора манипулятора в точку X и Y 
        /// с углом между последним суставом и горизонталью, равному Angle (в радианах)
        /// См. чертеж Schematics.png!
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Angle"></param>
        /// <returns></returns>
        public static double[] MoveTo(double X, double Y, double Angle)
        {
            double[] angles = new double[4];

            double X1 = X - Palm * Math.Cos(Angle);
            double Y1 = Y + Palm * Math.Sin(Angle);
            double ShoulderXY = Math.Sqrt(Sqr(X) + Sqr(Y));
            double ShoulderX1Y1 = Math.Sqrt(Sqr(X1) + Sqr(Y1));
            double Elbow = GetAngle(ShoulderX1Y1, UpperArm, Forearm);
            double Shoulder = 0;
            if (Y1 > 0)
            {
                if (X1 > 0)
                    Shoulder = GetAngle(Forearm, UpperArm, ShoulderX1Y1) + GetAngle(Y1, X1, ShoulderX1Y1);
                else
                    Shoulder = GetAngle(Forearm, UpperArm, ShoulderX1Y1) + Math.PI - GetAngle(Y1, Math.Abs(X1), ShoulderX1Y1);
            }
            else
            {
                if (X1 > 0)
                    Shoulder = GetAngle(Forearm, UpperArm, ShoulderX1Y1) - GetAngle(Math.Abs(Y1), X1, ShoulderX1Y1);
                else
                    Shoulder =  -(Math.PI - (GetAngle(Forearm, UpperArm, ShoulderX1Y1) + GetAngle(Math.Abs(Y1), Math.Abs(X1), ShoulderX1Y1)));
            }
            double Wrist = GetAngle(UpperArm, Forearm, ShoulderX1Y1) + GetAngle(ShoulderXY, ShoulderX1Y1, Palm);

            angles[0] = Shoulder;
            angles[1] = Elbow;
            angles[2] = Wrist;
            return angles;

            //throw new NotImplementedException();
        }

    }
}
