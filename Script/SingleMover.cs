using UnityEngine;
using System;

public class SingleMover : BaseMover
{
    public override bool CanMove()
    {
        var x = Mathf.RoundToInt(transform.position.x);
        var cy = Round4thDecimalPlace(transform.position.y);
        var y = Mathf.FloorToInt(cy);

        if (!field.IsInside(x, y)) return false;

        if (field[x, y] != null) return false;

        return true;
    }

    //小数第4位で四捨五入
    private float Round4thDecimalPlace(float n)
    {
        return (float)(Math.Round(n * 1000, MidpointRounding.AwayFromZero)) / 1000;
    }
}
