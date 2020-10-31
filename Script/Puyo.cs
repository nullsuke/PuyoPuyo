using System;
using UnityEngine;

public class Puyo : MonoBehaviour
{
    [SerializeField] PuyoColor puyoColor = default;
    public enum PuyoColor { Red, Blue, Yellow, Green, Purple }
    private readonly static float acceleration = 0.25f;
    private static Field field;
    private Sprite[] sprites;
    private SingleMover mover;
    private SpriteRenderer spriteRenderer;
    private float speed;

    public PuyoColor Color { get; private set; }
    public float X { get => transform.position.x; }
    public float Y { get => transform.position.y; }
    public int IntX { get => Mathf.RoundToInt(transform.position.x); }
    public int IntY { get => Mathf.RoundToInt(transform.position.y); }
    public bool Droppable { get; set; }
    public bool Checked { get; set; }

    public void Setup(Field field)
    {
        Puyo.field = field;
        mover.Setup(field);

        speed = acceleration;
    }

    //自由落下
    public void FreeFall()
    {
        //フレーム毎に速度を上げる。最大１まで。
        speed = Math.Min(speed += acceleration, 1f);
        mover.Fall(speed);

        if (!mover.CanMove())
        {
            mover.AdjustToAbove();
            Fix();
            //落下できなくなったら、フラグを消す
            Droppable = false;
        }
    }

    public bool CanMove()
    {
        return mover.CanMove();
    }

    //消去
    public void Vanish()
    {
        var p = field[IntX, IntY];

        if (p == null) throw new Exception("can't vanish");

        Destroy(p.gameObject);
        field[IntX, IntY] = null;
    }

    //ぷよの画像を変える
    public void ChangeSprite(int stickType)
    {
        //組ぷよを回転させると、一緒にぷよも回転しているので角度を元に戻す
        transform.rotation = Quaternion.identity;
        spriteRenderer.sprite = sprites[stickType];
    }

    public void Fix()
    {
        if (field[IntX, IntY] != null)
        {
            Debug.Log(X + ", " + Y);
            throw new Exception("block overlap");
        }

        field[IntX, IntY] = this;
    }

    private void Awake()
    {
        mover = GetComponent<SingleMover>();
        Color = puyoColor;
        spriteRenderer = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>(Color.ToString() + "Puyoes");
    }
}