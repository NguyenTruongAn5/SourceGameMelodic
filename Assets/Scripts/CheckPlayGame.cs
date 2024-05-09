using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayGame : MonoBehaviour
{
    private void Update()
    {
        CheckIsPlayGame();
    }
    private void CheckIsPlayGame()
    {
        //Chỉnh 2 touch để có thể điều khiển 2 chạm
        if (Input.touchCount == 2)
        {
            if(GameManager.Instance._isStatusGame != 2)
            {
                GameManager.Instance.SetStatus(1);
            }
        }
    }
}
