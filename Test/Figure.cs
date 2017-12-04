using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    class Figure
    {
        private Coordinate[] contourСoordinates;
        private Coordinate centre;

        private double[] histogram;
        private List<Coordinate> controlPoints = new List<Coordinate>();

        public Figure(Coordinate[] contourСoordinates)
        {
            this.contourСoordinates = contourСoordinates;

            centre = SearchСenter();
            histogram = new double[contourСoordinates.Length];
            histogram = GetHistogram();
        }

        public List<Coordinate> SearchControlPoints()
        {
            List<double> listHgrm = histogram.ToList();
            listHgrm.AddRange(histogram.ToList().GetRange(0, histogram.ToList().IndexOf(histogram.Min())));
            listHgrm.RemoveRange(0, histogram.ToList().IndexOf(histogram.Min()));

            List<Coordinate> listCrd = contourСoordinates.ToList();
            listCrd.AddRange(contourСoordinates.ToList().GetRange(0, histogram.ToList().IndexOf(histogram.Min())));
            listCrd.RemoveRange(0, histogram.ToList().IndexOf(histogram.Min()));

            int count = 0;
            bool flag = false;
            for (int i = 0; i < histogram.Length; i++)
            {
                if (listHgrm[i] > histogram.Max() * 0.93)
                {
                    count++;
                    flag = true;
                }
                else if (flag)
                {
                    controlPoints.Add(new Coordinate(listCrd[i - count / 2].x, listCrd[i - count / 2].y));

                    count = 0;
                    flag = false;
                }
            }

            if (controlPoints.Count > 4) controlPoints.RemoveAll(x => true);
            controlPoints.Add(new Coordinate(centre.x, centre.y));

            return controlPoints;
        }

        private double[] GetHistogram()
        {
            double[] result = new double[contourСoordinates.Length];

            for (int i = 0; i < contourСoordinates.Length; i++)
            {
                result[i] = DistanceBetweenPoints(contourСoordinates[i], centre);
            }

            Normalization(result);
            return result;
        }

        public double DistanceBetweenPoints(Coordinate c1, Coordinate c2)
        {
            return Math.Sqrt(Math.Pow(c1.x - c2.x, 2) + Math.Pow(c1.y - c2.y, 2));
        }

        private void Normalization(double[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] /= array.Max();
            }
        }

        private Coordinate SearchСenter()
        {
            Coordinate centre = new Coordinate(0, 0);
            for (int i = 0; i < contourСoordinates.Length; i++)
            {
                centre.x += contourСoordinates[i].x;
                centre.y += contourСoordinates[i].y;
            }
            centre.x /= contourСoordinates.Length;
            centre.y /= contourСoordinates.Length;

            return centre;
        }
    }
}