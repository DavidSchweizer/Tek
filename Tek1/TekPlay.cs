using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tek1
{
    public enum TekMove { tmValue, tmNote, tmClear, tmSnapshot };

    public class TekPlay
    {
        private int _row;
        private int _col;
        private TekMove _move;
        private int _value;
        private List<int> _notes;
        private string _name;
        public int Row { get { return _row; } }
        public int Col { get { return _col; } }
        public int Value { get { return _value; } }
        public List<int> Notes { get { return _notes; } } 
        public TekMove Move { get { return _move; } }
        public string Name { get { return _name; } }
        public TekPlay(int row, int col, TekMove move, int value, List<int> notes = null, string name = "")
        {
            _row = row;
            _col = col;
            _move = move;
            _value = value;
            if (notes == null)
                _notes = null;
            else
            {
                _notes = new List<int>();
                foreach (int i in notes)
                    _notes.Add(i);
            }
            _name = name;
        }
    }

    public class TekMoves
    {
        private TekBoard _board;
        public TekBoard Board { get { return _board; } }

        private Stack<TekPlay> _moves;
        public Stack<TekPlay> Moves { get { return _moves; } }

        private TekSnapShot _snapshots;
        protected TekSnapShot Snapshots { get { return _snapshots; } }

        public TekMoves(TekBoard board)
        {
            _moves = new Stack<TekPlay>();
            _board = board;
            _snapshots = new TekSnapShot(Board);
        }

        public void TakeSnapshot(string name)
        {
            Snapshots.TakeSnapshot(name);
            PushMove(0, 0, TekMove.tmSnapshot, 0, name);
        }

        public void RestoreSnapshot(string name)
        {
            Snapshots.RestoreSnapshot(name);
            while (Moves.Count > 0 && Moves.Peek().Move != TekMove.tmSnapshot && Moves.Peek().Name != name)
                Moves.Pop();
            if (Moves.Count > 0) // found snapshot location
                Moves.Pop();
        }
        public int SnapshotCount()
        {
            return Snapshots.Count();
        }
        private void PushMove(int row, int col, TekMove move, int value, string name="")
        {
            switch(move)
            {
                case TekMove.tmClear:
                    Moves.Push(new TekPlay(row, col, TekMove.tmClear, value, Board.values[row, col].Notes));
                    break;
                case TekMove.tmSnapshot:
                    Moves.Push(new TekPlay(row, col, TekMove.tmClear, value, null, name));
                    break;
                default:
                    Moves.Push(new TekPlay(row, col, move, value));
                    break;
            }
        }
        
        public void PlayValue(int row, int col, int value)
        {
            PushMove(row, col, TekMove.tmValue, value);
            Board.values[row, col].ToggleValue(value);
        }

        public void PlayNote(int row, int col, int value)
        {
            PushMove(row, col, TekMove.tmNote, value);
            Board.values[row, col].ToggleNote(value);
        }

        public void PlayClear(int row, int col)
        {
            PushMove(row, col, TekMove.tmClear, Board.values[row,col].Value);
            Board.values[row, col].ClearNotes();
            if (!Board.values[row,col].initial)
                Board.values[row, col].Value = 0;
        }

        public void UnPlay()
        {
            if (Moves.Count() > 0)
            {
                TekPlay move = Moves.Pop();
                switch (move.Move)
                {
                    case TekMove.tmValue:
                        Board.values[move.Row, move.Col].ToggleValue(move.Value);
                        break;
                    case TekMove.tmNote:
                        Board.values[move.Row, move.Col].ToggleNote(move.Value);
                        break;
                    case TekMove.tmClear:
                        Board.values[move.Row, move.Col].ToggleValue(move.Value);
                        foreach (int i in move.Notes)
                            Board.values[move.Row, move.Col].ToggleNote(i);
                        break;
                }
            }
        }
        public void Clear()
        {
            Moves.Clear();
        }
    }

    public class TekSnapShot
    {
        private TekBoard _board;
        public TekBoard Board { get { return _board; } }

        protected List<int[,]> _ssValues;
        protected List<int[,]> Values {  get { return _ssValues; } }

        protected List<List<int>[,]> _ssNotes;
        protected List<List<int>[,]> Notes {  get { return _ssNotes; } }

        protected List<string> _snapshots;
        public List<string> Snapshots { get { return _snapshots; } }


        public TekSnapShot(TekBoard board)
        {
            _ssValues = new List<int[,]>();
            _ssNotes = new List<List<int>[,]>();
            _snapshots = new List<string>();
            _board = board;
        }

        public void TakeSnapshot(string name)
        {
            Values.Add(Board.CopyValues());
            Notes.Add(Board.CopyNotes());
            Snapshots.Add(name);
        }

        public void RestoreSnapshot(string name)
        {
            int index = Snapshots.IndexOf(name);
            if (index == -1)
                return;
            Board.LoadValues(Values.ElementAt(index));
            Board.LoadNotes(Notes.ElementAt(index));
            Values.RemoveAt(index);
            Notes.RemoveAt(index);
            Snapshots.RemoveAt(index);
        }

        public int Count()
        {
            return Snapshots.Count;
        }
    }
}
