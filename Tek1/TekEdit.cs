using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

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
        public void AddRandomArea()
        {
            TekAreaDef area = StandardAreas.GetValue(R.Next(0, StandardAreas.Count - 1));

            if (canFit(new Point(0, 0), area))
                AddAreaToBoard(new Point(0, 0), area);
            else
                MessageBox.Show("does not fit");
        }
    }
}
