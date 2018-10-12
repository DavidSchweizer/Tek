using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace Tek1
{
    public class TekAreaDef
    {
        Point TopLeft;
        List<Point> Deltas;

        public TekAreaDef(Point topLeft, params Point[] deltas)
        {
            TopLeft = topLeft;
            Deltas = new List<Point>();
            foreach(Point value in deltas)
            {
                Deltas.Add(value);
            }
        }

        public void Dump(StreamWriter sw)
        {
            sw.Write(String.Format("Area: [{0},{1}]", TopLeft.X, TopLeft.Y));
            foreach(Point value in Deltas)
            {
                sw.Write(" ({0},{1})", value.X, value.Y);
            }
            sw.WriteLine();
        }
    }
}
