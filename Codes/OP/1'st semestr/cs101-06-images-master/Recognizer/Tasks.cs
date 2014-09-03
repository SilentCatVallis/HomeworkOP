using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recognizer
{
    class Tasks
    {
        /* Задача #1
         * Переведите изображение в серую гамму.
         * 
         * original[x,y,i] - это красный (i=0), зеленый (1) и синий (2) 
         * каналы пикселя с координатами x,y. Каждый канал лежит в диапазоне от 0 до 255.
         * 
         * Получившийся массив должен иметь тот же размер, 
         * grayscale[x,y] - яркость от 0 до 1 пикселя в координатах x,y
         *
         * Используйте формулу http://ru.wikipedia.org/wiki/Оттенки_серого
         */
        public static double[,] Grayscale(byte[, ,] original)
        {
            var grayArr = new double[original.GetLength(0), original.GetLength(1)];
            for (int x = 0; x < grayArr.GetLength(0); ++x)
                for (int y = 0; y < grayArr.GetLength(1); ++y)
                    grayArr[x, y] = (original[x, y, 0] * 0.299 + original[x, y, 1] * 0.587 + original[x, y, 2] * 0.114) / 255;
            return grayArr;
        }

       
        /* Задача #2
         * Очистите переданное изображение от шума.
         * 
         * Пиксели шума (и только они) имеют черный цвет (0,0,0) или белый цвет (255,255,255)
         * Вам нужно заменить эти цвета на средний цвет соседних пикселей.
         */
        public static void check(int x, int y, byte[,,] original, int colour, ref int amount, ref double answer)
        {
            if ((x >= 0 && x < original.GetLength(0) && y >= 0 && y < original.GetLength(1))
                    &&
               (original[x, y, 0] != original[x, y, 1] && original[x, y, 0] != original[x, y ,2] && original[x, y, 1] != original[x, y, 2]))
            {
                answer += original[x, y, colour];
                amount += 1;
            }
        }
        public static double takeNearColour(int x, int y, int colour, byte[,,] original)
        {
            int amount = 0;
            double answer = 0;
            for (int i = x - 2; i <= x + 2; ++i)
                for (int j = y - 2; j <= y + 2; ++j)
                {
                    check(i, j, original, colour, ref amount, ref answer);
                }
            return answer / amount;
        }
        public static void ClearNoise(byte[,,] original)
        {
            for (int x = 0; x < original.GetLength(0); ++x)
                for (int y = 0; y < original.GetLength(1); ++y)
                    if  ((original[x, y, 0] == 0 && original[x, y, 1] == 0 && original[x, y, 2] == 0)
                            ||
                        (original[x, y, 0] == 255 && original[x, y, 1] == 255 && original[x, y, 2] == 255))
                    {
                        original[x, y, 0] = (byte)takeNearColour(x, y, 0, original);
                        original[x, y, 1] = (byte)takeNearColour(x, y, 1, original);
                        original[x, y, 2] = (byte)takeNearColour(x, y, 2, original);
                    }
        }

        /* Задача #3
         * Перекрасьте 10% самых ярких пикселей в белый цвет, остальные - в черный 
         */
        public struct point
        {
            public int X;
            public int Y;
            public double pixel;
        }
        public static void ThresholdFiltering(double[,] original)
        {
            var array = new point[original.GetLength(0) * original.GetLength(1)];
            int amount = -1;
            for (int x = 0; x < original.GetLength(0); ++x)
                for (int y = 0; y < original.GetLength(1); ++y)
                {
                    amount++;
                    array[amount].X = x;
                    array[amount].Y = y;
                    array[amount].pixel = original[x, y];
                }
            Array.Sort(array, (a, b) => a.pixel.CompareTo(b.pixel));
            for (int i = array.Length - 1; i >= 0; --i)
            {
                if (i >= array.Length - array.Length / 10)
                    original[array[i].X, array[i].Y] = 1;
                else
                    original[array[i].X, array[i].Y] = 0;
            }
        }


        /* Задача #4
         * Разберитесь, как работает нижеследующий код (называемый фильтрацией Собеля), 
         * и какое отношение к нему имеют эти матрицы:
         *
         *    | -1 -2 -1 |         | -1  0  1 |  
         * Gx=|  0  0  0 |      Gy=| -2  0  2 |       
         *    |  1  2  1 |         | -1  0  1 |    
         *    
         * Замените эти матрицы на указанные здесь матрицы размера 5х5: 
         * (-)
         * 1 2
         * 4 8
         * 6 12
         * 4 8
         * 1 2....
         * http://www.cim.mcgill.ca/~image529/TA529/Image529_99/assignments/edge_detection/references/sobel.htm
         */
        public static double[,] SobelFiltering(double[,] g)
        {
            var width = g.GetLength(0);
            var height = g.GetLength(1);
            var matrix = new int[5, 5] {{1, 2, 0, -2, -1},
                                        {4, 8, 0, -8, -4},
                                        {6, 12, 0, -12, -6},
                                        {4, 8, 0, -8, -4},
                                        {1, 2, 0, -2, -1}};
            var result = new double[width,height];
            for (int x = 2; x < width - 2; x++)
                for (int y = 2; y < height - 2; y++)
                {
                    double gx = 0, gy = 0;
                    for (int i = x - 2; i <= x + 2; ++i)
                        for (int j = y - 2; j <= y + 2; ++j)
                        {
                            gx += matrix[i + 2 - x, j + 2 - y] * g[i, j];
                            gy += matrix[j + 2 - y, i + 2 - x] * g[i, j];
                        }
                    result[x, y] = Math.Sqrt(gx * gx + gy * gy);
                }
            return result;
        }



        /* Задача #5
         * Реализуйте или используйте готовый алгоритм Хафа для поиска аналитических координат прямых на изображений
         * http://ru.wikipedia.org/wiki/Преобразование_Хафа
         */
        public static Line[] HoughAlgorithm(double[,] original)
        {
            var width = original.GetLength(0);
            var height = original.GetLength(1);
            int diagonal = (int)Math.Sqrt(width * width + height * height);
            var lines = new int [360, diagonal];
            for (int x = 0; x < width; ++x)
                for (int y = 0; y < height; ++y)
                {
                    if (original[x, y] == 1)
                    {
                        for (int i = 0; i < 360; ++i)
                        {
                            double alpha = i * 2 * Math.PI / 360;
                            int length = (int)Math.Abs((x * Math.Cos(alpha) + y * Math.Sin(alpha)));
                            //int q = 0;
                            //int radius = 0;
                            lines[i, length]++;
                            /*if (length > 0)
                            {
                                while (Math.Abs(radius - length) > 0.5)
                                {
                                    radius++;
                                    q++;
                                }
                                lines[i, q]++;
                            }*/
                        }
                    }
                }
            var ans = new List<Line>();
            for (int i = 0; i < 360; ++i)
                for (int j = 0; j < diagonal; ++j)
                    if (lines[i, j] >= 95)
                    {
                        double radius = j;
                        int x1, x0, y1, y0;
                        if (i >= 0 && i <= 90)
                        {
                            double alpha = i * 2 * Math.PI / 360;
                            int wall = (int)(radius / Math.Sin(alpha));
                            int floor = (int)(wall * Math.Tan(alpha));
                            if (wall <= height)
                            {
                                y0 = height - wall;
                                x0 = 0;
                            }
                            else
                            {
                                y0 = 0;
                                x0 = (int)(Math.Tan(alpha) * (wall - height));
                            }
                            if (floor < width)
                            {
                                y1 = height;
                                x1 = floor;
                            }
                            else
                            {
                                x1 = width;
                                y1 = (int)((floor - width) / Math.Tan(alpha));
                            }
                            ans.Add(new Line(x0, y0, x1, y1));
                        }
                        if (i > 90 && i <= 180)
                        {
                            double alpha = (180 - i) * 2 * Math.PI / 360;
                            int wall = height - (int)(radius / Math.Sin(alpha));
                            if (wall > 0)
                            {
                                int floor = (int)(wall * Math.Tan(alpha));
                                if (floor < width)
                                {
                                    x0 = 0;
                                    y0 = wall;
                                    x1 = floor;
                                    y1 = 0;
                                }
                                else
                                {
                                    x0 = 0;
                                    y0 = wall;
                                    x1 = width;
                                    y1 = (int)((floor - width) / Math.Tan(alpha));
                                }
                                ans.Add(new Line(x0, y0, x1, y1));
                            }
                        }
                        if (i >= 270 && i < 360)
                        {
                            double alpha = (90 - (360 - i)) * 2 * Math.PI / 360;
                            int floor = (int)(radius / Math.Sin(alpha));
                            if (floor < width)
                            {
                                int wall = (int)((width - floor) * Math.Tan(alpha));
                                if (wall < height)
                                {
                                    x0 = floor;
                                    y0 = height;
                                    x1 = width;
                                    y1 = height - wall;
                                }
                                else
                                {
                                    x0 = floor;
                                    y0 = height;
                                    y1 = 0;
                                    x1 = floor + (int)(height / Math.Tan(alpha));
                                }
                                ans.Add(new Line(x0, y0, x1, y1));
                            }
                        }
                    }
            var result = new Line[ans.Count];
            for (int i = 0; i < ans.Count; ++i)
            {
                result[i] = ans[i];
            }
            return result;    
        }
    }
}




            /*for (int x = 0; x < width; ++x)
                for (int y = 0; y < height; ++y)
                {
                    if (original[x, y] == 1)
                        for (int theta = 1; theta < 180; theta++)
                        {
                            double alpha = theta * Math.PI / (180);
                            int length = Math.Abs((int)(x * Math.Cos(alpha) + y * Math.Sin(alpha)));
                            lines[theta, length]++;
                        }
                }
            var ans = new List<Line>();
            for (int i = 0; i < lines.GetLength(0); ++i)
                for (int j = 0; j < lines.GetLength(1); ++j)
                {
                    if (lines[i, j] > 50)
                    {
                        double alpha = i * Math.PI / (180);
                        int x0, x1, y1, y0;*/
                        /*double a = Math.Tan(alpha);
                        double b = (j) * Math.Sin(alpha);
                        if (-b / a > 0)
                        {
                            x0 = (int)(-b / a);
                            y0 = 0;
                        }
                        else
                        {
                            x0 = 0;
                            y0 = (int)b;
                        }
                        if ((height - b) / a < width)
                        {
                            y1 = height;
                            x1 = (int)((height - b) / a);
                        }
                        else
                        {
                            x1 = width;
                            y1 = (int)(a * width + b);
                        }*/
                        /*int wall = (int)(Math.Sin(alpha) * (j));
                        if (wall <= height)
                        {
                            if (wall > 0)
                                y0 = height - wall;
                            else
                                y0 = height;
                            x0 = 0;
                        }
                        else
                        {
                            y0 = 0;
                            x0 = (int)(Math.Tan(alpha) * (wall - height));
                        }
                        int floor = (int)(wall * Math.Tan(alpha));
                        if (floor <= width)
                        {
                            if (floor > 0)
                                x1 = floor;
                            else x1 = 0;
                            y1 = height;
                        }
                        else
                        {
                            x1 = width;
                            y1 = height - (int)((floor - width) / Math.Tan(alpha));
                        }
                        ans.Add(new Line(x0, y0, x1, y1));
                    }
                }
            var result = new Line[ans.Count];
            for (int i = 0; i < ans.Count; ++i)
            {
                result[i] = ans[i];
            }
            return result;    
        }
    }
}*/
            /*var ans = new List<Line>();
            for (int x = 0; x < width; ++x)
                for (int y = 0; y < height; ++y)
                {
                    if (original[x, y] == 1)
                        for (int theta = 1; theta < 180; theta++)
                        {
                            double alpha = theta * Math.PI / (180);
                            double dy = Math.Cos(alpha) * 4.0;
                            double dx = Math.Sin(alpha) * 4.0;

                            int x1 = x;
                            int y1 = y;
                            int amount = 0;
                            while (true)
                            {
                                x1 = (int)(x1 + dx);
                                y1 = (int)(y1 + dy);
                                if (x1 >= width || y1 >= height || x1 < 0 || y1 < 0)
                                    break;
                                if (original[x1, y1] == 1)
                                    amount++;
                                else
                                    break;
                            }
                            if (amount >= 3)
                            {
                                Line test = new Line(x, y, x1, y1);
                                if (ans.Count > 1)
                                {
                                    if (ans[ans.Count - 1].X0 != test.X0 || ans[ans.Count - 1].Y1 != test.Y1 || ans[ans.Count - 1].Y0 != test.Y0 || ans[ans.Count - 1].X1 != test.X1)
                                        ans.Add(test);
                                }
                                else
                                    ans.Add(test);
                            }
                        }
                }
            var result = new Line[ans.Count];
            for (int i = 0; i < ans.Count; ++i)
            {
                    result[i] = ans[i];
            }
            return result;                            
        }       
    }
}*/








/*var ans = new List<Line>();
            for (int x = 0; x < width; ++x)
                for (int y = 0; y < height; ++y)
                {
                    if (original[x, y] == 1)
                        for (int theta = 1; theta < 180; theta++)
                        {
                            double alpha = theta * Math.PI / (180);
                            double dx = Math.Cos(alpha);
                            double dy = Math.Sin(alpha);
                            double constatna = 3;
                            for (int i = 1; i < 3; ++i)
                            {
                                if ((x - dx * i * constatna >= 0 && x - dx * i * constatna < width && y - dy >= 0)
                                        &&
                                   (original[(int)(x - dx * i * constatna), (int)(y - dy)] == 1))
                                    if (i == 2)
                                    {
                                        int x0, x1, y0, y1;
                                        int floor = (int)(x + (height - y) / Math.Tan(alpha));
                                        x0 = Math.Min(floor, width);
                                        if (floor <= width)
                                            y0 = height;
                                        else y0 = (int)(height - Math.Tan(alpha) * (floor - width));
                                        int wall = (int)(Math.Tan(alpha) * floor);
                                        if (wall < height)
                                        {
                                            y1 = height - wall;
                                            x1 = 0;
                                        }
                                        else
                                        {
                                            y1 = 0;
                                            x1 = (int)(floor - (height / Math.Tan(alpha)));
                                        }
                                        ans.Add(new Line(x0, y0, x1, y1));
                                    }
                                    else
                                        break;
                            }
                        }
                }*/



/*var answer = new List<Line>();
            for (int i = 0; i < ans.Count; ++i)
            {
                int amount = 0;
                for (int j = i + 1; j < ans.Count; ++j)
                {
                    if (ans[i] == ans[j])
                        amount++;
                }
                if (amount > 1)
                {
                    answer.Add(ans[i]);
                }
            }*/