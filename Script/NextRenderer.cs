using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextRenderer : MonoBehaviour
{
    [SerializeField] Image nextCenterImage = default;
    [SerializeField] Image nextUpperImage = default;
    [SerializeField] Image next2CenterImage = default;
    [SerializeField] Image next2UpperImage = default;
    [SerializeField] Sprite[] puyoSprites = default;
    
    public void Render(Queue<int[]> queue)
    {
        var next = queue.ToArray()[0];
        nextCenterImage.sprite = puyoSprites[next[0]];
        nextUpperImage.sprite = puyoSprites[next[1]];

        var next2 = queue.ToArray()[1];
        next2CenterImage.sprite = puyoSprites[next2[0]];
        next2UpperImage.sprite = puyoSprites[next2[1]];
    }
}
