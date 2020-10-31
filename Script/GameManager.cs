using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] PairPuyoCreator pairPuyoCreator = default;
    [SerializeField] PlayDataRenderer playDataRenderer = default;
    [SerializeField] GameObject gameOverPanel = default;
    [SerializeField] GameObject clearPanel = default;

    public void RenderPlayData(PlayData pd)
    {
        playDataRenderer.Render(pd);
    }

    public void Over()
    {
        gameOverPanel.SetActive(true);
    }

    public void Clear()
    {
        clearPanel.SetActive(true);
    }

    public void ReturnTitle()
    {
        SceneManager.LoadScene("Title");
    }

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void Start()
    {
        pairPuyoCreator.NewPairPuyo(true);
    }
}