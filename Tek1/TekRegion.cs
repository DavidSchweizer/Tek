using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tek1
{
    class TekRegion : TekFields
    {
        public TekRegion() : base()
        {
        }

        public TekRegion(params TekField[] fields)
        {
            AddFields(fields.ToArray());
        }

        public void AddFields(params TekField[] fields)
        {
            foreach (TekField field in fields)
                AddField(field);
        }

        static public bool IsPair(TekField field1, TekField field2)
        // hidden pairs are ignored
        {
            if (!field1.Influencers.Contains(field2))
                return false;
            if (field1.PossibleValues.Count != 2 || field2.PossibleValues.Count != 2)
                return false;
            foreach (int value in field1.PossibleValues)
                if (!field2.ValuePossible(value))
                    return false;
            return true;
        }

        public bool IsPair()
        // hidden pairs are ignored
        {
            if (Fields.Count != 2)
                return false;
            return IsPair(Fields[0], Fields[1]);
        }

        static public bool IsTriplet(TekField field1, TekField field2, TekField field3, bool inSameArea = true)
        // hidden triplets are ignored
        {
            if (inSameArea && (field1.area != field2.area || field1.area != field3.area || field2.area != field3.area))
                return false;
            if (field1.TotalPossibleValues(field2, field3).Count != 3)
                return false;
            // 2 or 3 values per field
            if (field1.PossibleValues.Count < 2 || field1.PossibleValues.Count > 3)
                return false;
            if (field2.PossibleValues.Count < 2 || field2.PossibleValues.Count > 3)
                return false;
            if (field3.PossibleValues.Count < 2 || field3.PossibleValues.Count > 3)
                return false;

            return true;
        }

        public bool IsTriplet(bool inSameArea = true)
        // hidden triplets are ignored
        {
            if (Fields.Count != 3)
                return false;
            return IsTriplet(Fields[0], Fields[1], Fields[2]);
        }

        static public bool IsInvalidThreePairs(TekField field1, TekField field2, TekField field3)
        { 
            if (field1.CommonPossibleValues(field2, field3).Count != 2 || !IsPair(field1, field2) || !IsPair(field1, field3) || !IsPair(field2, field3))
                return false;
            return (field1.Influencers.Contains(field2) && field1.Influencers.Contains(field3) && field2.Influencers.Contains(field3));
        }

        public bool IsInvalidThreePairs()
        {
            if (Fields.Count != 3)
                return false;
            TekField field1 = Fields[0];
            TekField field2 = Fields[1];
            TekField field3 = Fields[1];

            if (field1.CommonPossibleValues(field2, field3).Count != 2 || !IsPair(field1, field2) || !IsPair(field1, field3) || !IsPair(field2, field3))
                return false;
            return (field1.Influencers.Contains(field2) && field1.Influencers.Contains(field3) && field2.Influencers.Contains(field3));
        }
    }
}
