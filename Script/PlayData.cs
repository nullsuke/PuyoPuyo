using System.Collections.Generic;
using UnityEngine;

public class PlayData
{
    public int Score { get; private set; }
    public int MaxChain { get; private set; }
    public int Vanished { get; private set; }

    public int UpdateData(List<Puyo> vanishable, int chainCount)
    {
        Score += Scorer.CountScore(vanishable, chainCount);
        MaxChain = Mathf.Max(MaxChain, chainCount);
        Vanished += vanishable.Count;

        return Score;
    }

    public void Clear()
    {
        Score = 0;
        MaxChain = 0;
        Vanished = 0;
    }
}
