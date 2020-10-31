using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Puyo puyo;
    private SingleMover mover;

    public int IntX { get => Mathf.RoundToInt(transform.position.x); }
    public int IntY { get => Mathf.RoundToInt(transform.position.y); }

    public void Setup(Puyo puyo, Field field)
    {
        this.puyo = puyo;
        mover.Setup(field);
    }

    //実体と同じ位置に移動
    public void Move()
    {
        transform.position = puyo.transform.position;
    }

    public void HardDrop()
    {
        mover.HardDrop();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        mover = GetComponent<SingleMover>();
    }
}
