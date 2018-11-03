using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Tek1
{
    public enum HeuristicAction { haNone, haSetValue, haExcludeValue, haExcludeComplement };
    public abstract class TekHeuristic
    {
        static string[] actionDescriptions = { "none", "set value", "exclude value(s)", "exclude complement value(s)" };

        string _description;
        private HeuristicAction _action; 
        public HeuristicAction Action { get { return _action; } }
        public string Description { get { return _description; } }
        public List<TekField> HeuristicFields;
        public List<TekField> SkipFields;
        public List<TekField> AffectedFields;
        public List<int> HeuristicValues;
//        public int LastIndex = 0;
        

        public TekHeuristic()
        {

        }
        public TekHeuristic(string description, HeuristicAction action)
        {
            _description = description;
            _action = action;
            HeuristicFields = new List<TekField>();
            SkipFields = new List<TekField>();
            AffectedFields = new List<TekField>();
            HeuristicValues = new List<int>();
        }
        public string AsString()
        {
           string result = String.Format("{0} [fields: ", Description);

            foreach (TekField field in HeuristicFields)
                result = result + String.Format("{0} ", field.AsString());
            result = result + "; affects: ";
            foreach (TekField field in AffectedFields)
                result = result + String.Format("{0} ", field.AsString());
            result = result + "| values: ";
            foreach (int value in HeuristicValues)
                result = result + String.Format("{0} ", value);
            return result + "] " + actionDescriptions[(int)Action];
        }
        public void Reset(bool totalReset = false)
        {
            HeuristicFields.Clear();
            AffectedFields.Clear();
            if (totalReset)
                SkipFields.Clear();
            HeuristicValues.Clear();
            //LastIndex = 0;
        }

        private void AddOnce(List<TekField> list, TekField field)
        {
            if (!list.Contains(field))
                list.Add(field);
        }

        public void AddHeuristicField(TekField field)
        {
            AddOnce(HeuristicFields, field);
        }
        public void AddAffectedField(TekField field)
        {
            AddOnce(AffectedFields, field);

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
                if (field.Value > 0 || SkipFields.Contains(field))
                    continue;
                if (field.FieldIndex < StartField)
                    continue;
                if (HeuristicApplies(board, field))
                {
//                    LastIndex = field.FieldIndex;
                    return true;
                }
                else Reset();
            }
            return false;
        }

        abstract public bool HeuristicApplies(TekBoard board, TekField field);

        //abstract public bool HeuristicPlay(TekMoves moves);

        public void SetValue(TekMoves moves, TekField field, int value)
        {
            bool result = field.Value == 0;
            moves.PlayValue(field, value);
        }

        public void ExcludeValues(TekMoves moves, TekField field)
        {
            foreach (int value in HeuristicValues)
            {
                if (field.PossibleValues.Contains(value))
                    moves.ExcludeValue(field, value);
            }
        }

        public void ExcludeComplementValues(TekMoves moves, TekField field)
        {
            List<int> excludingValues = new List<int>();
            foreach (int value in field.PossibleValues)
            {
                if (!HeuristicValues.Contains(value))
                    excludingValues.Add(value);
            }
            foreach (int value in excludingValues)
                if (field.PossibleValues.Contains(value))
                    moves.ExcludeValue(field, value);
        }

        public void ExecuteAction(TekMoves moves)
        {
              switch(Action)
              {
                case HeuristicAction.haNone:
                    break;

                case HeuristicAction.haSetValue:
                    SetValue(moves, AffectedFields[0], HeuristicValues[0]);
                    break;

                case HeuristicAction.haExcludeValue:
                    foreach (TekField field in AffectedFields)
                        ExcludeValues(moves, field);
                    break;

                case HeuristicAction.haExcludeComplement:
                    foreach (TekField field in AffectedFields)
                    {
                        ExcludeComplementValues(moves, field);
                    }
                    break;
              }
        }

    } // TekHeuristic

    public class SingleValueHeuristic : TekHeuristic
    {
        public SingleValueHeuristic() : base("Single Value", HeuristicAction.haSetValue)
        {
        }
        public override bool HeuristicApplies(TekBoard board, TekField field)
        {
            if (field.PossibleValues.Count == 1)
            {
                AddHeuristicField(field);
                AddAffectedField(field);
                AddValue(field.PossibleValues[0]);
                return true;
            }
            return false;
        }
    } // SingleValueHeuristic

    public class HiddenSingleValueHeuristic : TekHeuristic
    {
        public HiddenSingleValueHeuristic() : base("Hidden Single", HeuristicAction.haSetValue)
        {
        }
        public override bool HeuristicApplies(TekBoard board, TekField field)
        {
            if (field.PossibleValues.Count == 0)
                return false;
            else
            {
                Dictionary<int, List<TekField>> FieldsPerValueInArea = field.area.GetFieldsForValues();
                foreach(int value in field.PossibleValues)
                    if(FieldsPerValueInArea[value].Count == 1)
                    {
                        AddHeuristicField(field);
                        AddAffectedField(field);
                        AddValue(value);
                        return true;
                    }
                return false;
            }
        }
    } // HiddenSingleValueHeuristic

    public class DoubleValueHeuristic : TekHeuristic
    {
        public DoubleValueHeuristic() : base("Pair of Values", HeuristicAction.haExcludeValue)
        {
        }
        public override bool HeuristicApplies(TekBoard board, TekField field)
        {
            if (field.PossibleValues.Count == 2)
            {
                foreach (TekField field2 in field.area.fields)
                {
                    if (field == field2)
                        continue;
                    bool hasAllValues = field2.PossibleValues.Count == 2;
                    if (hasAllValues)
                        foreach (int value in field.PossibleValues)
                        {
                            if (!field2.PossibleValues.Contains(value))
                                hasAllValues = false;
                        }
                    if (hasAllValues)
                    {
                        AddHeuristicField(field);
                        AddHeuristicField(field2);
                        foreach (TekField f in field.CommonInfluencers(field2))
                        {
                            bool isAffected = false;
                            foreach (int value in field.PossibleValues)
                                if (f.PossibleValues.Contains(value))
                                {
                                    isAffected = true;
                                    break;
                                }
                            if (isAffected)
                                AddAffectedField(f);
                        }
                        foreach (int value in field.PossibleValues)
                            AddValue(value);
                        return AffectedFields.Count > 0;
                    }
                }
                return false;
            }
            return false;
        }


    }

    public class HiddenPairHeuristic : TekHeuristic
    {// this could perhaps be replaced by Triplets, since they are sort-of complimentary
        public HiddenPairHeuristic() : base("Hidden Pair", HeuristicAction.haExcludeComplement)
        {
        }
        public override bool HeuristicApplies(TekBoard board, TekField field)
        {
            Dictionary<int, List<TekField>> FieldsPerValueInArea = field.area.GetFieldsForValues();
            List<int> CandidateValues = new List<int>();
            List<TekField> CandidateFields = new List<TekField>();
            foreach (int value in field.PossibleValues)
            {
                FieldsPerValueInArea[value].Remove(field);
                if (FieldsPerValueInArea[value].Count == 1)
                {
                    CandidateValues.Add(value);
                    CandidateFields.Add(FieldsPerValueInArea[value][0]);
                }
            }
            TekField field2 = null;
            int value1 = 0, value2 = 0;
            for (int i = 0; i < CandidateFields.Count && field2 == null; i++)
            {
                for (int j = i+1; j < CandidateFields.Count && field2 == null; j++)
                    if (CandidateFields[j] == CandidateFields[i])
                    {
                        field2 = CandidateFields[i];
                        value1 = CandidateValues[i];
                        value2 = CandidateValues[j];
                    }
            }
            if (field2 == null)
                return false;
            AddHeuristicField(field);
            AddHeuristicField(field2);
            if (field.PossibleValues.Count > 2)
                AddAffectedField(field);
            if (field2.PossibleValues.Count > 2)
                AddAffectedField(field2);
            AddValue(value1);
            AddValue(value2);
            return AffectedFields.Count > 0;
        }
    }

    public class TripletHeuristic : TekHeuristic
    {
        public TripletHeuristic() : base("Triplets", HeuristicAction.haExcludeValue)
        {
        }

        public override bool HeuristicApplies(TekBoard board, TekField field)
        {
            Dictionary<int, List<TekField>> FieldsPerValueInArea = field.area.GetFieldsForValues();
            List<int> CandidateValues = new List<int>();
            List<TekField> CandidateFields = new List<TekField>();
            foreach (int value in field.PossibleValues)
            {
                FieldsPerValueInArea[value].Remove(field);
                if (FieldsPerValueInArea[value].Count >= 1 && FieldsPerValueInArea[value].Count <= 2)
                {
                    foreach (TekField f in FieldsPerValueInArea[value])
                    {
                        CandidateValues.Add(value);
                        CandidateFields.Add(f);
                    }
                }
            }
            //TekField field2 = null, field3 = null;
            //int value1 = 0, value2 = 0, value3 = 0;
            //for (int i = 0; i < CandidateFields.Count && field2 == null && field3 == null; i++)
            //{
            //    for (int j = i + 1; j < CandidateFields.Count && field2 == null && field3 == null; j++)
            //        if (CandidateFields[j] == CandidateFields[i])
            //        {
            //            field2 = CandidateFields[i];
            //            value1 = CandidateValues[i];
            //            value2 = CandidateValues[j];
            //            for(int k = j+1; k < CandidateFields.Count && field2 == null && field3 == null; k++)
            //                if(CandidateFields[k]==CandidateFields[j])
            //        }
            //}
            List<TekField> candidates = new List<TekField>();
            List<int> TripletValues = new List<int>();
            if (field.PossibleValues.Count > 3 || field.PossibleValues.Count < 2)
                return false;
           // foreach (int value in field.PossibleValues)
           // {
           //     TripletValues.Add(value);
           //     valuesFields[value].Remove(field);
           //     if (valuesFields[value].Count == 1 && !candidates.Contains(valuesFields[value][0]))
           //         candidates.Add(valuesFields[value][0]);
           //     if (valuesFields[value].Count == 2 && !candidates.Contains(valuesFields[value][1]))
           //         candidates.Add(valuesFields[value][1]);
           // }
           //while (i >= 0)
           // {
           //     TekField f = candidates[i];
           //      foreach(int value in f.PossibleValues)
           //      { 
           //             if ()


           // // candidates contains all fields with shared value(s) and no other fields for this value
           // // or,better,the target field and the candidate are a (hidden) pair.

           // for (int i = 0; i < candidates.Count; i++)
           //     for (int j = i + 1; j < candidates.Count; j++)
           //         if (candidates[i] == candidates[j] &&
           //             ((field.PossibleValues.Count > 2 || candidates[i].PossibleValues.Count > 2))
           //             ) // this is a confirmed HIDDEN pair
           //         {
           //             AddField(field);
           //             AddField(candidates[i]);
           //             foreach (int value in field.PossibleValues)
           //             {
           //                 if (valuesFields[value].Contains(candidates[i]) && valuesFields[value].Count == 1)
           //                     AddValue(value);
           //             }
           //             return true;
           //         }
            return false;
        }

    } // TripletHeuristic

    public class BlockingHeuristic : TekHeuristic
    {
        public BlockingHeuristic() : base("Blocking", HeuristicAction.haExcludeValue)
        {
        }

        public override bool HeuristicApplies(TekBoard board, TekField field)
        {
            List<TekArea> AdjacentAreas = field.area.GetAdjacentAreas();
            foreach (TekArea area in AdjacentAreas)
            {
                Dictionary<int, List<TekField>> FieldsPerValueInArea = area.GetFieldsForValues();
                foreach (int value in field.PossibleValues)
                {
                    if (FieldsPerValueInArea.Keys.Contains(value))
                    {
                        bool blocking = true;
                        foreach (TekField field2 in FieldsPerValueInArea[value])
                            if (!field.influencers.Contains(field2))
                            {
                                blocking = false;
                                break;
                            }
                        if (blocking)
                        {
                            AddHeuristicField(field);
                            AddAffectedField(field);
                            AddValue(value);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

    } // BlockingHeuristic

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
            Heuristics.Add(new TripletHeuristic());
            Heuristics.Add(new BlockingHeuristic());
            LastHeuristic = null;
        }

        public TekHeuristic FindHeuristic(TekBoard board)
        {
            board.AutoNotes = true;
            foreach(TekHeuristic heuristic in Heuristics)
            {
                int index = 0;
//                if (heuristic == LastHeuristic)
 //                   index = heuristic.LastIndex + 1;
   //             else
     //               index = 0;

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
