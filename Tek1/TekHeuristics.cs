﻿using System;
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
        public List<TekField> AffectedFields;
        public List<int> HeuristicValues;
        protected TekRegion Region;

        public TekHeuristic()
        {
            Region = new TekRegion();
        }

        public TekHeuristic(string description, HeuristicAction action) : this()
        {
            _description = description;
            _action = action;
            HeuristicFields = new List<TekField>();
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

        public void Reset()
        {
            HeuristicFields.Clear();
            AffectedFields.Clear();
            HeuristicValues.Clear();
            Region.Clear();
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
        protected void AddHeuristicFields(List <TekField> fields)
        {
            foreach (TekField field in fields)
                AddOnce(HeuristicFields, field);
        }

        protected void AddAffectedField(TekField field)
        {
            AddOnce(AffectedFields, field);
        }
        protected void AddAffectedFields(params TekField[] fields)
        {
            foreach (TekField field in fields)
                AddOnce(AffectedFields, field);
        }
        protected void AddAffectedFields(List<TekField> fields)
        {
            foreach (TekField field in fields)
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
        protected void AddValues(List<int> values)
        {
            foreach (int value in values)
                AddValue(value);
        }

        protected virtual void SetupApplies(TekBoard board)
        {
            // override to setup local variables
        }

        public bool Applies(TekBoard board)
        {
            Reset();
            SetupApplies(board);
            foreach (TekField field in board.values)
            {
                if (field.Value > 0 )
                    continue;
               if (HeuristicApplies(board, field))
                {
                    return true;
                }
                else Reset();
            }
            return false;
        }

        abstract public bool HeuristicApplies(TekBoard board, TekField field);

        public void SetValue(TekMoves moves, TekField field, int value)
        {
            bool result = field.Value == 0;
            moves.PlayValue(field, value);
        }

        public void ExcludeValues(TekMoves moves, TekField field)
        {
            foreach (int value in HeuristicValues)
            {
                if (field.ValuePossible(value))
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
                if (field.ValuePossible(value))
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
                    {
                        ExcludeValues(moves, field);
                        if (field.PossibleValues.Count == 1)
                            SetValue(moves, field, field.PossibleValues[0]);
                    }
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

    public class CoupledPairHeuristic : TekHeuristic
    {
        public CoupledPairHeuristic() : base("Coupled Pair", HeuristicAction.haExcludeValue)
        {
        }

        public override bool HeuristicApplies(TekBoard board, TekField field)
        {
            if (field.PossibleValues.Count != 2)
                return false;
            Region.AddField(field);
            foreach (TekField field2 in field.Influencers)
            {
                if (field == field2)
                    continue;
                if (Region.IsPair(field2))
                { 
                    AddHeuristicFields(field, field2);
                    foreach (TekField f in field.CommonInfluencers(field2))
                    {
                        bool isAffected = false;
                        foreach (int value in field.PossibleValues)
                            if (f.ValuePossible(value))
                            {
                                isAffected = true;
                                break;
                            }
                        if (isAffected)
                            AddAffectedField(f);
                    }
                    AddValues(field.PossibleValues);
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

    public class CoupledTripletsHeuristic : TekHeuristic
    {
        public CoupledTripletsHeuristic() : base("Coupled Triplets", HeuristicAction.haExcludeValue)
        {
        }

        public override bool HeuristicApplies(TekBoard board, TekField field)
        {
            Region.Clear();
            Region.AddField(field);
            foreach (TekField field2 in field.Influencers)
            {
                Region.AddField(field2);
                foreach(TekField field3 in field.Influencers)
                    if (field != field2 && field != field3 && field2 != field3)
                        if (Region.IsTriplet(field3))
                        {
                            Region.AddField(field3);
                            AddHeuristicFields(Region.Fields);
                            AddValues(Region.GetTotalPossibleValues());
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
                Region.RemoveField(field2);
            }
            return false;
        }
    } // TripletHeuristic

    public class CascadingTripletsHeuristic : TekHeuristic
    { // logic is a bit suspect
        public CascadingTripletsHeuristic() : base("Coupled Triplets (cascade)", HeuristicAction.haExcludeValue)
        {
        }

        public override bool HeuristicApplies(TekBoard board, TekField field)
        {
            foreach (TekField field2 in field.Influencers)
                if (field2.PossibleValues.Count == 2 || field2.PossibleValues.Count == 3)
                {
                    Region.AddField(field2);
                    foreach (TekField field3 in field.CommonInfluencers(field2))
                        if (field3.PossibleValues.Count == 2)
                        {
                            Region.AddField(field3);
                            foreach (TekField field4 in field2.CommonInfluencers(field3))
                                if (field4 != field && field4.PossibleValues.Count == 2 && Region.IsTriplet(field4, false))
                                {
                                    foreach (int value in field.TotalPossibleValues(field2, field3))
                                        if (field.ValuePossible(value) && field2.ValuePossible(value) && field3.ValuePossible(value) && !field4.ValuePossible(value))
                                        {
                                            Region.AddField(field4);
                                            AddHeuristicFields(Region.Fields);
                                            AddAffectedField(field);
                                            AddValue(value);
                                            return true;
                                        }
                                }
                            Region.RemoveField(field3);
                        }
                    Region.RemoveField(field2);
                }
            return false;
        }
    } // TripletHeuristic2

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
                Region.Clear();
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
                            else Region.AddField(field2);
                        if (blocking)
                        {                            
                            AddHeuristicFields(Region.Fields);
                            AddAffectedField(field);
                            AddValue(value);                            
                        }
                    }
                }
            }
            return AffectedFields.Count > 0 && HeuristicValues.Count > 0;
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
                    {
                        Region.AddField(field1);
                        foreach (TekField field2 in field.Influencers)
                            if (field1 != field2)
                            {
                                Region.AddField(field2);
                                foreach (TekField field3 in field1.CommonInfluencers(field2))
                                    // and if there is a third field as well we might have the invalid configuration
                                    if (Region.IsInvalidThreePairs(field3))
                                    {
                                        Region.AddField(field3);
                                        AddHeuristicFields(Region.Fields);
                                        AddAffectedField(field);
                                        AddValue(field.Value);
                                        return true;
                                    }
                                Region.RemoveField(field2);
                            }
                    }
                }
                finally 
                {
                    field.Value = 0;
                }
            }
            Region.RemoveField(field);
            return false;
        }
    } // BlockingThreePairsHeuristic

    public class AlternatingChainHeuristic : TekHeuristic
    {
        TekChains Chains;
        public AlternatingChainHeuristic() : base("Alternating Chain", HeuristicAction.haExcludeValue)
        {
        }

        protected override void SetupApplies(TekBoard board)
        {
            Chains = new TekChains(board);
            using (StreamWriter sw = new StreamWriter("chains.dmp"))
            {
                Chains.Dump(sw);
            }
        }
        public override bool HeuristicApplies(TekBoard board, TekField field)
        {
            if (!Chains.HasChains())
                return false;

            List<List<TekField>> localChains = new List<List<TekField>>();
            List<TekField> localFields = new List<TekField>();
            foreach(TekField f in field.Influencers)
            {
                List<TekField> chain = Chains.FindChain(f);
                if (chain != null && Chains.FindChain(field) != chain)
                {
                    localChains.Add(chain);
                    localFields.Add(f);
                }
            }
            for (int i = 0; i < localChains.Count; i++)
            {
                int j = localChains.IndexOf(localChains[i], i + 1);
                if (j != -1)
                {
                    if (Chains.ComputeDistance(localFields[i], localFields[j]) % 2 == 1)
                    {
                        bool noInfluence = true;
                        foreach (int value in Chains.ChainValues(localChains[i]))
                            if (field.ValuePossible(value))
                                noInfluence = false;
                        if (!noInfluence)
                        {
                            AddHeuristicFields(Chains.ShortestRoute(localFields[i], localFields[j]));
                            AddAffectedField(field);
                            AddValues(Chains.ChainValues(localChains[i]));
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    } // AlternatingChainHeuristic

    public class ConflictingChainsHeuristic : TekHeuristic
    {
        TekChains Chains;

        public ConflictingChainsHeuristic() : base("Conflicting Chains", HeuristicAction.haExcludeValue)
        {
        }

        protected override void SetupApplies(TekBoard board)
        {
            Chains = new TekChains(board);
            using (StreamWriter sw = new StreamWriter("chains.dmp"))
            {
                Chains.Dump(sw);
            }
        }

        public override bool HeuristicApplies(TekBoard board, TekField field)
        {
            if (field.PossibleValues.Count != 2 || !Chains.HasChains())
                return false;
            List<TekField> chain = Chains.FindChain(field);
            if (chain == null)
                return false;
            foreach (TekField f in field.Influencers)
            {
                List<TekField> chain2 = Chains.FindChain(f);
                if (chain2 == null || chain2 == chain || Chains.CommonValues(chain, chain2).Count != 1)
                    continue;
                List<TekField> touchPoints1 = Chains.Intersection(chain2, chain);
                List<TekField> touchPoints2 = Chains.Intersection(chain, chain2);
                if (touchPoints1.Count == 2 && touchPoints2.Count == 2)
                {
                    List<TekField> chainFields1 = Chains.ShortestRoute(touchPoints1[0], touchPoints1[1]);
                    List<TekField> chainFields2 = Chains.ShortestRoute(touchPoints2[0], touchPoints2[1]);
                    if ((chainFields1.Count % 2 == 0) != (chainFields2.Count % 2 == 0))
                    {
                        AddValue(Chains.CommonValues(chain, chain2)[0]);
                        if (chainFields1.Count % 2 == 0)
                        {
                            AddAffectedFields(touchPoints2);
                            AddHeuristicFields(chainFields1);
                        }
                        else
                        {
                            AddAffectedFields(touchPoints1);
                            AddHeuristicFields(chainFields2);
                        }
                        return true;
                    }
                }
            }
            return false;
        }
    } // ConflictingChainsHeuristic

    public class CascadingTripletsHeuristic2 : TekHeuristic
    {// rework this
        public CascadingTripletsHeuristic2() : base("Cascading Triplets variation", HeuristicAction.haExcludeValue)
        {
        }

        public override bool HeuristicApplies(TekBoard board, TekField field)
        {
            List<TekField> emptyFields = field.area.GetEmptyFields();
            if (emptyFields.Count != 4)
                return false;
            emptyFields.Remove(field);
            List<int> TotalValues = emptyFields[0].TotalPossibleValues(emptyFields[1], emptyFields[2]);
            List<TekField> commonInfluencers = 
                emptyFields[0].CommonInfluencers(emptyFields[1], emptyFields[2]);
            commonInfluencers.Remove(field);
            int i = 0;
            while (i < commonInfluencers.Count)
                if (commonInfluencers[i].Value > 0)
                    commonInfluencers.RemoveAt(i);
                else i++;
            if (commonInfluencers.Count != 1)
                return false;
            foreach(int value in field.PossibleValues)
            {
                if (!commonInfluencers[0].PossibleValues.Contains(value))
                {
                    AddHeuristicField(field);
                    AddAffectedField(field);
                    AddValue(value);
                    return true;
                }
            }
            return false;
        }
    } // CascadingTripletsHeuristic

    public class TekHeuristics
    {
        List<TekHeuristic> Heuristics;
        
        public TekHeuristics()
        {
            Heuristics = new List<TekHeuristic>();
            Heuristics.Add(new SingleValueHeuristic());
            Heuristics.Add(new HiddenSingleValueHeuristic());
            Heuristics.Add(new CoupledPairHeuristic());
            Heuristics.Add(new HiddenPairHeuristic());
            Heuristics.Add(new CoupledTripletsHeuristic());
            Heuristics.Add(new BlockingHeuristic());
            Heuristics.Add(new BlockingThreePairsHeuristic());
            Heuristics.Add(new AlternatingChainHeuristic());
            Heuristics.Add(new CascadingTripletsHeuristic());
            Heuristics.Add(new ConflictingChainsHeuristic());
//            Heuristics.Add(new CascadingTripletsHeuristic());
        }

        public TekHeuristic FindHeuristic(TekBoard board)
        {
            board.AutoNotes = true;
            foreach(TekHeuristic heuristic in Heuristics)
            {
                if (heuristic.Applies(board))
                {
                    return heuristic;
                }
            }
            return null;
        }
    }

    
}
