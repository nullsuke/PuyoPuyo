using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PairPuyo : MonoBehaviour
{
    [SerializeField] Puyo[] puyoPrefabs = default;
    [SerializeField] Ghost[] ghostPrefabs = default;
    public event EventHandler<EventArgs> Falled;
    public Vector3 RotatePosition;
    private static readonly Vector3 startPosition = new Vector3(2, 12);
    private static readonly float maxSpeed = 0.5f;
    private static readonly float defaultSpeed = 0.05f;
    private static readonly int delayLimit = 30;
    private static readonly int timeLimit = 30;
    private static readonly int clearScore = 50000;
    private static GameManager gameManager;
    private static Field field;
    private static List<Puyo> droppablePuyoes = new List<Puyo>();
    private static float levelSpeed = defaultSpeed;
    private static float speed = levelSpeed;
    private static float timer = 0;
    private enum State { Normal, FreeFall, Vanish, Wait }
    private List<Puyo> puyoes;
    private PairMover mover;
    private PairGhost pairGhost;
    private State state;
    private bool isDelaying;
    private int delayCount;
    private int chainCount;

    public void SetUp(int[] indexes)
    {
        puyoes = new List<Puyo>();

        for (int i = 0; i < indexes.Length; i++)
        {
            var p = Instantiate(puyoPrefabs[indexes[i]], transform, false);
            p.Setup(field);
            puyoes.Add(p);

            var g = Instantiate(ghostPrefabs[indexes[i]], transform.parent, false);
            g.Setup(p, field);
            pairGhost.Add(g);
        }

        puyoes[0].transform.localPosition = new Vector2(0, 0);
        puyoes[1].transform.localPosition = new Vector2(0, 1);

        transform.position = startPosition;
        
        pairGhost.Move();

        state = State.Normal;
    }

    public void Initialize()
    {
        levelSpeed = defaultSpeed;
        speed = levelSpeed;

        PlayData.Clear();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    //落下終了後の処理
    protected virtual void OnFalled()
    {
        Falled?.Invoke(this, EventArgs.Empty);
    }

    private void Awake()
    {
        transform.position = startPosition;

        field = GetComponent<Field>();

        mover = GetComponent<PairMover>();
        mover.Setup(field);

        pairGhost = new PairGhost();

        delayCount = delayLimit;
        isDelaying = false;
        speed = levelSpeed;
        
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeLimit) SpeedUp();

        switch (state)
        {
            case State.Normal:
                CheckInput();
                mover.Fall(speed);

                if (!mover.CanMove())
                {
                    isDelaying = true;
                    delayCount--;

                    //上へずらす
                    mover.AdjustToAbove();

                    if (delayCount < 0)
                    {
                        //ぷよの固定
                        field.Fix();

                        if(field.IsOver())
                        {
                            this.enabled = false;
                            gameManager.Over();

                            return;
                        }

                        //落下可能なぷよを取得
                        droppablePuyoes = field.GetDroppablePuyoes();
                        
                        pairGhost.Destory();

                        //自由落下へ遷移
                        state = State.FreeFall;
                    }
                }
                else
                {
                    isDelaying = false;
                }

                break;

            case State.FreeFall:
                //落下可能なぷよを下にあるものから順に落下させる
                droppablePuyoes.OrderBy(p => p.IntY).ToList<Puyo>()
                    .ForEach(p => p.FreeFall());

                //落下できなくなったぷよは全てリストから消す
                droppablePuyoes.RemoveAll(p => !p.Droppable);

                if (droppablePuyoes.Count == 0)
                {
                    field.StickPuyoes();
                    //連鎖へ遷移
                    state = State.Vanish;
                }

                break;

            case State.Vanish:
                //消去可能ぷよを取得
                var vanishable = field.GetVanishablePuyoes();

                if (vanishable.Count > 0)
                {
                    StartCoroutine(CoroutineWait(0.1f, () =>
                    {
                        chainCount++;

                        PlayData.UpdateData(vanishable, chainCount);

                        gameManager.RenderPlayData(PlayData.Score, 
                            PlayData.MaxChain, PlayData.Vanished);

                        //ぷよを消去
                        vanishable.ForEach(p => p.Vanish());
                        
                        //落下可能ぷよを取得
                        droppablePuyoes = field.GetDroppablePuyoes();

                        StartCoroutine(CoroutineWait(0.5f, () =>
                        {
                            if (droppablePuyoes.Count > 0) state = State.FreeFall;
                            else
                            {
                                //通常へ遷移
                                TransitionToNormal();
                            }
                        }));
                    }));
                }
                else
                {
                    //通常へ遷移
                    TransitionToNormal();
                }

                break;

            case State.Wait:
                break;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            speed = levelSpeed;
        }
    }

    private void CheckInput()
    {
        KeyInputReceiver.Update();

        if (KeyInputReceiver.GetKeyLongDown(KeyCode.LeftArrow))
        {
            mover.MoveLeft();
            pairGhost.Move();
        }
        else if (KeyInputReceiver.GetKeyLongDown(KeyCode.RightArrow))
        {
            mover.MoveRight();
            pairGhost.Move();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (isDelaying) delayCount = -1;
            else speed = maxSpeed;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!isDelaying) mover.HardDrop();

            delayCount = -1;
            pairGhost.Move();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            mover.TurnLeft(RotatePosition);
            pairGhost.Move();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            mover.TurnRight(RotatePosition);
            pairGhost.Move();
        }
    }

    private IEnumerator CoroutineWait(float seconds, Action callback)
    {
        state = State.Wait;
        yield return new WaitForSeconds(seconds);
        callback();
    }

    private void TransitionToNormal()
    {
        if (PlayData.Score >= clearScore)
        {
            this.enabled = false;
            gameManager.Clear();
            
            return;
        }

        chainCount = 0;
        state = State.Normal;
        //この組ぷよ自身の消去と新しい組ぷよの作成
        OnFalled();
    }

    private void SpeedUp()
    {
        timer = 0;
        levelSpeed = Mathf.Min(levelSpeed + 0.01f, maxSpeed);
        speed = levelSpeed;
    }
}
