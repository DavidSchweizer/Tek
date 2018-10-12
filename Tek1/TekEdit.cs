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
            if (TopLeft.X + sArea.xSize >= Board.Cols || TopLeft.Y + sArea.ySize >= Board.Rows)
                return false;
            foreach (TekField field in GetAreaFields(TopLeft, sArea))
                if (field.area != null)
                    return false;
            return true;
        }

        private List<Point> FirstEmptyArea()
        {

            int r, c;
            r = c = 0;
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
            List<Point> result = new List<Point>();
            if (r < Board.Rows && c < Board.Cols)
            {
                int r0 = r, c0 = c;
                while (r < Board.Rows && c < Board.Cols && Board.values[r, c].area == null)
                {
                    result.Add(new Point(r, c));
                    if (r < Board.Rows - 1)
                        r++;

                    else if (c < Board.Cols - 1)
                        c++;
                    else 
                        r = 
                }

            } 

        }

        public bool AddRandomArea()
        {
            int areaIndex0 = R.Next(0, StandardAreas.Count - 1);

            // find next open area

            int areaIndex = areaIndex0;
            while (true)
            {
                TekAreaDef area = StandardAreas.GetValue(areaIndex);
                int r, c;
                r = c = 0;
                while (r < Board.Rows && c < Board.Cols)
                {
                    if (Board.values[r, c].area == null && canFit(new Point(r, c), area))
                        break;
                    c++;
                    if (c >= Board.Cols)
                    {
                        c = 0; r++;
                    }
                }
                if (canFit(new Point(r, c), area))
                {
                    AddAreaToBoard(new Point(r, c), area);
                    return true;
                }
                else
                {
                    areaIndex++;
                    if (areaIndex >= StandardAreas.Count)
                        areaIndex = 0;
                    if (areaIndex == areaIndex0)
                        return false;
                }
            }
        }
    }
}
