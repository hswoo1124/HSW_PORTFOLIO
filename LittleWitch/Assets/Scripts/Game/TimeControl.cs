using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    #region ui_변수
    [SerializeField]
    GameObject pauseBtnUI;
    [SerializeField]
    GameObject resumeBtnUI;
    #endregion

    bool isPause = false;

    public bool playerDie = false;
    float delayT = 2f;

    public void PauseOrResumeGame()
    {
        if(isPause == true)
        {
            pauseBtnUI.SetActive(true);
            //resumeBtnUI.SetActive(false);
            Time.timeScale = 1f;
            isPause = false;
            return;
        }
        if(isPause == false)
        {
            pauseBtnUI.SetActive(false);
            //resumeBtnUI.SetActive(true);
            Time.timeScale = 0f;
            isPause = true;
            return;
        }
    }

    public void PlayerDie()
    {
        delayT -= Time.deltaTime * 1f;
        if(delayT < 0)
        {
            Time.timeScale = 0f;
            isPause = true;
        }

    }

    private void FixedUpdate()
    {
        if (playerDie)
            PlayerDie();
    }
}
