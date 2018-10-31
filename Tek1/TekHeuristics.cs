using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tek1
{
    public abstract class TekHeuristic
    {
        string _description;
        public string Description { get { return _description; } }
        public List<TekField> HeuristicFields;
        public List<int> HeuristicValues;
        

        public TekHeuristic()
        {

        }
        public TekHeuristic(string description)
        {
            _description = description;
            HeuristicFields = new List<TekField>();
            HeuristicValues = new List<int>();
        }

        public void Reset()
        {
            HeuristicFields.Clear();
            HeuristicValues.Clear();
        }

         abstract public 
        public bool Applies(TekBoard board)
        {
            if ()
        }
        public bool AppliesSingleField(TekBoard board)
        {
            foreach(TekField field in board.values)
            {
                if (HeuristicApplies(field))
                {
                    HeuristicFields.Add(field);
                    return true;
                }
            }
            return false;
        }

        abstract public bool HeuristicApplies(params TekField[] fields);

        abstract public void HeuristicPlay();

        //abstract public bool Apply(List<TekField> SortedFields);
    }

    public class SingleValueHeuristic : TekHeuristic
    {
        public SingleValueHeuristic() : base("Single Value")
        {

        }
        public override bool HeuristicApplies(params TekField[] fields)
        {
            if (fields.Length != 1)
                return false;
            else
                return fields[0].PossibleValues.Count == 1;
        }

        public override void HeuristicPlay()
        {
            //Moves.PlayValue(Fields[0].Row, Fields[0].Col, Fields[0].PossibleValues[0]);
        }
    }
    public class HiddenSingleValueHeuristic : TekHeuristic
    {
        public HiddenSingleValueHeuristic() : base("Hidden Single")
        {

        }
        public override bool HeuristicApplies(params TekField[] fields)
        {
            if (fields.Length != 1)
                return false;
            else
            {
                int[] numberOfValues = new int[1+Const.MAXTEK];
                TekArea area = fields[0].area;
                int maxT = (area.fields.Count < Const.MAXTEK ? area.fields.Count : Const.MAXTEK);
                foreach (TekField field2 in area.fields)
                {
                    if (field2 == fields[0])
                        continue;
                    for (int i = 1; i <= maxT; i++)
                        if (field2.PossibleValues.Contains(i))
                            numberOfValues[i]++;
                }
                for (int i = 1; i <= maxT; i++)
                    if (numberOfValues[i] == 0 && fields[0].PossibleValues.Contains(i))
                    {
                        HeuristicValues.Add(i);
                        return true;
                    }
                return false;
            }
        }

        public override void HeuristicPlay()
        {
           // Moves.PlayValue(Fields[0].Row, Fields[0].Col, HeuristicValues[0]);
        }
    }

    public class TekHeuristics
    {
        List<TekHeuristic> Heuristics;

        public TekHeuristics()
        {
            Heuristics = new List<TekHeuristic>();
            Heuristics.Add(new SingleValueHeuristic());
            Heuristics.Add(new HiddenSingleValueHeuristic());
        }

        public TekHeuristic FindHeuristic(TekBoard board)
        {
            foreach(TekHeuristic heuristic in Heuristics)
            {
                heuristic.Reset();
                if (heuristic.Applies(board))
                    return heuristic;
            }
            return null;
        }
    }
}
