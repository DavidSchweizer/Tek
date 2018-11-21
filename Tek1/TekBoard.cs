using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tek1
{

    static public class Const
    {
        public const int MAXTEK = 5;
    }

    public class ETekFieldInvalid : Exception
    {
        public TekField Field;
        public int Value;
        public string Msg;
        public ETekFieldInvalid(string msg, TekField field, int value) : base("Invalid field value")
        {
            Field = field;
            Value = value;
            Msg = msg;
        }
    }

    public class TekField
    {
        static int __FieldIndex = 0;
        private int _FieldIndex;
        private bool _cascading = false;
        public int _value;
        public bool initial;
        public TekArea area;
        public List<TekField> neighbours;
        public List<TekField> Influencers;
        public List<int> PossibleValues;
        public List<int> ExcludedValues; // values excluded by heuristic solution process
        public List<int> Notes;
        private int _row, _col;
        public int FieldIndex { get { return _FieldIndex; } }
        public int Row { get { return _row; } }
        public int Col { get { return _col; } }
        public bool AutoNotes { get; set; }
        public bool EatExceptions = true;

        public TekField(int arow, int acol) 
        {
            _row = arow;
            _col = acol;
            _value = 0;
            _FieldIndex = __FieldIndex++;
            initial = false;
            neighbours = new List<TekField>();
            Influencers = new List<TekField>();
            PossibleValues = new List<int>();
            for (int i = 1; i <= Const.MAXTEK; i++)
                PossibleValues.Add(i);
            ExcludedValues = new List<int>();
            Notes = new List<int>();
            area = null;
            AutoNotes = false;
        }

        public int Value { get { return _value;  } set { SetValue(value); } }

        public void SetValue(int avalue)
        {
            if (!EatExceptions && (avalue < 0 || avalue > Const.MAXTEK))
                throw new ETekFieldInvalid("value", this, avalue);
            _value = avalue;
            UpdatePossibleValues(true);
        }

        public void ToggleValue(int avalue)
        {
            if (Value == avalue)
                SetValue(0);
            else
                SetValue(avalue);
        }

        public void ToggleNote(int anote)
        {
            if (!EatExceptions && (anote < 0 || anote > Const.MAXTEK))
                throw new ETekFieldInvalid("note", this, anote);
            if (Notes.Contains(anote))
                Notes.Remove(anote);
            else
                Notes.Add(anote);
        }

        public void SetDefaultNotes()
        {
            Notes.Clear();
            foreach (int value in PossibleValues)
                if (!ExcludedValues.Contains(value))
                    ToggleNote(value);
        }

        public void ClearNotes()
        {
            Notes.Clear();
        }

		public List<int> CopyNotes()
		{
			return new List<int>(Notes);
        }

        public void LoadNotes(List<int> values)
        {
            Notes.Clear();
            Notes.AddRange(values);
        }

        public void ExcludeValue(int value, bool onoff = true)
        {
            if (!onoff)
            {
                ExcludedValues.Remove(value);
            }
            else if (!ExcludedValues.Contains(value))
                ExcludedValues.Add(value);
            UpdatePossibleValues(false);
        }

        public List<int> CopyExcludedValues()
        {
            return new List<int>(ExcludedValues);
        }

        public void LoadExcludedValues(List<int> values)
        {
            ExcludedValues.Clear();
            ExcludedValues.AddRange(values);
        }

        public void UpdatePossibleValues(bool cascade = false)
        {
            if (_cascading) // protection against endless loops 
                return;
            PossibleValues.Clear();
            if (Value == 0)
            {
                for (int i = 1; i <= ((area == null) ? Const.MAXTEK : area.Fields.Count); i++)
                    if (!ExcludedValues.Contains(i))
                        PossibleValues.Add(i);
                foreach (TekField field in Influencers)
                    if (field.Value > 0)
                        PossibleValues.Remove(field.Value);
                if (PossibleValues.Count == 0 && !EatExceptions)
                    throw new ETekFieldInvalid("no possible values", this, 0);
            }
            if (AutoNotes)
                SetDefaultNotes();
            if (!cascade)
                return;
            _cascading = true;
            try
            {
                foreach (TekField field in Influencers)
                    field.UpdatePossibleValues();
            }
            finally
            {
                _cascading = false;
            }

        }
        public bool IsValid()
        {
            if (Value == 0)
                return PossibleValues.Count > 0;
            else
            {
                foreach (TekField field in Influencers)
                    if (field.Value == Value)
                        return false;
                return true;
            }
        }

        public void AddNeighbour(TekField f)
        {
            if (!neighbours.Contains(f)) // don't add more than once
                neighbours.Add(f);
        }

        public bool HasNeighbour(TekField f)
        {
            return neighbours.Contains(f);
        }

        public void AddInfluencer(TekField f)
        {
            if (!Influencers.Contains(f)) // don't add more than once
                Influencers.Add(f);
        }

        public void SetInfluencers()
        {
            Influencers.Clear();
            // add area
            if (area != null)
                foreach (TekField field in area.Fields)
                    if (field != this)
                        AddInfluencer(field);
            // add neighbours not in area
            foreach (TekField field in neighbours)
                AddInfluencer(field);
        }
        public List<TekField> CommonInfluencers(params TekField[] fields)
        {
            List<TekField> result = new List<TekField>();
            foreach(TekField f in Influencers)
            {
                bool isCommon = true;
                for (int i = 0; i < fields.Length && isCommon; i++)
                    if (fields.Contains(f) || !fields[i].Influencers.Contains(f))
                        isCommon = false;
                if (isCommon)
                    result.Add(f);
            }
            return result;
        }

        public List<TekField> CommonInfluencers(List<TekField> fields)
        {
            List<TekField> result = new List<TekField>();
            foreach (TekField f in Influencers)
            {
                bool isCommon = true;
                for (int i = 0; i < fields.Count && isCommon; i++)
                    if (fields.Contains(f) || !fields[i].Influencers.Contains(f))
                        isCommon = false;
                if (isCommon)
                    result.Add(f);
            }
            return result;
        }

        public bool ValuePossible(int value)
        {
            return PossibleValues.Contains(value);
        }

        public bool ValuesPossible(params int[] values)
        {
            foreach (int value in values)
                if (!ValuePossible(value))
                    return false;
            return true;
        }
        public List<int> CommonPossibleValues(params TekField[] fields)
        {
            List<int> result = new List<int>();
            for(int value = 1; value <= Const.MAXTEK; value++)
            {
                bool isCommon = this.ValuePossible(value);
                for (int i = 0; i < fields.Length && isCommon; i++)
                    if (!fields[i].ValuePossible(value))
                        isCommon = false;
                if (isCommon)
                    result.Add(value);
            }
            return result;
        }
        public List<int> TotalPossibleValues(params TekField[] fields)
        {
            List<int> result = new List<int>();
            for (int value = 1; value <= Const.MAXTEK; value++)
            {
                if (this.ValuePossible(value))
                    result.Add(value);
                else
                    foreach (TekField field in fields)
                        if (field.ValuePossible(value) && !result.Contains(value))
                            result.Add(value);
            }
            return result;
        }
        public List<int> TotalPossibleValues(List <TekField> fields)
        {
            List<int> result = new List<int>();
            for (int value = 1; value <= Const.MAXTEK; value++)
            {
                if (this.ValuePossible(value))
                    result.Add(value);
                else
                    foreach(TekField field in fields)
                       if(field.ValuePossible(value) && !result.Contains(value))
                          result.Add(value);
            }
            return result;
        }
        public string AsString(bool includeValue = false, bool includeArea = false, bool includeFieldIndex = false)
        {
            string result = String.Format("[{0},{1}", Row, Col);
            if (includeFieldIndex)
                result = result + String.Format("({0})", FieldIndex);
            result = result + "]";

            if (includeValue)
                result += String.Format(" value:{0}{1}", Value == 0 ? "-" : Value.ToString(), initial ? "i" : " ");
            if (includeArea)
                result += String.Format(" area: {0}", area == null ? "-" : area.AreaNum.ToString());
            return result;
        }
        
		public const uint FLD_DMP_NEIGHBOURS    = 1;
        public const uint FLD_DMP_INFLUENCERS   = 2;
        public const uint FLD_DMP_POSSIBLES     = 4;
        public const uint FLD_DMP_NOTES         = 8;
        public const uint FLD_DMP_EXCLUDES      = 16;
        public const uint FLD_DMP_ALL           = 65535;

        public void Dump(StreamWriter sw, uint flags = FLD_DMP_ALL)
        {
            sw.WriteLine("Field {0}", AsString(true, true));
            if ((flags & FLD_DMP_NEIGHBOURS) != 0)
            {
                sw.Write("Neighbours:");
                foreach (TekField t in neighbours)
                    sw.Write("{0} ", t.AsString());
                sw.WriteLine();
            }
            if ((flags & FLD_DMP_INFLUENCERS) != 0)
            {
                sw.Write("Influencrs:");
                foreach (TekField t in Influencers)
                    sw.Write("{0} ", t.AsString());
                sw.WriteLine();
            }
            if ((flags & FLD_DMP_POSSIBLES) != 0)
            {
                sw.Write("Poss. Vals:");
                for (int i = 0; i < PossibleValues.Count; i++)
                    sw.Write("{0} ", PossibleValues[i]);
                sw.WriteLine();
            }
            if ((flags & FLD_DMP_NOTES) != 0)
            {
                sw.Write("Notes     :");
                for (int i = 0; i < Notes.Count; i++)
                    sw.Write("{0} ", Notes[i]);
                sw.WriteLine();
            }
            if ((flags & FLD_DMP_EXCLUDES) != 0)
            {
                sw.Write("Excludes  :");
                for (int i = 0; i < ExcludedValues.Count; i++)
                    sw.Write("{0} ", ExcludedValues[i]);
                sw.WriteLine();
            }
        }
    } // TekField

    public class TekFields
    {
        public List<TekField> Fields;
        public TekFields()
        {
            Fields = new List<TekField>();
        }

        public virtual bool IsEqual(TekFields fields2)
        {
            if (Fields.Count != fields2.Fields.Count)
                return false;
            foreach (TekField field in Fields)
                if (!fields2.Fields.Contains(field))
                    return false;
            return true;
        }
            
        public virtual void AddField(TekField f)
        {
            if (Fields.Contains(f)) // don't add more than once
                return;
            Fields.Add(f);
            Sort();
        }

        public List<TekField> GetCommonInfluencers()
        {
            if (Fields.Count > 0)
                return Fields[0].CommonInfluencers(Fields.GetRange(1, Fields.Count - 1));
            else
                return new List<TekField>();
        }

        public List<int> GetTotalPossibleValues()
        {
            if (Fields.Count > 0)
                return Fields[0].TotalPossibleValues(Fields.GetRange(1, Fields.Count - 1));
            else
                return new List<int>();
        }

        public bool FieldsAreConnected()
        {
            if (Fields.Count() <= 1)
                return true;
            foreach (TekField f in Fields)
            {
                bool hasOne = false;
                foreach (TekField f2 in Fields)
                    if (f2 != f && f.HasNeighbour(f2) && (f.Col == f2.Col || f.Row == f2.Row))
                    {
                        hasOne = true;
                        break;
                    }
                if (!hasOne)
                    return false;
            }
            return true;
        }

        public virtual string AsString()
        {
            string result = String.Format("Region: ");
            foreach (TekField f in Fields)
                result = result + f.AsString();
            return result;
        }

        public void Dump(StreamWriter sw)
        {
            sw.WriteLine(AsString());
        }

        public int MaxValue()
        {
            return (Fields.Count < Const.MAXTEK ? Fields.Count : Const.MAXTEK);
        }

        public void Sort()
        {
            Fields.Sort(new TekFieldComparer2());
        }

        public Dictionary<int, List<TekField>> GetFieldsForValues()
        {
            Dictionary<int, List<TekField>> result = new Dictionary<int, List<TekField>>();
            for (int i = 1; i <= MaxValue(); i++)
                result.Add(i, new List<TekField>());
            foreach (TekField f in Fields)
                if (f.Value > 0)
                    result[f.Value].Add(f);
                else
                    foreach (int value in f.PossibleValues)
                        result[value].Add(f);
            return result;
        }       
    } // TekFields

    public class TekArea : TekFields 
    {
        public int AreaNum;
        public TekArea(int anum) : base()
        {
            AreaNum = anum;
        }

        public List<TekArea> GetAdjacentAreas()
        {
            List<TekArea> result = new List<TekArea>();
            foreach (TekField field in Fields)
            {
                foreach (TekField Neighbour in field.neighbours)
                    if (Neighbour.area != null && this != Neighbour.area && !result.Contains(Neighbour.area))
                    {
                        result.Add(Neighbour.area);
                    }
            }
            return result;
        }

        public void SetInfluencers()
        {
            foreach (TekField f in Fields)
                f.SetInfluencers();
        }

        public override void AddField(TekField f)
        {
            base.AddField(f);
            if (f.area != null)
                return; // or exception
            f.area = this;
            SetInfluencers();            
        }

        public override string AsString()
        {
            string result = String.Format("Area {0}:", AreaNum);
            foreach (TekField f in Fields)
                result = result + f.AsString();
            return result;
        }

    } // TekArea

    public class TekBoard
    {
        public TekField[,] values;
        public List<TekArea> areas;
        private int _rows, _cols;
        public int Rows { get { return _rows; } }
        public int Cols { get { return _cols; } }

        private bool _AutoNotes;
        public bool AutoNotes { get { return _AutoNotes; } set { _AutoNotes = value; foreach (TekField field in values) field.AutoNotes = value;  } }
        private bool _EatExceptions;
        public bool EatExceptions { get { return _EatExceptions; } set { _EatExceptions = value; foreach (TekField field in values) field.EatExceptions = value; } }

        public TekBoard(int rows, int cols)
        {
            areas = new List<TekArea>();
            _rows = rows;
            _cols = cols;
            initValues(rows, cols);
            setNeighbours();
            EatExceptions = true;
        }      

        private void initValues(int rows, int cols)
        {
            values = new TekField[rows, cols];
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                {
                    values[r, c] = new TekField(r, c);
                }
        }

        private void setNeighbours()
        {
            // set neighbours
            for (int r = 0; r < Rows; r++)
                for (int r1 = r - 1; r1 <= r + 1; r1++)
                {
                    if (r1 >= 0 && r1 < Rows)
                    {
                        for (int c = 0; c < Cols; c++)
                            for (int c1 = c - 1; c1 <= c + 1; c1++)
                            {
                                if ((r1 != r || c1 != c) && c1 >= 0 && c1 < Cols)
                                    values[r, c].AddNeighbour(values[r1, c1]);
                            }
                    }
                }
        }

        public int[,] CopyValues()
        {
            int[,] result = new int[Rows, Cols];
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                    result[r, c] = values[r, c].Value;
            return result;
        }

        
        public void LoadValues(int[,] newValues)
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                    values[r, c].Value = newValues[r, c];
        }

        public List<int>[,] CopyNotes()
        {
            List<int>[,] result = new List<int>[Rows, Cols];
            foreach (TekField field in values)
            {
                result[field.Row, field.Col] = field.CopyNotes();
            }
            return result;
        }

        public void LoadNotes(List<int>[,] notes)
        {
            foreach (TekField field in values)
            {
                field.LoadNotes(notes[field.Row, field.Col]);
            }
        }
        public List<int>[,] CopyExcludedValues()
        {
            List<int>[,] result = new List<int>[Rows, Cols];
            foreach (TekField field in values)
            {
                result[field.Row, field.Col] = field.CopyExcludedValues();
            }
            return result;
        }

        public void LoadExcludedValues(List<int>[,] excludedvalues)
        {
            foreach (TekField field in values)
            {
                field.LoadExcludedValues(excludedvalues[field.Row, field.Col]);
            }
        }

        public bool IsInRange(int row, int col)
        {
            return row >= 0 && row < Rows && col >= 0 && col < Cols;
        }

        public void ResetValues()
        {
            foreach (TekField field in values)
                if (!field.initial)
                {
                    field.Value = 0;
                    field.Notes.Clear();
                    field.ExcludedValues.Clear();
                }
        }

        public TekArea DefineArea(List<TekField> list)
        {
            TekArea result = new TekArea(areas.Count());
            foreach (TekField f in list)
                result.AddField(f);
            areas.Add(result);
            return result;
        }

        public void DeleteArea(TekArea area)
        {
            foreach (TekField field in area.Fields)
            {
                field.area = null;
            }
            foreach (TekField field in area.Fields)
            {
                field.SetInfluencers();
            }
            areas.Remove(area);
        }

        public bool IsSolved()
        {
            foreach (TekField field in values)
                if (field.Value == 0)
                    return false;
                else foreach (TekField field2 in field.Influencers)
                        if (field2.Value == field.Value)
                            return false;
            return true;
        }

        private TekField[,] CopyFields()
        {
            TekField[,] result = new TekField[Rows, Cols];
            for (int row = 0; row < Rows; row++)
                for (int col = 0; col < Cols; col++)
                {
                    result[row, col] = new TekField(row, col);
                    result[row, col].Value = values[row, col].Value;
                    result[row, col].initial = values[row, col].initial;
                }
            return result;

        }

        public void Resize(int rows, int cols)
        {
            TekField [,] oldValues = CopyFields();

            foreach (TekArea area in areas)
                DeleteArea(area);
            areas.Clear();

            _rows = rows;
            _cols = cols;
            initValues(rows, cols);
            setNeighbours();
            for (int row = 0; row < Math.Min(Rows-1,oldValues.GetLength(0)); row++)
                for (int col = 0; col < Math.Min(Cols - 1, oldValues.GetLength(1)); col++)
                {
                    values[row, col].Value = oldValues[row, col].Value;
                    values[row, col].initial = oldValues[row, col].initial;
                }
        }

        public void Dump(StreamWriter sw)
        {
            for (int r = 0; r < _rows; r++)
                for (int c = 0; c < _cols; c++)
                {
                    values[r, c].Dump(sw);
                }
            foreach (TekArea a in areas)
            {
                a.Dump(sw);
            }
        }

        public void Dump(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
                Dump(sw);
        }

        public List<string> ValidAreasErrors()
        {
            List<string> result = new List<string>();
            // every field is part of an area
            foreach(TekField field in values)
            {
                if (field.area == null)
                    result.Add(String.Format("Field ({0},{1}) is not part of an area", field.Row, field.Col));
            }
            // every area contains only adjacent fields
            foreach(TekArea area in areas)
            {
                if (!area.FieldsAreConnected())
                    result.Add(String.Format("Area {0} is not valid", area.AsString()));
            }
            return result;
        }

        public bool IsValidAreas()
        {
            return ValidAreasErrors().Count == 0;
        }

        public void SetDefaultNotes()
        {
            foreach (TekField field in values)
                field.SetDefaultNotes();
        }

        
    } // TekBoard
 
    public class TekBoardParser
    {
        const string COMMENTPATTERN = @"#.*";
        private Regex commentPattern;

        const string SIZEPATTERN = @"size=(?<rows>[1-9]\d*),(?<cols>[1-9]\d*)";
        const string SIZEFORMAT = @"size={0},{1}";
        private Regex sizePattern;
        const string AREAPATTERN1 = @"(area=)(\((?<row>\d+),(?<col>\d+)\))(?<rest>\(.*\))?";
        const string AREAPATTERN2 = @"(\((?<row>\d+),(?<col>\d+)\))";
        const string AREAFORMAT1 = @"area=";
        const string AREAFORMAT2 = @"({0},{1})";
        private Regex areaPattern1, areaPattern2;
        const string VALUEPATTERN = @"value=\((?<row>\d+),(?<col>\d+):(?<value>[1-5])(?<initial>i)?\)";
        const string VALUEFORMAT = @"value=({0},{1}:{2}{3})";
        private Regex valuePattern;
        const string NOTESPATTERN1 = @"notes=\((?<row>\d+),(?<col>\d+)\)(?<value>[1-5])+(?<rest>(.*))?";
        const string NOTESPATTERN2 = @"(?<value>[1-5])+";
        const string NOTESFORMAT1 = @"notes=({0},{1})";
        const string NOTESFORMAT2 = @"{0} ";
        const string EXCLUDESPATTERN1 = @"excludes=\((?<row>\d+),(?<col>\d+)\)(?<value>[1-5])+(?<rest>(.*))?";
        const string EXCLUDESPATTERN2 = @"(?<value>[1-5])+";
        const string EXCLUDESFORMAT1 = @"excludes=({0},{1})";
        const string EXCLUDESFORMAT2 = @"{0} ";
        private Regex notesPattern1, notesPattern2;
        private Regex excludesPattern1, excludesPattern2;

        public TekBoardParser()
        {
            commentPattern = new Regex(COMMENTPATTERN);
            sizePattern = new Regex(SIZEPATTERN);
            areaPattern1 = new Regex(AREAPATTERN1);
            areaPattern2 = new Regex(AREAPATTERN2);
            valuePattern = new Regex(VALUEPATTERN);
            notesPattern1 = new Regex(NOTESPATTERN1);
            notesPattern2 = new Regex(NOTESPATTERN2);
            excludesPattern1 = new Regex(EXCLUDESPATTERN1);
            excludesPattern2 = new Regex(EXCLUDESPATTERN2);
        }

        private void ParseError(string format, params object[] list)
        {
            throw new Exception(String.Format(format, list));
        }

        public TekBoard Import(string filename)
        {
            TekBoard board = null;
            using (StreamReader sr = new StreamReader(filename))
            {
                if ((board = ParseStream(sr)) == null)
                {
                    ParseError("invalid file {0}", filename);
                }
            }
            return board;
        }

        public void Export(TekBoard board, string filename)
        {
            using (StreamWriter wr = new StreamWriter(filename))
            {
                _Export(board, wr);
            }    
        }

        private bool ParseComment(string input)
        {
            Match match = commentPattern.Match(input);
            return match.Success;
        }

        private TekBoard ParseSize(string input)
        {
            int rows=0, cols=0;
            Match match = sizePattern.Match(input);

            if (match.Success &&
                Int32.TryParse(match.Groups["rows"].Value, out rows) &&
                Int32.TryParse(match.Groups["cols"].Value, out cols))
            {
                if (rows <= 0 || cols <= 0)
                    ParseError("Invalid size line {0}: ({1},{2})", input, rows, cols);
                return new TekBoard(rows, cols);
            }
            else
            {
                return null;
            }
        }

        private bool ParseAreaField(string rowS, string colS, TekBoard board, List<TekField> fields)
        {
            int row, col;
            if (Int32.TryParse(rowS, out row) && Int32.TryParse(colS, out col))
            {
                if (board.IsInRange(row, col))
                {
                    fields.Add(board.values[row, col]);
                    return true;
                }
            }
            return false;
        }
        private bool ParseArea(string input, TekBoard board)
        {
            List<TekField> fields = new List<TekField>();
            Match match = areaPattern1.Match(input);
            if (match.Success)
            {
                if (!ParseAreaField(match.Groups["row"].Value, match.Groups["col"].Value, board, fields))
                    ParseError("Invalid field in area line {0}: ({1},{2})", input, match.Groups["row"].Value, match.Groups["col"].Value);
                match = areaPattern2.Match(match.Groups["rest"].Value);
                while (match.Success)
                {
                    if (!ParseAreaField(match.Groups["row"].Value, match.Groups["col"].Value, board, fields))
                        ParseError("Invalid field in area line {0}: ({1},{2})", input, match.Groups["row"].Value, match.Groups["col"].Value);
                    match = match.NextMatch();
                }
            }
            if (fields.Count > 0)
            {
                board.DefineArea(fields);
                return true;
            }
            else
                return false;
        }

        private bool ParseValue(string input, TekBoard board)
        {
            int row, col, value;
            Match match = valuePattern.Match(input);
            if (match.Success &&
                Int32.TryParse(match.Groups["row"].Value, out row) &&
                Int32.TryParse(match.Groups["col"].Value, out col) &&
                Int32.TryParse(match.Groups["value"].Value, out value)
                )
            {
                if (!board.IsInRange(row, col) || value <= 0 || value > Const.MAXTEK)
                {
                    ParseError("Invalid value line {0}: ({1},{2}", input, row, col);
                }
                TekField field = board.values[row, col];
                field.Value = value;
                field.initial = match.Groups["initial"].Value == "i";
                return true;
            }
            else
                return false;
        }

        private enum MultiValues { mvNotes, mvExcludes };
        static string[] MultiValueString = { @"notes", @"excludes" };

        private bool ParseMultiValues(string input, TekBoard board, MultiValues mvType)
        {
            int row, col, value;
            TekField field = null;
            Regex pattern1 = null, pattern2 = null;


            switch (mvType)
            {
                case MultiValues.mvNotes:
                    pattern1 = notesPattern1;
                    pattern2 = notesPattern2;
                    break;
                case MultiValues.mvExcludes:
                    pattern1 = excludesPattern1;
                    pattern2 = excludesPattern2;
                    break;
            }

            Match match = pattern1.Match(input);
            if (match.Success)
            {
                if (field == null && Int32.TryParse(match.Groups["row"].Value, out row) &&
                    Int32.TryParse(match.Groups["col"].Value, out col))
                {
                    if (!board.IsInRange(row, col))
                        ParseError("Invalid field in {0} line {1}: ({2},{3})", MultiValueString[(int)mvType], input, row, col);
                    field = board.values[row, col];
                }
                if (field != null && Int32.TryParse(match.Groups["value"].Value, out value))
                {
                    switch (mvType)
                    { 
                        case MultiValues.mvNotes:
                            field.ToggleNote(value);
                            break;                      
                        case MultiValues.mvExcludes:
                            field.ExcludeValue(value);
                            break;
                    }
                    match = pattern2.Match(match.Groups["rest"].Value);
                    while (match.Success)
                    {
                        if (Int32.TryParse(match.Groups["value"].Value, out value))
                        {
                            switch (mvType)
                            {
                                case MultiValues.mvNotes:
                                    field.ToggleNote(value);
                                    break;
                                case MultiValues.mvExcludes:
                                    field.ExcludeValue(value);
                                    break;
                            }
                            match = match.NextMatch();
                        }
                        else
                            ParseError("Invalid value in {0} line {1}: ({2})", MultiValueString[(int)mvType], input, match.Groups["value"].Value);
                    }
                }
            }
            return field != null;
        }

        private bool ParseNotes(string input, TekBoard board)
        {
            return ParseMultiValues(input, board, MultiValues.mvNotes);
        }

        private bool ParseExcludes(string input, TekBoard board)
        {
            return ParseMultiValues(input, board, MultiValues.mvExcludes);
        }

        private void UpdatePossibleValues(TekBoard board)
        {
            foreach (TekField field in board.values)
                field.UpdatePossibleValues(false);
        }

        private TekBoard ParseStream(StreamReader sr)
        {
            string s;
            TekBoard board = null;
            while (!sr.EndOfStream)
            {
                s = sr.ReadLine();
                if (s.Trim() == "")
                    continue;
                else
                {
                    if (ParseComment(s))
                        continue;                   
                    if (board == null)
                    {
                        board = ParseSize(s);
                    }
                    else
                    {
                        if (!(ParseNotes(s, board) || ParseExcludes(s, board) || ParseArea(s, board) || ParseValue(s, board)))
                        {
                            ParseError("Error parsing line {0}", s);
                        }
                    }
                }
            }
            if (board != null)
            {
                UpdatePossibleValues(board);
            }
            return board;
        }
        private void ExportValue(TekField field, StreamWriter wr)
        {
            wr.WriteLine(VALUEFORMAT, field.Row, field.Col, field.Value, field.initial ? "i" : "");
        }

        private void ExportArea(TekArea area, StreamWriter wr)
        {
            wr.Write(AREAFORMAT1);
            foreach (TekField field in area.Fields)
            {
                wr.Write(AREAFORMAT2, field.Row, field.Col);
            }
            wr.WriteLine();
        }

        private void ExportNotes(TekField field, StreamWriter wr)
        {
            wr.Write(NOTESFORMAT1, field.Row, field.Col);
            foreach (int value in field.Notes)
            {
                wr.Write(NOTESFORMAT2, value);
            }
            wr.WriteLine();
        }
        private void ExportExcludes(TekField field, StreamWriter wr)
        {
            wr.Write(EXCLUDESFORMAT1, field.Row, field.Col);
            foreach (int value in field.ExcludedValues)
            {
                wr.Write(EXCLUDESFORMAT2, value);
            }
            wr.WriteLine();
        }

        private void _Export(TekBoard board, StreamWriter wr)
        {
            wr.WriteLine(SIZEFORMAT, board.Rows, board.Cols);
            foreach (TekArea area in board.areas)
            {
                ExportArea(area, wr);
            }
            foreach (TekField value in board.values)
            {
                if (value.Value > 0)
                    ExportValue(value, wr);
                if (value.Notes.Count > 0)
                    ExportNotes(value, wr);
                if (value.ExcludedValues.Count > 0)
                    ExportExcludes(value, wr);
            }
        }
    } // TekBoardParser
} // namespace Tek1
