using UnityEngine;
using UnityEngine.UI;

public class PlayDataRenderer : MonoBehaviour
{
    [SerializeField] Text scoreText = default;
    [SerializeField] Text maxChainText = default;
    [SerializeField] Text vanishedText = default;

    public void Render(int score, int maxChain, int vanished)
    {
        scoreText.text = score.ToString();
        maxChainText.text = maxChain.ToString();
        vanishedText.text = vanished.ToString();
    }
}
