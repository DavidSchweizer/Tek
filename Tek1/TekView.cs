﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;


namespace Tek1
{
    class TekView
    {
        private TekBoardView _view;
        public TekBoard Board { get { return _view.Board; } }
        protected TekMoves Moves = null;


        public TekView(Control parent, Point TopLeft, Point BottomRight)
        {
            _view = new TekBoardView();
            parent.Controls.Add(_view);
            _view.Top = TopLeft.Y;
            _view.Left = TopLeft.X;
            _view.Width = BottomRight.X - TopLeft.X;
            _view.Height = BottomRight.Y - TopLeft.Y;
        }

        public void SetBoard(TekBoard board)
        {
            _view.Board = board;
            Moves = new TekMoves(board);
        }

        public void SetSize(int width, int height)
        {
            Width = width - TekBoardView.PADDING;
            Height = height - TekBoardView.PADDING;
        }

        public bool LoadFromFile(string FileName)
        {
            TekBoardParser tbp = new TekBoardParser();
            TekBoard board = null;
            try
            {
                board = tbp.Import(FileName);
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
                return false;
            }
            if (board != null)
            {
                SetBoard(board);
                return true;
            }
            else
                return false;
        }

        public bool SetShowErrors(bool onoff = true)
        {
            _view.SetShowErrors(onoff);
            return onoff;
        }

        public bool SaveToFile(string FileName)
        {
            if (_view.Board == null)
                return false;
            TekBoardParser tbp = new TekBoardParser();
            tbp.Export(_view.Board, FileName);
            return true;
        }

        public bool ShowDefaultNotes()
        {
            if (Board == null)
                return false;
            _view.ShowDefaultNotes();
            return true;
        }

        public bool ToggleSelectedValue(int value)
        {
            if (Board != null && TekFieldView.SelectedFieldView != null)
            {
                Moves.PlayValue(TekFieldView.SelectedFieldView.Row, TekFieldView.SelectedFieldView.Col, value);
                _view.Refresh();
                return true;
            }
            else
                return false;
        }

        public bool Solve()
        {
            if (Board == null)
                return false;

           TekSolver solver = new TekSolver(Board);
            bool result = solver.Solve();

            _view.Refresh();
            return result;
        }

        public bool ResetValues()
        {
            if (Board == null)
                return false;
            Board.ResetValues();
            Moves.Clear();
            _view.Refresh();
            return true;
        }

        public bool ToggleSelectedNoteValue(int value)
        {
            if (Board != null && TekFieldView.SelectedFieldView != null)
            {
                Moves.PlayNote(TekFieldView.SelectedFieldView.Row, TekFieldView.SelectedFieldView.Col, value);
                _view.Refresh();
                return true;
            }
            else return false;
        }

        public bool UnPlay()
        {
            if (Moves != null)
            {
                Moves.UnPlay();
                _view.Refresh();
                return true;
            }
            else
                return false;
        }

        public int SnapshotCount()
        {
            if (Moves == null)
                return 0;
            else
                return Moves.SnapshotCount();
        }

        public bool TakeSnapshot(string name)
        {
            if (Moves != null)
            {
                Moves.TakeSnapshot(name);
                return true;
            }
            else
                return false;
        }

        public bool RestoreSnapshot(string name)
        {
            if (Moves != null)
            {
                Moves.RestoreSnapshot(name);
                _view.Refresh();
                return true;
            }
            else
                return false;

        }

        public void MoveSelected(int deltaR, int deltaC)
        {
            TekFieldView v = TekFieldView.SelectedFieldView;
            if (v == null)
                return;
           _view.SelectField(v.Field.Row + deltaR, v.Field.Col + deltaC);
           // v.Refresh();
        }

        public void HandleKeyDown(ref Message msg, Keys keyData)
        {
            if (TekFieldView.SelectedFieldView != null)
            {
                switch (keyData)
                {
                    case Keys.D1:
                    case Keys.D2:
                    case Keys.D3:
                    case Keys.D4:
                    case Keys.D5:
                        ToggleSelectedValue(1 + keyData - Keys.D1);
                        break;
                    case Keys.Alt | Keys.D1:
                    case Keys.Alt | Keys.D2:
                    case Keys.Alt | Keys.D3:
                    case Keys.Alt | Keys.D4:
                    case Keys.Alt | Keys.D5:
                        ToggleSelectedNoteValue(1 + (int)keyData - Keys.Alt - Keys.D1);
                        break;
                    case Keys.Up:
                        MoveSelected(-1, 0);
                        break;
                    case Keys.Down:
                        MoveSelected(1, 0);
                        break;
                    case Keys.Left:
                        MoveSelected(0, -1);
                        break;
                    case Keys.Right:
                        MoveSelected(0, 1);
                        break;
                    case Keys.Back:
                    case Keys.Control | Keys.Z:
                        UnPlay();
                        break;
                }
            }
            
        }

        public int Width { get { return _view.Width; } set { _view.Width = value; } }
        public int Height { get { return _view.Height; } set { _view.Height = value; } }

    }

    class TekBoardView : Panel
    {
        static System.Drawing.Color[] AreaColors =
            { Color.LightGreen, Color.Orange, Color.LightSkyBlue,
              Color.LightPink, Color.LightYellow, Color.LightSalmon,
              Color.LightGray, Color.Beige, Color.DeepPink
            };
        static System.Drawing.Color[] SelectedAreaColors =
             { Color.MediumSeaGreen, Color.DarkOrange, Color.DeepSkyBlue,
              Color.HotPink, Color.Goldenrod, Color.Salmon,
              Color.SlateGray, Color.SaddleBrown, Color.Fuchsia
            };
        const int MAXCOLOR = 9;
        static int[] AreaColorIndex = null;

        private TekFieldView[,] _Panels = null;

        private TekPanelData data = new TekPanelData();

        private TekBoard board = null;
        public TekBoard Board { get { return board; } set { SetBoard(value); } }
        private void SetBoard(TekBoard value)
        {
            board = value;
            SetAreaColors(board);
            initializePanels();
            SetBorders();
        }

        private void removeBoard()
        {
            if (_Panels == null)
                return;
            for (int r = 0; r < _Panels.GetLength(0); r++)
                for (int c = 0; c < _Panels.GetLength(1); c++)
                {
                    this.Controls.Remove(_Panels[r, c]);
                    _Panels[r, c] = null;
                }
            _Panels = null;
        }

        public TekBoardView()
        {
            this.Resize += Board_Resize;
        }

        private void Panel_Click(object sender, EventArgs e)
        {
            if (Board == null)
                return;
            if (sender is TekFieldView)
            {
                TekFieldView panel = sender as TekFieldView;
                panel.SelectPanel(!panel.IsSelected);
            }
        }

        private void Board_Resize(object sender, EventArgs e)
        {
            if (Board != null)
            {
                data.TileSize = ComputeTileSize(ClientRectangle.Width, ClientRectangle.Height);
                for (int r = 0; r < Board.Rows; r++)
                    for (int c = 0; c < Board.Cols; c++)
                        ReSizeFieldView(_Panels[r, c], data.TileSize, r, c);
                Refresh();
            }
                
        }
        public const int PADDING = 6;

        private int ComputeTileSize(int width, int height)
        {
            if (Board != null)
                return Math.Min((width - PADDING) / Board.Cols, (height - PADDING) / Board.Rows);
            else
                return 0;
        }

        private void ReSizeFieldView(TekFieldView v, int TileSize, int r, int c)
        {
            v.Size = new Size(TileSize, TileSize);
            v.Location = new Point(PADDING / 2 + data.TileSize * c, PADDING / 2 + data.TileSize * r);
//            this.Width = PADDING + Board.Cols * data.TileSize;
//            this.Height = PADDING + Board.Rows * data.TileSize;
        }

        private void initializePanels()
        {
           
            var clr1 = Color.DarkGray;
            var clr2 = Color.White;
            Random R = new Random();

            removeBoard();
            if (Board == null)
                return;

            data.TileSize = ComputeTileSize(this.ClientRectangle.Width, this.ClientRectangle.Height);

            _Panels = new TekFieldView[Board.Rows, Board.Cols];

            for (int r = 0; r < Board.Rows; r++)
                for (int c = 0; c < Board.Cols; c++)
                {
                    TekFieldView newP = new TekFieldView();
                    ReSizeFieldView(newP, data.TileSize, r, c);
                    
                    newP.Data = data;
                    newP.Field = Board.values[r, c];
                    newP.NormalColor = AreaColors[AreaColorIndex[newP.Field.area.AreaNum]];
                    newP.SelectedColor = SelectedAreaColors[AreaColorIndex[newP.Field.area.AreaNum]];
                    newP.Click += new EventHandler(Panel_Click);
                    //(newP as Control).KeyDown+= new EventHandler(Panel_KeyDown);

                    this.Controls.Add(newP);
                    _Panels[r, c] = newP;
                }
            this.Width = PADDING + Board.Cols * data.TileSize;
            this.Height = PADDING + Board.Rows * data.TileSize;
        }

        private void SetAreaColors(TekBoard board)
        {
            AreaColorIndex = new int[board.areas.Count()];
            for (int i = 0; i < AreaColorIndex.Length; i++)
                AreaColorIndex[i] = -1; // 
            Random R = new Random();
            foreach (TekArea area in board.areas)
            {
                List<TekArea> neighbours = area.GetAdjacentAreas();
                List<int> inUseByNeighbours = new List<int>();
                foreach (TekArea area2 in neighbours)
                    if (AreaColorIndex[area2.AreaNum] != -1)
                        inUseByNeighbours.Add(AreaColorIndex[area2.AreaNum]);
                int index0 = R.Next(MAXCOLOR);
                int index = (index0 + 1) % MAXCOLOR;
                while (index != index0)
                {
                    if (inUseByNeighbours.Contains(index))
                        index = (index + 1) % MAXCOLOR;
                    else
                        break;
                }
                AreaColorIndex[area.AreaNum] = index;
            }
        }

        private void SetBorders()
        {
            if (Board == null || _Panels == null)
                return;
            int area = Board.values[0, 0].area.AreaNum;
            for (int r = 0; r < Board.Rows; r++)
                for (int c = 0; c < Board.Cols; c++)
                {
                    TekFieldView p = _Panels[r, c];
                    if (r == 0)
                        p.Borders[(int)TekFieldView.TekBorder.bdTop] = TekFieldView.TekBorderStyle.tbsBoard;
                    else
                    {
                        TekField f2 = Board.values[r - 1, c];
                        if (f2.area.AreaNum == p.Field.area.AreaNum)
                            p.Borders[(int)TekFieldView.TekBorder.bdTop] = TekFieldView.TekBorderStyle.tbsInternal;
                        else
                            p.Borders[(int)TekFieldView.TekBorder.bdTop] = TekFieldView.TekBorderStyle.tbsExternal;
                    }
                    if (c == 0)
                        p.Borders[(int)TekFieldView.TekBorder.bdLeft] = TekFieldView.TekBorderStyle.tbsBoard;
                    else
                    {
                        TekField f2 = Board.values[r, c - 1];
                        if (f2.area.AreaNum == p.Field.area.AreaNum)
                            p.Borders[(int)TekFieldView.TekBorder.bdLeft] = TekFieldView.TekBorderStyle.tbsInternal;
                        else
                            p.Borders[(int)TekFieldView.TekBorder.bdLeft] = TekFieldView.TekBorderStyle.tbsExternal;
                    }
                    if (r == Board.Rows - 1)
                        p.Borders[(int)TekFieldView.TekBorder.bdBottom] = TekFieldView.TekBorderStyle.tbsBoard;
                    else
                    {
                        TekField f2 = Board.values[r + 1, c];
                        if (f2.area.AreaNum == p.Field.area.AreaNum)
                            p.Borders[(int)TekFieldView.TekBorder.bdBottom] = TekFieldView.TekBorderStyle.tbsInternal;
                        else
                            p.Borders[(int)TekFieldView.TekBorder.bdBottom] = TekFieldView.TekBorderStyle.tbsExternal;
                    }
                    if (c == Board.Cols - 1)
                        p.Borders[(int)TekFieldView.TekBorder.bdRight] = TekFieldView.TekBorderStyle.tbsBoard;
                    else
                    {
                        TekField f2 = Board.values[r, c + 1];
                        if (f2.area.AreaNum == p.Field.area.AreaNum)
                            p.Borders[(int)TekFieldView.TekBorder.bdRight] = TekFieldView.TekBorderStyle.tbsInternal;
                        else
                            p.Borders[(int)TekFieldView.TekBorder.bdRight] = TekFieldView.TekBorderStyle.tbsExternal;
                    }
                    p.Invalidate();
                }
            
        }

        public TekFieldView SelectField(int row, int col)
        {
            TekFieldView result = TekFieldView.SelectedFieldView;
            if (row >= 0 && row < Board.Rows && col >= 0 && col < Board.Cols)
                _Panels[row, col].SelectPanel();
            return result;
        }

        public void SetShowErrors(bool onoff = true)
        {
            if (Board == null)
                return;
            foreach (TekFieldView P in _Panels)
                P.FieldError = onoff && P.Field != null && !P.Field.IsValid();
            Refresh();
        }

        public void ShowDefaultNotes()
        {
            if (Board == null)
                return;
            Board.SetDefaultNotes();
            Refresh();
        }
    } // TekBoardView

    class TekFieldView : Panel
    {
        public enum TekBorder { bdTop, bdRight, bdBottom, bdLeft, bdLast };
        public enum TekBorderStyle { tbsNone, tbsInternal, tbsExternal, tbsBoard, tbsSelected };

        private System.Drawing.Color _NormalColor;
        public System.Drawing.Color NormalColor
        {
            get { return _NormalColor; }
            set { _NormalColor = value; if (!IsSelected) BackColor = value; }
        }

        private System.Drawing.Color _SelectedColor;
        public System.Drawing.Color SelectedColor
        {
            get { return _SelectedColor; }
            set { _SelectedColor = value; if (IsSelected) BackColor = value; }
        }

        private TekField field;

        public bool FieldError { get; set; } = false;
        static public TekFieldView SelectedFieldView = null;

        public bool IsSelected { get; set; }
        private TekPanelData _data;

        public void ToggleNote(int value)
        {
            if (field != null)
            {
                field.ToggleNote(value);
                Refresh();
            }
        }

        public int Row { get { return field == null ? -1 : field.Row; } }
        public int Col { get { return field == null ? -1 : field.Col; } }
        public int Value { get { return field == null ? 0 : field.Value; } set { if (field != null && !field.initial) { field.Value = value; Refresh(); } } }

        public TekBorderStyle[] Borders { get; set; }

        public TekPanelData Data { get { return _data; } set { _data = value; this.Refresh(); } }

        public TekFieldView() : base()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            Borders = new TekBorderStyle[(int)TekBorder.bdLast];
            for (int b = 0; b <= (int)TekBorder.bdTop; b++)
                Borders[b] = TekBorderStyle.tbsNone;
            NormalColor = Color.AliceBlue; // to find out if something is wrong
            SelectedColor = Color.YellowGreen;
        }

        public void SelectPanel(bool onoff = true)
        {
            if (Field != null && Field.initial)
                return;
            if (!onoff)
            {
                this.BackColor = NormalColor;
                SelectedFieldView = null;
                IsSelected = false;
            }
            else
            {
                if (SelectedFieldView != null)
                    SelectedFieldView.SelectPanel(false);
                SelectedFieldView = this;
                IsSelected = true;
                this.BackColor = SelectedColor;
            }
            this.Refresh();
        }

        private void SetField(TekField value)
        {
            field = value;

            this.Refresh();
        }

        public TekField Field
        {
            get { return field; }
            set { SetField(value); }
        }

        private void DrawBorders(PaintEventArgs e)
        {
            DrawBorderType(e, TekBorderStyle.tbsInternal);
            DrawBorderType(e, TekBorderStyle.tbsExternal);
            DrawBorderType(e, TekBorderStyle.tbsBoard);
        }

        private void DrawBorderType(PaintEventArgs e, TekBorderStyle BS)
        {
            for (TekBorder border = TekBorder.bdTop; border < TekBorder.bdLast; border++)
            {
                if (IsSelected)
                    DrawSingleBorder(e, border, TekBorderStyle.tbsSelected);
                else if (Borders[(int)border] == BS)
                    DrawSingleBorder(e, border, BS);
            }
        }

        private void DrawSingleBorder(PaintEventArgs e, TekBorder border, TekBorderStyle BS)
        {
            //tbsNone, tbsInternal, tbsExternal, tbsBoard, tbsSelected
            int[] penSizes = { 0, 1, 1, 1, 1 };
            int iBS = (int)BS;
            int iBorder = (int)border;
            System.Drawing.Color[] bColors = { Color.White, Color.DarkGray, Color.Black, Color.Black, Color.AntiqueWhite };

            int pensize = penSizes[iBS];


            int[] X1 = { 0, e.ClipRectangle.Width - 1, e.ClipRectangle.Width - 1, 0 };
            int[] X2 = { e.ClipRectangle.Width - 1, e.ClipRectangle.Width - 1, 0, 0 };
            int[] Y1 = { 0, 0, e.ClipRectangle.Height - 1, e.ClipRectangle.Height - 1 };
            int[] Y2 = { 0, e.ClipRectangle.Height - 1, e.ClipRectangle.Height - 1, 0 };
            if (pensize > 1)
            {
                int pensize2 = pensize / 2;
                switch (border)
                {
                    case TekBorder.bdTop:
                        Y1[iBorder] += pensize2; Y2[iBorder] += pensize2;
                        break;
                    case TekBorder.bdLeft:
                        X1[iBorder] += pensize2; X2[iBorder] += pensize2;
                        break;
                }
            }
            e.Graphics.DrawLine(new Pen(new SolidBrush(bColors[iBS]), pensize), X1[iBorder], Y1[iBorder], X2[iBorder], Y2[iBorder]);
        }
        private void DisplayNote(PaintEventArgs e, int value)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            Data.SetCenterAlignment();
            e.Graphics.DrawString(value.ToString(),
                Data.ValueFont[TekPanelData.FONT_NOTE],
                    Data.TextBrush[IsSelected ? TekPanelData.PANEL_SELECTED : TekPanelData.PANEL_NORMAL],
                    Data.NotePoint[value - 1], Data.Format);
        }

        private void DisplayNotes(PaintEventArgs e)
        {
            if (Field != null && Field.Value == 0 && Field.Notes.Count > 0)
            {
                foreach (int value in Field.Notes)
                    DisplayNote(e, value);
            }
        }
        private void DisplayValue(PaintEventArgs e)
        {
            if (Field != null && Field.Value > 0)
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                Data.SetCenterAlignment();
                SolidBrush textBrush;
                if (FieldError)
                    textBrush = Data.TextBrush[TekPanelData.PANEL_ERROR];
                else
                    textBrush = Data.TextBrush[IsSelected ? TekPanelData.PANEL_SELECTED : TekPanelData.PANEL_NORMAL];
                e.Graphics.DrawString(Field.Value.ToString(),
                    Data.ValueFont[field.initial ? TekPanelData.FONT_INITIAL : TekPanelData.FONT_NORMAL],
                        textBrush, Data.ValuePoint, Data.Format);
            }
        } 
     
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DisplayValue(e);
            DisplayNotes(e);
            DrawBorders(e);
        }
    }

    class TekPanelData
    {
        // data to assist in displaying the fields
        const int MAXTILESIZE = 60;
        const int MINTILESIZE = 25;
        FontFamily fontFamily = new FontFamily("Calibri");
        int FontSize;
        int FontSize2;
        public Font[] ValueFont;
        public SolidBrush[] TextBrush;
        public Point ValuePoint;
        public Point[] NotePoint;
        private StringFormat format = new StringFormat();
        public StringFormat Format { get { return format; } }

        private int _tileSize;
        public int TileSize { get { return _tileSize; } set { SetTileSize(value); } }

        public const int FONT_NORMAL = 0;
        public const int FONT_INITIAL = 1;
        public const int FONT_NOTE = 2;

        public const int PANEL_NORMAL   = 0;
        public const int PANEL_SELECTED = 1;
        public const int PANEL_ERROR    = 2;

        public void SetTileSize(int value)
        {
            if (value == _tileSize)
                return;
            if (value > MAXTILESIZE)
                _tileSize = MAXTILESIZE;
            else
                if (value < MINTILESIZE)
                value = MINTILESIZE;
            else
                _tileSize = value;
            FontSize = Convert.ToInt32(TileSize * 0.7);
            FontSize2 = Convert.ToInt32(FontSize / 2.5);
            ValueFont = new Font[3];
            ValueFont[FONT_NORMAL] =
                new Font(fontFamily, FontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            ValueFont[FONT_INITIAL] =
                new Font(fontFamily, FontSize, FontStyle.Bold, GraphicsUnit.Pixel);
            ValueFont[FONT_NOTE] =
                new Font(fontFamily, FontSize2, FontStyle.Regular, GraphicsUnit.Pixel);
            TextBrush = new SolidBrush[3];
            TextBrush[PANEL_NORMAL] = new SolidBrush(Color.Black);
            TextBrush[PANEL_SELECTED] = new SolidBrush(Color.White);
            TextBrush[PANEL_ERROR] = new SolidBrush(Color.Red);

            ValuePoint = new Point(TileSize / 2, TileSize / 2);

            NotePoint = new Point[Const.MAXTEK];
            int d = TileSize / 5;
            NotePoint[0] = new Point(d, d);
            NotePoint[1] = new Point(TileSize - d, d);
            NotePoint[2] = new Point(TileSize / 2, TileSize / 2);
            NotePoint[3] = new Point(d, TileSize - d);
            NotePoint[4] = new Point(TileSize - d, TileSize - d);
        }

        public void SetCenterAlignment()
        {
            Format.LineAlignment = StringAlignment.Center;
            Format.Alignment = StringAlignment.Center;
        }
    }

}
