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
        public List<TekField> Fields;
        TekBoard _board;
        TekMoves _moves;
        public TekBoard Board { get { return _board; } }
        public TekMoves Moves { get { return _moves; } }

        public TekHeuristic()
        {

        }
        public TekHeuristic(string description, TekBoard board, TekMoves moves)
        {
            _description = description;
            _board = board;
            _moves = moves;
            Fields = new List<TekField>();

        }

        public bool Applies(params TekField[] fields)
        {
            if (HeuristicApplies(fields))
            {
                Fields.Clear();
                for (int i = 0; i < fields.Length; i++)
                    Fields.Add(fields[i]);
                return true;
            }
            return false;
        }

        abstract public bool HeuristicApplies(params TekField[] fields);

        abstract public void HeuristicPlay();

        public bool Apply(List <Fields> SortedFields)

    }
    public class SingleValueHeuristic: TekHeuristic
    {
        public SingleValueHeuristic(TekBoard board, TekMoves moves): base("Single Value", board, moves)
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
            Moves.PlayValue(Fields[0].Row, Fields[0].Col, Fields[0].PossibleValues[0]);
        }
    }
}
