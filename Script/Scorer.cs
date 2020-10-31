using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scorer
{
    private static readonly int[] chainBonus = { 0, 0, 8, 16, 32, 64, 96, 128, 160, 192, 224,
        256, 288, 320, 352, 384, 416, 448, 480, 512 };
    private static readonly int[] stickBonus =
        { 0, 0, 0, 0, 0, 2, 3, 4, 5, 6, 7};
    private static readonly int[] colorBonus = {0, 0, 3, 6, 12, 24 };

    public static int CountScore(List<Puyo> vanishable, int chainCount)
    {
        //連鎖ボーナス
        int chain = chainBonus[chainCount];
        
        var q = vanishable.GroupBy(p => p.Color)
            .Select(p => new { Color = p.Key, Number = p.Count() });

        int stick = 0;

        //連結ボーナス
        foreach (var p in q)
        {
            if (p.Number >= 11) stick += 10;
            else stick += stickBonus[p.Number];
        }

        int colorCount = q.Count();
        //色数ボーナス
        int color = colorBonus[colorCount];

        int total = Mathf.Max(chain + stick + color, 1);

        int score = vanishable.Count * 10 * total;
        
        return score;
    }
}
