using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tek1
{
    class TekChains
    {
        int[,] Connections;
        List<List<TekField>> Chains;
        List<int[,]> Distances;
        private List<TekField> ChainBackTracking = new List<TekField>();

        public TekChains(TekBoard board)
        {
            Connections = new int[board.Rows, board.Cols];
            Chains = new List<List<TekField>>();
            Chains.Add(null);
            Distances = new List<int[,]>();
            Distances.Add(null);
            // initialize the chains and connections
            InitializeChains(board);
            // make sure all fields in a chain are connected, else split the chain.
            CheckChains();
            ComputeDistances();
        }
        private void InitializeChains(TekBoard board)
        {
            for (int r = 0; r < board.Rows; r++)
                for (int c = 0; c < board.Cols; c++)
                {
                    if (board.values[r, c].Value == 0 && Connections[r, c] == 0 && board.values[r, c].PossibleValues.Count == 2)
                    {
                        TekField f = board.values[r, c];
                        foreach (TekField f2 in f.Influencers)
                            if (f.IsPair(f2))
                            {
                                Connections[r, c] = AddField(f, false);
                                Connections[f2.Row, f2.Col] = AddField(f2, false);
                            }
                    }
                }

        }

        private void CheckChains()
        {
            List<TekField> disconnects = new List<TekField>();
            for (int i = 1; i < Chains.Count; i++)
            {
                List<TekField> chain = Chains[i];
                TekField field0 = chain[0];
                for (int j = 1; j < chain.Count; j++)
                    if (!IsConnected(field0, chain[j]))
                        disconnects.Add(chain[j]);
            }
            foreach(TekField f in disconnects)
            {
                Connections[f.Row, f.Col] = AddField(f, true);
            }
            int ic = Chains.Count - 1;
            while (ic >= 1)
            {
                if (Chains[ic].Count == 1)
                {
                    TekField field = Chains[ic][0];
                    Connections[field.Row, field.Col] = 0;
                }
                if (Chains[ic].Count <= 1)
                    Chains.RemoveAt(ic);
                ic--;
            }
        }

        private int ComputeDistance(TekField field1, TekField field2, List<TekField> chain)
        {
            if (field1.Influencers.Contains(field2))
                return 1;
            else
            {
                int minDistance = 99;// Int32.MaxValue;
                foreach (TekField f in field1.Influencers)
                    if (chain.Contains(f) && !ChainBackTracking.Contains(f))
                    {
                        ChainBackTracking.Add(f);
                        int value = ComputeDistance(f, field2, chain);
                        if (value < minDistance)
                            minDistance = value;
                        //ChainBackTracking.Remove(f);
                    }
                return minDistance;
            }
        }

        private void ComputeDistances()
        {
            Distances.Clear();
            Distances.Add(null);
            for (int i = 1; i < Chains.Count; i++)
            {
                List<TekField> chain = Chains[i];
                int[,] table = new int[chain.Count, chain.Count];
                Distances.Add(table);
                ChainBackTracking.Clear();
                for (int j = 0; j < chain.Count; j++)
                {
                    TekField field = chain[j];
                    foreach (TekField f in field.Influencers)
                    {
                        int k = chain.IndexOf(f);
                        if (k != -1)
                        {
                            table[j, k] = 1;
                            table[k, j] = 1;
                        }
                    }
                    //    if (chain.)
                    //ChainBackTracking.Add(chain[j]);
                    //for (int k = j + 1; k < chain.Count; k++)
                    //{
                    //    table[j, k] = ComputeDistance(chain[j], chain[k], chain);
                    //    table[k, j] = table[j, k];
                    //}
                }
            }
        }
        private int AddField(TekField field, bool CheckConnect = true)
        {
            List<TekField> chain = FindChain(field, CheckConnect);
            if (chain == null)
            {
                chain = new List<TekField>();
                Chains.Add(chain);
            }
            if (!chain.Contains(field))
                chain.Add(field);
            return Chains.IndexOf(chain);
        }

        public List<TekField> FindChain(TekField field, bool CheckConnect = true)
        {
            for (int i = 1; i < Chains.Count; i++)
            {
                List<TekField> chain = Chains[i];
                if (chain.Count > 0 && chain[0].ValuesPossible(field.PossibleValues[0], field.PossibleValues[1]) && (!CheckConnect || IsConnected(chain[0], field)))
                    return chain;
            }                
            return null;
        }

        public bool IsConnected(TekField field1, TekField field2)
        {
            List<TekField> chain = FindChain(field1, false);
            foreach (TekField f in chain)
                if (f.Influencers.Contains(field2))
                    return true;
            return false; 
        }

        //public int numberOfSteps(TekField field1, TekField field2)
        //{
        //    if (!IsConnected(field1, field2))
        //        return -1;
        //    else
        //    {
        //        int result = 0;

        //    }
        //}

        public void Dump(StreamWriter sw)
        {
            for (int r = 0; r < Connections.GetLength(0); r++)
            {
                string s = "";
                for (int c = 0; c < Connections.GetLength(1); c++)
                    s = s + String.Format("{0,2}", Connections[r, c]);
                sw.WriteLine(s);
            }
            for (int i = 1; i < Chains.Count; i++)
            {
                sw.Write("chain {0} [{1},{2}]: ", i, Chains[i][0].PossibleValues[0], Chains[i][0].PossibleValues[1]);
                foreach (TekField field in Chains[i])
                    field.Dump(sw, TekField.FLD_DMP_INFLUENCERS | TekField.FLD_DMP_POSSIBLES);
                sw.WriteLine("end chain {0}", i);
            }

            sw.WriteLine("distances:");
            for (int i = 1; i < Distances.Count; i++)
            {
                sw.WriteLine("Chain {0}", i);
                int[,] table = Distances[i];
                for (int j = 0; j < table.GetLength(0); j++)
                {
                    string s = "";
                    for (int k = 0; k < table.GetLength(1); k++)
                        s = s + String.Format("{0,2} ", table[j, k]);
                    sw.WriteLine(s);
                }
            }
        }
    }
}
