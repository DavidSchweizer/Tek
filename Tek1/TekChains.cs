using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tek1
{
    class TekChains
    {
        List<TekField>[,] Connections;
        List<List<TekField>> Chains;

        public TekChains(TekBoard board)
        {
            Connections = new List<TekField>[board.Rows, board.Cols];
            Chains = new List<List<TekField>>();
            // initialize the chains and connections
            for (int r = 0; r < board.Rows; r++)
                for (int c = 0; c < board.Cols; c++)
                {
                    if (board.values[r, c].Value == 0 && Connections[r, c] == null && board.values[r, c].PossibleValues.Count == 2)
                    {
                        TekField f = board.values[r, c];
                        foreach (TekField f2 in f.Influencers)
                            if (f.IsPair(f2))
                            {
                                Connections[r, c] = AddField(f);
                                Connections[f2.Row, f2.Col] = AddField(f2);
                            }
                    }
                }
        }

        List<TekField> AddField(TekField field)
        {
            List<TekField> chain = FindChain(field.PossibleValues[0], field.PossibleValues[1]);
            if (chain == null)
            {
                chain = new List<TekField>();
                Chains.Add(chain);
            }
            if (!chain.Contains(field))
                chain.Add(field);
            return chain;
        }

        List<TekField> FindChain(int value1, int value2)
        {
            foreach (List<TekField> chain in Chains)
                if (chain.Count > 0 && chain[0].ValuesPossible(value1, value2))
                    return chain;
            return null;
        }
    }
}
