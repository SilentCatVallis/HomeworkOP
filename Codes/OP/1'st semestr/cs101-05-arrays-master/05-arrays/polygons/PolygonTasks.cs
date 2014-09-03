using System;
using System.Drawing;

namespace polygons
{
	public class PolygonTasks
	{
        /*PointF calculatePoint(PointF a, double maxX, double maxY, double minX, double minY, RectangleF visibleArea)
        {

        }*/
		public PointF[] RescalePolygon(PointF[] points, RectangleF visibleArea)
		{
            //гомотетия
            PointF[] newPoints = new PointF[points.Length];
            double maxY = double.NegativeInfinity;
            double minY = double.PositiveInfinity;
            double maxX = double.NegativeInfinity;
            double minX = double.PositiveInfinity;
            
            for (int i = 0; i < points.Length; ++i)
            {
                maxX = Math.Max(maxX, points[i].X);
                minX = Math.Min(minX, points[i].X);
                maxY = Math.Max(maxY, points[i].Y);
                minY = Math.Min(minY, points[i].Y);
            }
            double Koefficient = Math.Max((maxX - minX), (maxY - minY));
            for (int i = 0; i < points.Length; ++i)
            {
                //newPoints[i] = calculatePoint(points[i]);
                //if (points[i].Y == maxY && points[i].Y == minY)
                //    newPoints[i].Y = points[i].Y;
                //else
                    newPoints[i].Y = (float)(visibleArea.Y + visibleArea.Height - (visibleArea.Height * (maxY - points[i].Y) / Koefficient));
                
               // if (points[i].X == maxX && points[i].X == minX)
                //    newPoints[i].X = points[i].X;
               // else
                    newPoints[i].X = (float)(visibleArea.X + visibleArea.Width - (visibleArea.Width * (maxX - points[i].X) / Koefficient));
            }
            return newPoints;
            /* Применить сдвиг и сжатие координат многоугольника так, 
            *	чтобы многоугольник помещался внутрь visibleArea 
            *	и касался 
            *		либо его правой и левой границы одновременно, 
            *		либо верхней и нижней одновременно.
            */
		}

        PointF sub(PointF a, PointF b)
        {
            return new PointF(a.X - b.X, a.Y - b.Y);
        }

        double CalculateArea(PointF first, PointF second, PointF third)
        {
            //знаковая площадь
            PointF pointSF = sub(second, first);
            PointF pointTF = sub(third, first);
            return pointSF.X * pointTF.Y - pointSF.Y * pointTF.X;
        }


		public double GetArea(PointF[] polygon)
		{
            //нахождение площади
            if (polygon.Length <= 2)
                return 0.0;
            double area = 0.0;
            for (int i = 1; i < polygon.Length - 1; ++i)
            {
                area += CalculateArea(polygon[0], polygon[i], polygon[i + 1]);
            }
            return Math.Abs(area / 2);
		}


        

		public bool IsConvex(PointF[] polygon)
		{
            if (polygon.Length <= 3)
                return true;
            double area = CalculateArea(polygon[0], polygon[1], polygon[2]);
            bool clockwize = (area >= 0);
            bool clockwizeInTest;
            for (int i = 2; i < polygon.Length - 1; ++i)
            {
                area = CalculateArea(polygon[0], polygon[i], polygon[i + 1]);
                clockwizeInTest = (area >= 0);
                if (clockwizeInTest != clockwize)
                    return false;
            }
            return true;
			// Определите, выпуклый многоугольник или нет
		}
	}
}