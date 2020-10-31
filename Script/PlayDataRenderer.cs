using UnityEngine;
using UnityEngine.UI;

public class PlayDataRenderer : MonoBehaviour
{
    [SerializeField] Text scoreText = default;
    [SerializeField] Text maxChainText = default;
    [SerializeField] Text vanishedText = default;

    public void Render(PlayData pd)
    {
        scoreText.text = pd.Score.ToString();
        maxChainText.text = pd.MaxChain.ToString();
        vanishedText.text = pd.Vanished.ToString();
    }
}
