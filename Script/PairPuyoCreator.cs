using System.Collections.Generic;
using UnityEngine;

public class PairPuyoCreator : MonoBehaviour
{
    [SerializeField] PairPuyo pairPuyoPrefab = default;

    private NextRenderer nextRenderer;
    private Queue<int[]> queue;

    public void NewPairPuyo(bool isFirst)
    {
        var nextIndexes = queue.Dequeue();
        var p = CreatePairPuyo(nextIndexes);

        if (isFirst) p.Initialize();

        SetNextIndexes();

        nextRenderer.Render(queue);
    }

    private PairPuyo CreatePairPuyo(int[] indexes)
    {
        var p = Instantiate(pairPuyoPrefab);

        p.SetUp(indexes);

        p.Falled += (s, e) =>
        {
            NewPairPuyo(false);
            ((PairPuyo)s).Destroy();
        };

        return p;
    }

    private void Awake()
    {
        nextRenderer = GetComponent<NextRenderer>();
        queue = new Queue<int[]>();

        for (int i = 0; i < 2; i++)
        {
            SetNextIndexes();
        }
    }

    private void SetNextIndexes()
    {
        var indexes = new int[2];

        for (int i = 0; i < 2; i++)
        {
            indexes[i] = UnityEngine.Random.Range(0, 5);
        }

        queue.Enqueue(indexes);
    }
}
