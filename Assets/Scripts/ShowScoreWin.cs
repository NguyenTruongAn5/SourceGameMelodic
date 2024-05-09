using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowScoreWin : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtScore;
    [SerializeField] GameObject txtScoreGamePlay;
    [SerializeField] GameObject ShadowStar1;
    [SerializeField] GameObject ShadowStar2;
    [SerializeField] GameObject ShadowStar3;
    private void Update()
    {
        if (GameManager.Instance._isStatusGame == 3)
        {
            ShowScore();
            ShowStar();
        }
    }
    private void ShowScore()
    {
        txtScoreGamePlay.SetActive(false);
        txtScore.text = GameManager.Instance.score.ToString();
    }
    private void ShowStar()
    {
        int count = CreateStar.Instance.count;
        GameManager.Instance.SetCoin(count);
        if (count >= 1)
        {
            ShadowStar1.SetActive(true);
        }
        if (count >= 2)
        {
            ShadowStar2.SetActive(true);
        }
        if (count == 3)
        {
            ShadowStar3.SetActive(true);
        }
    }
}
