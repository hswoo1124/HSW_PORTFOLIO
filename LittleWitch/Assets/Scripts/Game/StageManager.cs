using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    #region 프로퍼티
    private int stageNum //현재 스테이지 번호
    {
        get; set;
    }
    private int enemyCount //스테이지에 남은 적 수
    {
        get; set;
    }
    #endregion

    #region serializeField 변수
    [SerializeField]
    GameObject dungeonDoor;
    [SerializeField]
    GameObject dungeonEntrance;
    [SerializeField]
    GameObject bossEntrance;
    [SerializeField]
    GameObject loadingUI;
    [SerializeField]
    LoadingBar loadingBar;
    [SerializeField]
    MonsterSpawn monsterSpawn;
    [SerializeField]
    MapGenerator mapGenerater;
    [SerializeField]
    GameObject enemyCountText;
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject playerOriginalPos;
    [SerializeField]
    GameObject exitTracker;
    [SerializeField]
    GameObject bossUI;
    #endregion
    Text enemyCountText_t;

    bool isSpawn = false;
    [SerializeField]
    bool isClear = false;
    bool startCount = false;
    public bool bossClear = false;
    float remainTime;
    float endingDelayT = 3f;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetStage(bool skipTutorial) //처음 시작시 튜토리얼 스킵 여부에 따른 스테이지 시작번호 설정
    {
        if (skipTutorial)
        {
            isClear = true;
            GoNextStage();
        }
        else
            stageNum = 0;
    }

    void InitiateMonsterCount() //남은 몬스터수 초기화 함수
    {
        enemyCount = 5; //임시로 20으로 디폴트값 설정
        ShowEnemyCountUI();
    }

    public void DecreaseEnemyCount()    //몬스터 처치시 몬스터클래스(AI)에서 해당 함수를 실행
    {
        enemyCount--;
        ShowEnemyCountUI();
        CheckStageClear();
    }

    void ShowEnemyCountUI()             //UI초기화 함수
    {
        Text enemyCountText_t = enemyCountText.GetComponent<Text>();

        enemyCountText_t.text = "남은 몬스터 수 " + enemyCount.ToString();
    }

    void CheckStageClear()      //모든적을 처치했는지 확인하는 함수
    {
        if(enemyCount == 0)
        {
            isClear = true;

            enemyCountText_t = enemyCountText.GetComponent<Text>();

            enemyCountText_t.text = "다음 스테이지로 이동하세요";

            if(stageNum < 4)
                exitTracker.SetActive(true);
        }
    }

    public void CountToNextStage()
    {
        if(isClear)
        {
            enemyCountText_t = enemyCountText.GetComponent<Text>();
            startCount = true;
            remainTime = 4f;
        }
    }

    public void GoNextStage()  //다음 스테이지로 이동하는 함수
    {
        startCount = false;
        if(isClear)
        {
            exitTracker.SetActive(false);
            stageNum++;

            if (stageNum == 4)   //보스스테이지 진입
            {
                bossUI.SetActive(true);
                enemyCountText.SetActive(false);
                SceneManager.LoadScene(2);
                return;
            }
            

            



            mapGenerater.GenerateMap();
            InitiateMonsterCount();


            Vector3 pop = new Vector3(playerOriginalPos.transform.position.x, playerOriginalPos.transform.position.y, playerOriginalPos.transform.position.z);
            player.transform.position = pop;

            dungeonEntrance.transform.position = bossEntrance.transform.position;
            dungeonEntrance.transform.rotation = bossEntrance.transform.rotation;

            loadingBar.sliderValue = 0;
            loadingUI.SetActive(true);
            dungeonDoor.SetActive(true);


            isSpawn = true;
            SpawnMonster();

            isClear = false;
        }

    }

    public void SpawnMonster()
    {
        if(isSpawn == true)
        {
            isSpawn = false;
            MonsterSpawn monsterSpawn = FindObjectOfType<MonsterSpawn>().GetComponent<MonsterSpawn>();
            monsterSpawn.stage = stageNum;
            monsterSpawn.SpawnMon();

            InitiateMonsterCount();
        }
       
    }

    public void ResetMap()  //맵의 랜덤구조를 새로고침하는 함수 (주로 맵 입구, 출구 오류시 발생)
    {
        mapGenerater.GenerateMap();
    }

    private void FixedUpdate()
    {
        if(startCount)
        {
            remainTime -= Time.deltaTime * 1f;
            enemyCountText_t.text = ((int)remainTime).ToString();
            
            if (remainTime < 1)
            {
                GoNextStage();
            }
        }

        BossClear();
    }

    public void GotoBossScene()
    {
        bossUI.SetActive(true);
        enemyCountText.SetActive(false);
        SceneManager.LoadScene(2);
    }

    public void BossClear()
    {
        if(bossClear == true)
        {
            stageNum = 0;
            endingDelayT -= Time.deltaTime * 1f;
            if (endingDelayT < 0)
            {
                bossClear = false;
                GameObject player = FindObjectOfType<CharacterStatus>().gameObject;
                //player.SetActive(false);
                Destroy(player);
                GameObject camera = GameObject.Find("Camera");
                Destroy(camera);
                GameObject sceneManager = FindObjectOfType<SceneContoller>().gameObject;
                Destroy(sceneManager);
                GameObject ingameUI = FindObjectOfType<UI_Manager>().gameObject;
                Destroy(ingameUI);
                GameObject fireManager = FindObjectOfType<ShootingManager>().gameObject;
                Destroy(fireManager);
                GameObject stageManager = FindObjectOfType<StageManager>().gameObject;
                Destroy(stageManager);
                SceneManager.LoadScene(3);
            }
        }
    }

    public void BackToMainmenu()
    {
         
    }

}
