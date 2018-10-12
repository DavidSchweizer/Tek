using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Tek1
{
    class TekEdit : TekView
    {
        TekStandardAreas StandardAreas;
        Random R = new Random();

        public TekEdit(Control parent, Point TopLeft, Point BottomRight) : base(parent, TopLeft, BottomRight)
        {
            // tbd
            TekFieldView.IgnoreInitial = true;
            StandardAreas = new TekStandardAreas();
            using (StreamWriter sw = new StreamWriter("areas.log"))
            {
                for(int i = 0; i < StandardAreas.Count; i++)
                {
                    TekAreaDef area = StandardAreas.GetValue(i);
                    sw.WriteLine("area {0}:", i);
                    area.DumpAsAsciiArt(sw);
                    area.Dump(sw);
                }
            }
        }

        public void ResizeBoard(int rows, int cols)
        {
            if (Board == null)
                return;
            Board.Resize(rows, cols);
            SetBoard(Board);
        }

        private void UpdateArea(TekArea area)
        {
            _view.SetAreaColors(Board);
            foreach (TekField field in area.fields)
            {
                TekFieldView view = _view.GetField(field.Row, field.Col);
                _view.SetPanelColors(view);
                _view._SetBorders(view);
            }
            _view.Refresh();
        }

        public void DeleteArea(TekArea area)
        {
            if (area == null)
                return;
            Board.DeleteArea(area);
            UpdateArea(area);
        }

        public TekArea SelectArea(int row, int col)
        {
            TekFieldView view = _view.GetField(row, col);
            Selector.CurrentMode = TekSelect.SelectMode.smMultiple;
            Selector.MultiselectFieldView.Clear();
            if (view == null)
                return null;
            TekArea area = view.Field.area;
            if (area == null)
                return null;
            foreach (TekField field in area.fields)
            {
                Selector.SelectCurrentField(_view.GetField(field.Row, field.Col));
            }
            _view.Refresh();
            return area;
        }

        private List<TekField> GetAreaFields(Point TopLeft, TekAreaDef sArea)
        {
            List<TekField> fields = new List<TekField>();
            for (int i = 0; i < sArea.PointCount; i++)
            {
                Point P = sArea.GetPoint(i);
                fields.Add(Board.values[TopLeft.Y + P.Y, TopLeft.X + P.X]);
            }
            return fields;
        }

        private void AddAreaToBoard(Point TopLeft, TekAreaDef sArea)
        {
            UpdateArea(Board.DefineArea(GetAreaFields(TopLeft, sArea)));
        }

        public bool canFit(Point TopLeft, TekAreaDef sArea)
        {
            if (TopLeft.X + sArea.xSize > Board.Cols || TopLeft.Y + sArea.ySize > Board.Rows)
                return false;
            foreach (TekField field in GetAreaFields(TopLeft, sArea))
                if (field.area != null)
                    return false;
            return true;
        }


        public int canFit(List<Point> points, TekAreaDef sArea)
        {
            if (points.Count == 0)
                return -1;
            int index = 0;
            int xMin = points[index].X;
            int yMin = points[index].Y;
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].X <= xMin && points[i].Y <= yMin)
                {
                    index = i;
                    xMin = points[index].X;
                    yMin = points[index].Y;
                }
            }
            if (canFit(points[index], sArea))
                return index;
            else return -1;
        }

        private Point FirstEmptyPoint(int r0, int c0)
        {

            int r = r0, c = c0;
            while (r < Board.Rows && c < Board.Cols)
            {
                if (Board.values[r, c].area == null)
                    break;
                c++;
                if (c >= Board.Cols)
                {
                    c = 0; r++;
                }
            }
            if (r < Board.Rows && c < Board.Cols)
                return new Point(c, r);
            else
                return new Point(-1, -1);
        }

        private List<Point> FirstEmptyArea()
        {

            List<Point> result = new List<Point>();
            Point P = FirstEmptyPoint(0, 0);
            if (P.X != -1 && P.Y != -1)
            {
                result.Add(P);
                int r = P.Y, c = P.X + 1;
                while (r < Board.Rows && c < Board.Cols && Board.values[r, c].area == null)
                {
                    result.Add(new Point(c, r));
                    if (c < Board.Cols - 1)
                        c++;
                    else
                    {
                        r++;
                        c = P.X;
                    }
                }
            }
            return result;
        }

        public bool AddRandomArea()
        {
            int standardAreaIndex0 = R.Next(0, StandardAreas.Count - 1);

            // find next open area

            List<Point> areaPoints = FirstEmptyArea();

            int standardAreaIndex = standardAreaIndex0;
            while (true)
            {
                TekAreaDef area = StandardAreas.GetValue(standardAreaIndex);
                int iPoint = canFit(areaPoints, area);
                if (iPoint != -1)
                { 
                    AddAreaToBoard(areaPoints[iPoint], area);
                    return true;
                }
                else
                {
                    standardAreaIndex = (standardAreaIndex + 1) % StandardAreas.Count;
                    if (standardAreaIndex == standardAreaIndex0)
                        return false;
                }
            }
        }
    }
}
