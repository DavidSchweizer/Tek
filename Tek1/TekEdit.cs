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
        public TekEdit(Control parent, Point TopLeft, Point BottomRight) : base(parent, TopLeft, BottomRight)
        {
            // tbd
            TekFieldView.IgnoreInitial = true;
        }

        public void ResizeBoard(int rows, int cols)
        {
            if (Board == null)
                return;
            Board.Resize(rows, cols);
            SetBoard(Board);
        }

        public void DeleteArea(TekArea area)
        {
            if (area == null)
                return;
            
            Board.DeleteArea(area);
            foreach(TekField field in area.fields)
            {
                TekFieldView view = _view.GetField(field.Row, field.Col);
                _view.SetPanelColors(view);
                _view._SetBorders(view);
            }
            _view.Refresh();
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
    }
}
