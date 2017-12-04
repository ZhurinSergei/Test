using System;
using System.Collections.Generic;

namespace Test
{
    class ImageProccesing
    {
        private static ImageProccesing _instance;

        public static ImageProccesing GetInstance()
        {
            if (_instance == null)
                _instance = new ImageProccesing();

            return _instance;
        }

        public int[,] Grayscale(byte[,,] original)
        {
            int[,] grayscale = new int[original.GetLength(0) + 2, original.GetLength(1) + 2];

            for (int x = 1; x < original.GetLength(0) - 1; x++)
            {
                for (int y = 1; y < original.GetLength(1) - 1; y++)
                {
                    double var =
                        (0.2126 * original[x, y, 0] + 0.7152 * original[x, y, 1] + 0.0722 * original[x, y, 2]) / 255;
                    if (var < 0.5) grayscale[x, y] = 0;
                    else grayscale[x, y] = 1;
                }
            }
            return grayscale;
        }

        public double[,] SobelFiltering(double[,] array)
        {
            var width = array.GetLength(0);
            var height = array.GetLength(1);
            var result = new double[width, height];

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    var gx = -array[x - 1, y - 1] - 2 * array[x, y - 1] - array[x + 1, y - 1] + array[x - 1, y + 1] +
                             2 * array[x, y + 1] + array[x + 1, y + 1];
                    var gy = -array[x - 1, y - 1] - 2 * array[x - 1, y] - array[x - 1, y + 1] + array[x + 1, y - 1] +
                             2 * array[x + 1, y] + array[x + 1, y + 1];
                    if (Math.Sqrt(gx * gx + gy * gy) >= 1) result[x, y] = 1;
                    else result[x, y] = 0;
                }
            }
            return result;
        }

        enum Direction
        {
            North,
            East,
            South,
            West
        }

        public Coordinate[] AlgorithmBeetle(int[,] g)
        {
            int cX, cY; // current
            List<Coordinate> result = new List<Coordinate>();

            Coordinate initialCrd = getInitialCrd(g);      
            result.Add(initialCrd);

            cX = initialCrd.x + 1;
            cY = initialCrd.y;
            Direction direct = Direction.East;

            while ((cX != initialCrd.x) || (cY != initialCrd.y))
            {
                switch (direct)
                {
                    case Direction.North:
                        if (g[cX, cY] == 1)
                        {
                            result.Add(new Coordinate(cX, cY));
                            direct = Direction.West;
                            cX--;
                        }
                        else
                        {
                            direct = Direction.East;
                            cX++;
                        }
                        break;

                    case Direction.East:
                        if (g[cX, cY] == 1)
                        {
                            result.Add(new Coordinate(cX, cY));
                            direct = Direction.North;
                            cY--;
                        }
                        else
                        {
                            direct = Direction.South;
                            cY++;
                        }
                        break;

                    case Direction.South:
                        if (g[cX, cY] == 1)
                        {
                            result.Add(new Coordinate(cX, cY));
                            direct = Direction.East;
                            cX++;
                        }
                        else
                        {
                            direct = Direction.West;
                            cX--;
                        }

                        break;

                    case Direction.West:
                        if (g[cX, cY] == 1)
                        {
                            result.Add(new Coordinate(cX, cY));
                            direct = Direction.South;
                            cY++;
                        }
                        else
                        {
                            direct = Direction.North;
                            cY--;
                        }
                        break;
                }
            }

            return result.ToArray();
        }

        private Coordinate getInitialCrd(int[,] array)
        {
            for (int x = 0; x< array.GetLength(0); x++)
            {
                for (int y = 0; y< array.GetLength(1); y++)
                {
                    if (array[x, y] == 1)
                    {
                        return new Coordinate(x, y);
                    }
                }
            }
            return new Coordinate(0, 0);
        }
    }
}
