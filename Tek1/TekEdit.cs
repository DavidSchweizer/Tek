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
