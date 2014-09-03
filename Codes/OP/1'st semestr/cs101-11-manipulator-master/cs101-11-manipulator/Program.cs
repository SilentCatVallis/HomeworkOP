using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manipulator
{
    public class Program
    {
        //static string Message;
        static Form form;

        const int PositivCoefficient = 1;
        const int NegativCoefficient = -1;
        const int DrowCoefficient = 5;
        //static double Wrist;
        //static double Elbow;
        //static double Shoulder;

        public struct Point
        {
            public Point(int x1, int y1, int x2, int y2)
            {
                X1 = x1;
                X2 = x2;
                Y1 = y1;
                Y2 = y2;
            }
            public int X1, X2, Y1, Y2;
        }
        static double X0 = -0.5;//10; // -0.5, 10, 1.5707963267
        static double Y0 = 10;
        static double Angle0 = 1.5707963267;//Math.PI / 2;

        static Point[] CalculateRobotHands()
        {
            var mass = RobotMathematics.MoveTo(X0, Y0, Angle0);

            mass[2] = -Angle0;    
            /*if (X0 < 0)
                mass[1] = Math.PI * 2 - ( mass[1] - Math.PI + mass[0]);
            else*/
                mass[1] = mass[1] - Math.PI + mass[0];
            var arm = new Point[4];
            arm[0] = new Point(0, 0, 250, 250);
            if (double.IsNaN(mass[0]))
                mass[0] = 0.2;
            for (int i = 1; i < 4; ++i)
            {
                if (double.IsNaN(mass[0]))
                {
                    return null;
                }
                int X = arm[i - 1].X2 + (int)(Math.Cos(mass[i - 1]) * RobotMathematics.Arm(i) * DrowCoefficient);
                int Y = arm[i - 1].Y2 - (int)(Math.Sin(mass[i - 1]) * RobotMathematics.Arm(i) * DrowCoefficient);
                if (X == double.NaN || Y == double.NaN || X < 0 || X > 500 || Y < 0 || Y > 500)
                {
                    return null;
                }
                arm[i] = new Point(arm[i - 1].X2, arm[i - 1].Y2, X, Y);
            }
            return arm;
        }

        static void Paint(object sender, PaintEventArgs e)
        {
            var arm = CalculateRobotHands();

            var graphics = e.Graphics;
            graphics.Clear(Color.Beige);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            var pen = new Pen(Color.Black, 2);

            for (int i = 1; i < 4; ++i)
            {
                graphics.DrawLine(pen, arm[i].X1, arm[i].Y1, arm[i].X2, arm[i].Y2);
                graphics.DrawEllipse(pen, arm[i].X2 - 3, arm[i].Y2 - 3, 6, 6);
            }

            graphics.FillRectangle(Brushes.YellowGreen, 240, 240, 20, 20);
        }


        static void changeArgs(string word, int coefficient)
        {
            if (word == "S")
                Y0 -= 0.5 * coefficient;
            if (word == "W")
                Y0 += 0.5 * coefficient;
            if (word == "A")
                X0 -= 0.5 * coefficient;
            if (word == "D")
                X0 += 0.5 * coefficient;
            if (word == "F")
                Angle0 -= Math.PI / 180 * coefficient;
            if (word == "R")
                Angle0 += Math.PI / 180 * coefficient;
        }

        static void KeyDown(object sender, KeyEventArgs key)
        {
            changeArgs(key.KeyCode.ToString(), PositivCoefficient);

            var arm = CalculateRobotHands();
            if (arm != null)
                form.Invalidate();
            else
            {
                changeArgs(key.KeyCode.ToString(), NegativCoefficient);
                //form.Invalidate();
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            form = new AntiFlickerForm();
            //form = new AntiFlickerForm(); //можете заменить AntiFlickerForm на Form, зажать какую-нибудь клавишу и почувствовать разницу
            form.Paint += Paint;
            form.KeyDown += KeyDown;
            form.ClientSize = new Size(500, 500);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.Text = "Manipulator";
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            Application.Run(form);
        }


    }
}
