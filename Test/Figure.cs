using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    class Figure
    {
        private Сoordinate[] contourСoordinates;
        private Сoordinate centre;

        private double[] histogram;
        private List<Сoordinate> controlPoints = new List<Сoordinate>();

        public Figure(Сoordinate[] contourСoordinates)
        {
            this.contourСoordinates = contourСoordinates;
            centre = SearchСenter();

            histogram = new double[contourСoordinates.Length];
            histogram = GetHistogram();
        }

        public List<Сoordinate> SearchControlPoints()
        {
            bool flag = false;
            int numAngles = 0;

            double max = histogram.Max();

            List<double> listHgrm = histogram.ToList();
            listHgrm.AddRange(histogram.ToList().GetRange(0, histogram.ToList().IndexOf(histogram.Min())));
            listHgrm.RemoveRange(0, histogram.ToList().IndexOf(histogram.Min()));

            List<Сoordinate> listCrd = contourСoordinates.ToList();
            listCrd.AddRange(contourСoordinates.ToList().GetRange(0, histogram.ToList().IndexOf(histogram.Min())));
            listCrd.RemoveRange(0, histogram.ToList().IndexOf(histogram.Min()));

            int count = 0;
            for (int i = 0; i < histogram.Length; i++)
            {
                if (listHgrm[i] > max * 0.93)
                {
                    flag = true;
                    count++;
                }
                else if (flag)
                {
                    controlPoints.Add(new Сoordinate(listCrd[i - count / 2].x, listCrd[i - count / 2].y));
                    numAngles++;

                    count = 0;
                    flag = false;
                }
            }

            if (controlPoints.Count > 4) controlPoints.RemoveAll(x => true);
            controlPoints.Add(new Сoordinate(centre.x, centre.y));

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

        public double DistanceBetweenPoints(Сoordinate c1, Сoordinate c2)
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

        private Сoordinate SearchСenter()
        {
            Сoordinate centre = new Сoordinate(0, 0);
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