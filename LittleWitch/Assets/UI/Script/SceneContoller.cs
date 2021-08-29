using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneContoller : MonoBehaviour
{
    public bool skipTutorial = false;

    GameObject player;
    GameObject startPosition_bossScene;

    void Start()
    {
        // SceneLoaded 를 사용한 경우
        // event에 함수를 등록한다.
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SkipTutorial(bool skipT)
    {
        skipTutorial = skipT;
    }

    void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)   //게임 시작시 씬이 로드되면 실행되는 이벤트
    {
        if(arg0.buildIndex == 1)    //싱글플레이 씬일 경우
        {
            StageManager stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
            stageManager.SetStage(skipTutorial);
        }
        else if (arg0.buildIndex == 2)    //첫번째 보스 씬일 경우
        {
            player = GameObject.Find("Player");
            startPosition_bossScene = GameObject.Find("StartPos_BossScene");
            player.transform.position = startPosition_bossScene.transform.position;
        }

        Debug.Log("Scene index number : " + arg0.buildIndex);
    }
    
}
