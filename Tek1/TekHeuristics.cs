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
        public int LastIndex = 0;
        

        public TekHeuristic()
        {

        }
        public TekHeuristic(string description)
        {
            _description = description;
            HeuristicFields = new List<TekField>();
            HeuristicValues = new List<int>();
        }
        public string AsString()
        {
            string result = String.Format("{0} [fields: ", Description);

            foreach(TekField field in HeuristicFields)
                result = result + String.Format("{0} ", field.AsString());
            result = result + ". values: ";
            foreach (int value in HeuristicValues)
                result = result + String.Format("{0} ", value);
            return result + "]";
        }
        public void Reset()
        {
            HeuristicFields.Clear();
            HeuristicValues.Clear();
            LastIndex = 0;
        }

        public void AddField(TekField field)
        {
            if (!HeuristicFields.Contains(field))
                HeuristicFields.Add(field);
        }

        public void AddValue(int value)
        {
            if (!HeuristicValues.Contains(value))
                HeuristicValues.Add(value);
        }

        public bool Applies(TekBoard board, int StartField = 0)
        {
            Reset();
            foreach(TekField field in board.values)
            {
                if (field.FieldIndex < StartField)
                    continue;
                if (HeuristicApplies(board, field))
                {
                    LastIndex = field.FieldIndex;
                    return true;
                }
            }
            return false;
        }

        abstract public bool HeuristicApplies(TekBoard board, TekField field);

        abstract public bool HeuristicPlay(TekMoves moves);

        public bool PlayValue(TekMoves moves, TekField field, int value)
        {
            bool result = field.Value == 0;
            moves.PlayValue(field, value);
            return result;
        }

        public bool ExcludeValues(TekMoves moves, TekField field)
        {
            bool result = false;
            foreach(int value in HeuristicValues)
            {
                if (field.PossibleValues.Contains(value))
                    result = true;
                moves.ExcludeValue(field, value);
            }
            return result;
        }

    }

    public class SingleValueHeuristic : TekHeuristic
    {
        public SingleValueHeuristic() : base("Single Value")
        {

        }
        public override bool HeuristicApplies(TekBoard board, TekField field)
        {
            if (field.PossibleValues.Count == 1)
            {
                AddField(field);
                AddValue(field.PossibleValues[0]);
                return true;
            }
            return false;
        }

        public override bool HeuristicPlay(TekMoves moves)
        {
            return PlayValue(moves, HeuristicFields[0], HeuristicValues[0]);
        }
    }
    public class HiddenSingleValueHeuristic : TekHeuristic
    {
        public HiddenSingleValueHeuristic() : base("Hidden Single")
        {

        }
        public override bool HeuristicApplies(TekBoard board, TekField field)
        {
            if (field.PossibleValues.Count == 0)
                return false;
            else
            {
                int[] numberOfValueAtIndex = new int[field.PossibleValues.Count];
                foreach (TekField field2 in field.area.fields)
                {
                    if (field2 == field)
                        continue;
                    for (int i = 0; i < field.PossibleValues.Count; i++)
                        if (field2.PossibleValues.Contains(field.PossibleValues[i]))
                            numberOfValueAtIndex[i]++;
                }
                for (int i = 0; i < field.PossibleValues.Count; i++)
                    if (numberOfValueAtIndex[i] == 0) // so, field is the only option for this value
                    {
                        AddField(field);
                        AddValue(field.PossibleValues[i]);
                        return true;
                    }
                return false;
            }
        }

        public override bool HeuristicPlay(TekMoves moves)
        {
            return PlayValue(moves, HeuristicFields[0], HeuristicValues[0]);
        }
    }

    public class DoubleValueHeuristic : TekHeuristic
    {
        public DoubleValueHeuristic() : base("Pair of Values")
        {

        }
        public override bool HeuristicApplies(TekBoard board, TekField field)
        {
            if (field.PossibleValues.Count == 2)
            {
                foreach (TekField field2 in field.area.fields)
                {
                    if (field2 == field || field2.PossibleValues.Count != 2 || field2.FieldIndex < field.FieldIndex)
                        continue;
                    int match = 0;
                    foreach (int value in field.PossibleValues)
                        if (field2.PossibleValues.Contains(value))
                            match++;
                    if (match < 2)
                        continue;
                    AddField(field);
                    AddField(field2);
                    foreach (int value in field.PossibleValues)
                        AddValue(value);
                    return true;
                }
                return false;
            }
            return false;
        }

        public override bool HeuristicPlay(TekMoves moves)
        {
            bool result = false;
            TekField field1 = HeuristicFields[0];
            TekField field2 = HeuristicFields[1];            
            foreach (TekField field in field1.influencers)
                if (field != field1 && field != field2 && field2.influencers.Contains(field))
                {
                    if (field.PossibleValues.Contains(HeuristicValues[0]) && ExcludeValues(moves, field))
                        result = true;
                }
            return result;
        }
    }

    public class HiddenPairHeuristic : TekHeuristic
    {
        public HiddenPairHeuristic() : base("Hidden Pair")
        {

        }
        public override bool HeuristicApplies(TekBoard board, TekField field)
        {
            Dictionary<int, List<TekField>> valuesFields = field.area.GetFieldsForValues();
            List<TekField> candidates = new List<TekField>();
            foreach (int value in field.PossibleValues)
            {
                if (!valuesFields[value].Contains(field))
                    continue;
                valuesFields[value].Remove(field);
                if (valuesFields[value].Count == 1)                
                    candidates.Add(valuesFields[value][0]); 
            }
            
            // candidates contains all fields with a shared value and no other fields for this value
            // or,better,the target field and the candidate are a (hidden) pair. 
            for (int i = 0; i < candidates.Count; i++)
                for (int j = i+1; j < candidates.Count;j++)
                    if (candidates[i] == candidates[j] && 
                        ((field.PossibleValues.Count > 2 || candidates[i].PossibleValues.Count > 2))
                        ) // this is a confirmed HIDDEN pair
                    {
                        AddField(field);
                        AddField(candidates[i]);
                        foreach(int value in field.PossibleValues)
                        {
                            if (valuesFields[value].Contains(candidates[i]))
                                AddValue(value);
                        }
                        return true;
                    }
            return false;
        }

        public override bool HeuristicPlay(TekMoves moves)
        {
            bool result = false;
            TekField field1 = HeuristicFields[0];
            TekField field2 = HeuristicFields[1];
            foreach (TekField field in field1.influencers)
                if (field != field1 && field != field2 && field2.influencers.Contains(field))
                {
                    if (field.PossibleValues.Contains(HeuristicValues[0]) && ExcludeValues(moves, field))
                        result = true;
                }
            return result;
        }
    }
    public class TekHeuristics
    {
        List<TekHeuristic> Heuristics;
        TekHeuristic LastHeuristic;

        public TekHeuristics()
        {
            Heuristics = new List<TekHeuristic>();
            Heuristics.Add(new SingleValueHeuristic());
            Heuristics.Add(new HiddenSingleValueHeuristic());
            Heuristics.Add(new DoubleValueHeuristic());
            Heuristics.Add(new HiddenPairHeuristic());
            LastHeuristic = null;
        }

        public TekHeuristic FindHeuristic(TekBoard board)
        {
            board.AutoNotes = true;
            foreach(TekHeuristic heuristic in Heuristics)
            {
                int index;
                if (heuristic == LastHeuristic)
                    index = heuristic.LastIndex + 1;
                else
                    index = 0;

                if (heuristic.Applies(board, index))
                {
                    LastHeuristic = heuristic;
                    return heuristic;
                }
            }
            LastHeuristic = null;
            return null;
        }
    }

    
}
