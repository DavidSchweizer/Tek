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

        protected void AddHeuristicField(TekField field)
        {
            AddOnce(HeuristicFields, field);
        }

        protected void AddHeuristicFields(params TekField[] fields)
        {
            foreach (TekField field in fields)
                AddOnce(HeuristicFields, field);
        }
        protected void AddAffectedField(TekField field)
        {
            AddOnce(AffectedFields, field);

        }

        protected void AddValue(int value)
        {
            if (!HeuristicValues.Contains(value))
                HeuristicValues.Add(value);
        }
        protected void AddValues(params int[] values)
        {
            foreach (int value in values)
                AddValue(value);
        }

        public bool Applies(TekBoard board, int StartField = 0)
        {
            Reset();
            foreach (TekField field in board.values)
            {
                if (field.Value > 0 || SkipFields.Contains(field))
                    continue;
                //                if (field.FieldIndex < StartField)
                //                  continue;
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
            switch (Action)
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

        protected bool IsPair(TekField field1, TekField field2)
        // hidden pairs are ignored
        {
            if (!field1.Influencers.Contains(field2))
                return false;
            if (field1.PossibleValues.Count != 2 || field2.PossibleValues.Count != 2)
                return false;
            foreach (int value in field1.PossibleValues)
                if (!field2.PossibleValues.Contains(value))
                    return false;
            return true;
        }

        protected bool IsTriplet(TekField field1, TekField field2, TekField field3, bool inSameArea = true)
        // hidden triplets are ignored
        {
            if (inSameArea && (field1.area != field2.area || field1.area != field3.area || field2.area != field3.area))
                return false;
            // 2 or 3 values per field
            if (field1.PossibleValues.Count < 2 || field1.PossibleValues.Count > 3)
                return false;
            if (field2.PossibleValues.Count < 2 || field2.PossibleValues.Count > 3)
                return false;
            if (field3.PossibleValues.Count < 2 || field3.PossibleValues.Count > 3)
                return false;          
            // find the common possible values: should be 3
            List<int> commonValues = field1.CommonPossibleValues(field2, field3);
            return commonValues.Count == 3;
        }

        protected bool IsInvalidThreePairs(TekField field1, TekField field2, TekField field3)
        {
            if (field1.CommonPossibleValues(field2, field3).Count != 2 || !IsPair(field1, field2) || !IsPair(field1, field3) || !IsPair(field2, field3))
                return false;
            return (field1.Influencers.Contains(field2) && field1.Influencers.Contains(field3) && field2.Influencers.Contains(field3));
        }

        static protected List<TekField> ChainBackTracking = new List<TekField>();
        protected void InitializeChain()
        { ChainBackTracking.Clear(); }

        protected bool ChainExists(TekField field, TekField target, bool isOdd)
        {
            if (field.Influencers.Contains(target))
                return !isOdd;
            else
            {
                foreach (TekField f in field.Influencers)
                    if (f.Value == 0 && !ChainBackTracking.Contains(f))
                    {
                        ChainBackTracking.Add(f);
                        if (IsPair(field, f) && ChainExists(f, target, !isOdd))
                            return true;
                        else
                            ChainBackTracking.Remove(f);
                    }
            }
            return false;
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
            if (field.PossibleValues.Count != 2)
                return false;
            foreach (TekField field2 in field.Influencers)
            {
                if (field == field2)
                    continue;
                if (IsPair(field, field2))
                {
                    AddHeuristicFields(field, field2);
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
                    AddValues(field.PossibleValues.ToArray());
                    return AffectedFields.Count > 0;
                }
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
            AddHeuristicFields(field, field2);
            if (field.PossibleValues.Count > 2)
                AddAffectedField(field);
            if (field2.PossibleValues.Count > 2)
                AddAffectedField(field2);
            AddValues(value1, value2);
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
            foreach (TekField field2 in field.Influencers)
            {
                foreach(TekField field3 in field.Influencers)
                    if (field != field2 && field != field3 && field2 != field3)
                        if (IsTriplet(field, field2, field3))
                        {
                            AddHeuristicFields(field, field2, field3);
                            AddValues(field.CommonPossibleValues(field2, field3).ToArray());
                            // determine affected fields
                            foreach(TekField f in field.CommonInfluencers(field2, field3))
                            {
                                foreach(int value in f.PossibleValues)
                                    if (HeuristicValues.Contains(value))
                                    {
                                        AddAffectedField(f);
                                        break;
                                    }
                            }
                            if (AffectedFields.Count > 0)
                                return true;
                            else
                                Reset();
                        }
            }
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
                            if (!field.Influencers.Contains(field2))
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

    public class BlockingThreePairsHeuristic : TekHeuristic
    {
        public BlockingThreePairsHeuristic() : base("Blocking (three pairs)", HeuristicAction.haExcludeValue)
        {
        }

        public override bool HeuristicApplies(TekBoard board, TekField field)
        {
            if (field.PossibleValues.Count == 0)
                return false;
            for (int i = 0; i < field.PossibleValues.Count; i++) // note: foreach can not work since we modify during processing!
            {
                try
                {
                    field.Value = field.PossibleValues[i];// trying this value
                    foreach (TekField field1 in field.Influencers) // field must at least influence two other fields (which could now be pairs)
                        foreach (TekField field2 in field.Influencers)
                            if (field1 != field2)
                                foreach (TekField field3 in field1.CommonInfluencers(field2)) 
                                    // and if there is a third field as well we might have the invalid configuration
                                    if (IsInvalidThreePairs(field1, field2, field3))
                                    {
                                        AddHeuristicField(field);
                                        AddAffectedField(field);
                                        AddValue(field.Value);
                                        return true;
                                    }
                }
                finally 
                {
                    field.Value = 0;
                }
            }
            return false;
        }
    } // BlockingThreePairsHeuristic

    public class AlternatingChainHeuristic : TekHeuristic
    {
        public AlternatingChainHeuristic() : base("Alternating Chain", HeuristicAction.haExcludeValue)
        {
        }

        public override bool HeuristicApplies(TekBoard board, TekField field)
        {
            if (field.PossibleValues.Count != 2)
                return false;
            List<TekField> candidates = new List<TekField>();

            foreach (TekField f in field.Influencers)
                if (IsPair(field, f))
                    candidates.Add(f);
            if (candidates.Count < 2)
                return false;
            InitializeChain();
            foreach (TekField field2 in candidates)
            {
                ChainBackTracking.Add(field);
                ChainBackTracking.Add(field2);
                foreach (TekField target in field2.Influencers)
                    if (target != field && target.Value == 0 && !IsPair(field, target) && ChainExists(field, target, false))
                    {
                        AddHeuristicField(field);
                        AddAffectedField(target);
                        AddValues(field.PossibleValues.ToArray());
                        return true;
                    }
                ChainBackTracking.Remove(field2);
            }
            return false;
        }
    } // AlternatingChainHeuristic

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
            Heuristics.Add(new BlockingThreePairsHeuristic());
            Heuristics.Add(new AlternatingChainHeuristic());
            LastHeuristic = null;
        }

        public TekHeuristic FindHeuristic(TekBoard board)
        {
            board.AutoNotes = true;
            foreach(TekHeuristic heuristic in Heuristics)
            {
                int index = 0;
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
