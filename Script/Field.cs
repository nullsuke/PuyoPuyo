using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    private static readonly Vector2Int overPoint1 = new Vector2Int(2, 12);
    private static readonly Vector2Int overPoint2 = new Vector2Int(2, 13);
    private static readonly Vector2Int overPoint3 = new Vector2Int(3, 12);
    private static readonly Vector2Int overPoint4 = new Vector2Int(3, 13);
    private static readonly int width = 6;
    private static readonly int height = 14;
    private static readonly Puyo[,] field = new Puyo[width, height];
    private static readonly Vanisher vanisher = new Vanisher(field);
    private static readonly int[,] dirs = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

    public int Width { get => width; }
    public int Height { get => height; }

    public Puyo this[int x, int y]
    {
        get => field[x, y];
        set => field[x, y] = value;
    }

    public void Fix()
    {
        foreach (Puyo p in transform.GetComponentsInChildren<Puyo>())
        {
            p.transform.SetParent(transform.parent);

            p.Fix();
        }
    }

    //空白の上にあるすべてをぷよを取得
    public List<Puyo> GetDroppablePuyoes()
    {
        var list = new List<Puyo>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (field[x, y] == null)
                {
                    PickDroppablePuyoes(x, y, list);
                    break;
                }
            }
        }

        return list;
    }

    public List<Puyo> GetVanishablePuyoes()
    {
        return vanisher.GetVanishablePuyoes();
    }

    //ぷよ同士をくっつける
    public void StickPuyoes()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var p = field[x, y];

                if (p == null) continue;

                int stickType = GetStickType(x, y);

                p.ChangeSprite(stickType);
            }
        }
    }

    public bool IsInside(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return false;
        }

        return true;
    }

    public bool IsOver()
    {
        return field[overPoint1.x, overPoint1.y] != null || 
            field[overPoint2.x, overPoint2.y] != null ||
            field[overPoint3.x, overPoint3.y] != null ||
            field[overPoint4.x, overPoint4.y] != null;
    }

    //空白の上にあるぷよを一列取得し、ぷよのあった部分を空白にする
    private void PickDroppablePuyoes(int x, int y, List<Puyo> list)
    {
        for (int i = y + 1; i < height; i++)
        {
            var p = field[x, i];

            if (p != null)
            {
                p.Droppable = true;
                list.Add(p);
                field[x, i] = null;
            }
        }
    }

    //ぷよがくっついている方向を整数で取得
    private int GetStickType(int x, int y)
    {
        var p = field[x, y];
        int stickType = 0;

        for (int i = 0; i < dirs.GetLength(0); i++)
        {
            int nx = x + dirs[i, 0];
            int ny = y + dirs[i, 1];

            if (!IsInside(nx, ny)) continue;

            var np = field[nx, ny];

            if (np != null && p.Color == np.Color)
            {
                //上下左右、それぞれに割り当てた数を足していく
                stickType += (int)Mathf.Pow(2, i);
            }
        }

        return stickType;
    }
}
