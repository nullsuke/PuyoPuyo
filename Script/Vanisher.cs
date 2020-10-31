using System.Collections.Generic;

public class Vanisher
{
    private readonly int[,] dirs = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };
    private readonly Puyo[,] field;
    private readonly int width;
    private readonly int height;

    public Vanisher(Puyo[,] field)
    {
        this.field = field;
        width = field.GetLength(0);
        height = field.GetLength(1);
    }

    //消去できるぷよを取得
    public List<Puyo> GetVanishablePuyoes()
    {
        //消去可能リスト
        var vanishable = new List<Puyo>();
        ClearCheckedFlag();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var p = field[x, y];

                if (p == null || p.Checked) continue;
                
                var same = PickSameColor(p);

                //同色リストの中身が4以上なら消去可能リストに追加
                if (same.Count >= 4)
                {
                    vanishable.AddRange(same);
                }
            }
        }

        return vanishable;
    }

    //隣接した同色ぷよを取得
    private List<Puyo> PickSameColor(Puyo puyo)
    {
        //同色リスト
        var same = new List<Puyo>();

        puyo.Checked = true;

        //ぷよの上下左右を調べる
        for (int i = 0; i < dirs.GetLength(0); i++)
        {
            int nx = puyo.IntX + dirs[i, 0];
            int ny = puyo.IntY + dirs[i, 1];

            if (nx < 0 || nx >= width || ny < 0 || ny >= height) continue;

            var np = field[nx, ny];

            if (np == null || np.Checked || puyo.Color != np.Color) continue;
            
            var s = PickSameColor(np);

            same.AddRange(s);
        }

        same.Add(puyo);

        return same;
    }

    //全てのCheckedフラグを消す
    private void ClearCheckedFlag()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var p = field[x, y];
                if (p != null) p.Checked = false;
            }
        }
    }
}
