using UnityEngine;

public abstract class BaseMover : MonoBehaviour
{
    protected static Field field;
    protected static int width;
    protected static int height;
    private static readonly float dropSpeed = 0.9f;

    //PairMoverとSingleMoverで処理が違う
    public abstract bool CanMove();

    public void Setup(Field field)
    {
        BaseMover.field = field;
        width = field.Width;
        height = field.Height;
    }

    public void Fall(float speed)
    {
        transform.position += new Vector3(0, -speed, 0);
    }

    public void HardDrop()
    {
        var vec = new Vector3(0, -dropSpeed);

        do
        {
            transform.position += vec;
        } while (CanMove());

        AdjustToAbove();
    }

    //上へずらす
    public void AdjustToAbove()
    {
        var p = transform.position;
        int y = Mathf.FloorToInt(p.y + 1);

        transform.position = new Vector3(p.x, y);
    }
}
