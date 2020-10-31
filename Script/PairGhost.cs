using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PairGhost
{
    private readonly List<Ghost> ghosts;

    public PairGhost()
    {
        ghosts = new List<Ghost>();
    }

    public void Add(Ghost ghost)
    {
        ghosts.Add(ghost);
    }

    public void Move()
    {
        ghosts.ForEach(g => g.Move());

        //Y座標でソート
        var ordered = ghosts.OrderBy(g => g.IntY).ToList<Ghost>();
        //ハードドロップ
        ordered.ForEach(g => g.HardDrop());

        //位置が重なった場合、上にあった方をずらす
        if (ordered[0].IntX == ordered[1].IntX)
        {
            ordered[1].transform.position += Vector3.up;
        }
    }

    public void Destory()
    {
        ghosts.ForEach(g => g.Destroy());
    }
}
