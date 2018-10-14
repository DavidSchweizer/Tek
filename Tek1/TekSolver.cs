using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tek1
{
    public class TekFieldComparer : IComparer<TekField>
    {
        public int Compare(TekField x, TekField y)
        {
            if (x.PossibleValues.Count == y.PossibleValues.Count)
                return 0;
            else if (x.PossibleValues.Count == 0)
                return 1;
           	else if (y.PossibleValues.Count == 0 || x.PossibleValues.Count < y.PossibleValues.Count ) 
                return -1;
            else
                return 1;
        }
    }

    class TekSolver
    {
        private TekBoard board;
        public TekBoard Board { get { return board; } set { SetBoard(value); } }

        private List<TekField> _sortedFields;
		protected List<TekField> SortedFields { get { return _sortedFields; } }
        private TekFieldComparer sorter;
        private Stack<int[,]> _stack;

        public TekSolver(TekBoard board)
        {
            _sortedFields = new List<TekField>();
            sorter = new TekFieldComparer();
            Board = board;
            _stack = new Stack<int[,]>();
        }

        public void PushState()
        {
            _stack.Push(Board.CopyValues());
        }

        public void PopState()
        {
            Board.LoadValues(_stack.Pop());
        }

        private void SetBoard(TekBoard aboard)
        {
            board = aboard;
            
            SortedFields.Clear();
            if (Board != null)
            {
                foreach (TekField field in Board.values)
                    SortedFields.Add(field);
            }
            SortFields();          
        }

        public void SortFields()
        {
            SortedFields.Sort(sorter);
        }

        public void Dump(StreamWriter sw)
        {
            sw.WriteLine("sorted fields:");
            foreach (TekField field in SortedFields)
            {
                field.Dump(sw, TekField.FLD_DMP_POSSIBLES);                                
            }
        }

        private void SetFieldValue(TekField field, int value)
        {
            field.Value = value;
            SortFields();
        }

        public bool SimpleSolve()
        {
            bool result = false;
            while (SortedFields[0].PossibleValues.Count == 1)
            {
                int i = 0;
                result = true;
                while (i < SortedFields.Count && SortedFields[0].PossibleValues.Count == 1)
                    SortedFields[i].Value = SortedFields[i].PossibleValues[0];
                SortFields();
            }
            return result && Board.IsSolved();
        }
        StreamWriter DBG = null;

        protected void DebugLog(string format, params object[] args)
        {
            if (DBG == null)
                DBG = new StreamWriter("debugLogLog.dmp");
            if (DBG != null)
            {
                DBG.WriteLine(format, args);
                DBG.Flush();
            }           
        }    

        public bool BruteForceSolve()
        {
            TekField Field0 = SortedFields[0];
            DebugLog("bfc");
            if (Field0.PossibleValues.Count == 0)
                return Board.IsSolved();
            for (int i = 0; i < Field0.PossibleValues.Count; i++)
            {
                SetFieldValue(Field0, Field0.PossibleValues[i]);
                DebugLog("set value {0}", Field0.AsString(true));
                if (BruteForceSolve())
                    return true;
                else // backtrack 
                {
                    SetFieldValue(Field0, 0);
                    DebugLog("backtrack {0}", Field0.AsString(true));
                }
            } // if we get here, this branch has no solution
            return false;
        }

        public bool Solve()
        {
            bool result = false;
            using (StreamWriter DBG = new StreamWriter("debug.log"))
            {
                if (SimpleSolve() || BruteForceSolve())
                    result = Board.IsSolved();
            }
            return result;
        }
    }
}
