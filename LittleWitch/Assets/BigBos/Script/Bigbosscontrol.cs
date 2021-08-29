using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bigbosscontrol : MonoBehaviour
{
    #region 게임 오브젝트
    public GameObject player;           //플레이어 게임오브젝트 등록
    public GameObject meteoskill;      //메테오 스킬 임펙트 등록
    public GameObject punchSkill;       //펀치 스킬 오브젝트 등록
    public GameObject breath;           //브레스 스킬 오브젝트 등록

    public GameObject faceFront;
    public GameObject faceLeft90;
    public GameObject faceLeft45;
    public GameObject faceRight90;
    public GameObject faceRight45;
    #endregion

    #region 보스능력치 변수
    [SerializeField]
    public float bossHp;


    #endregion

    #region 피해관련 변수
    SkillProjectile sp;
    bool deathCheck = true;
    bool isHit = false;
    bool isSplash = false;
    float skillProjectileDamage;
    float skillImpactDamage;
    #endregion
    [SerializeField]
    GameObject hpSlider;

    bool hasTriggered = false;

    Transform target;                   //플레이어 위치정보
    Animator bossanimetor;              //애니메이션 담당
    
    public int faceControler = 1;       // 얼굴 시선 고정 유무 : 0 = 고정  1 = 해제
    public int breathControler;

    int breathAngle;                    //브레스 발사 구역 저장 Breath스크립트에 보낼 것
    bool isAttack = false;              //공격 제어
    public int bossphase = 2;           //보스 페이즈
    int bosshp = 1000;                  //보스 체력
    int skillrandom;                    //스킬 랜덤값 중 최대값

    bool charSide = false;              //플레이어 사이드 체크

    [SerializeField]
    GameObject deathBody;
    [SerializeField]
    GameObject bossChar;

    // Awake는 모든 Start함수 앞에 호출됩니다. 따라서 스크립트의 초기화 순서를 정할 수 있습니다
    private void Awake() 
    {
        hpSlider = GameObject.Find("Boss_HP");
        bossanimetor = GetComponent<Animator>();        //현재 오브젝트의 컴퍼넌트 애니메이션 등록
        SetMaxHp();
    }

    // Start는 스크립트 시작시 한번만 호출됩니다.
    void Start()
    {
        player = GameObject.Find("Player");
        target = player.transform;              //플레이어 위치 정보 등록
    }

    // Update는 매 프레임마다 호출됩니다.
    void FixedUpdate()
    {
        BossDie();
        ShowHpToSlider();

        if (faceControler == 1) // 얼굴 시선 고정 해제 시
        {
            FaceControl(); // 얼굴 시선 바꾼다.
        }

        if (isAttack == false)               //isAttack이 false일 때 공격 선택 및 공격
        {
            UsingSkill();
        }
    }

    IEnumerator ChooseSkill()
    {
        float bossatktime = Random.Range(3.0f, 5.0f);                    //공격 속도 랜덤 (3초에서 5초) 애니메이션 실행 유무를 유의하여 설정
        //Debug.Log("보스 공격 속도 : " + bossatktime.ToString());        
        isAttack = true;                                                //공격 안함
        yield return new WaitForSeconds(bossatktime);                   //공격 속도만큼 공격안하고 기다린다.
        isAttack = false;                                               //공격 시작
        StopCoroutine(ChooseSkill());                                   //코루틴 멈춤 (공격 일시정지)
    }

    void BossDie()
    {
        if(bossHp < 1)
        {
            bossChar.SetActive(false);
            deathBody.SetActive(true);
            StageManager sm = FindObjectOfType<StageManager>().GetComponent<StageManager>();
            sm.bossClear = true;
        }
    }

    void UsingSkill()
    {
        int SkillNum = 0;
        if (charSide == true)
        {
            SkillNum = Random.Range(2, 5);
        }
        else
        {
            if (bossphase == 1)                             //보스 페이즈 
                skillrandom = 2;                            //보스 사용스킬 갯수
            else if (bossphase == 2)                         //보스 페이즈 
                skillrandom = 5;                             //보스 사용스킬 갯수

            SkillNum = Random.Range(0, skillrandom);

        }
        //int SkillNum = 2;

        Debug.Log("스킬 번호 : " + SkillNum.ToString());        //현재 사용 스킬 확인

        if (SkillNum == 0)                                      //스킬 0번 한손 치기
        {
            PunchSkill();
        }
        else if (SkillNum == 1)                              //스킬 1번 양손 치기
        {
            bossanimetor.SetTrigger("Attack3");
            //bossanimetor.SetBool("Attack3", true);
        }
        else if (SkillNum == 2)                             //스킬 2번 메테오
        {
            bossanimetor.SetTrigger("Attack4");             //애니메이션
            bossanimetor.SetTrigger("Attack4-1");
            GameObject meteoskill_instance = Instantiate(meteoskill, transform.position, Quaternion.identity) as GameObject;  //메테오 게임 오브젝트 생성
            bossanimetor.SetTrigger("Attack4-2");
        }
        else if (SkillNum == 3)                              //스킬 3번 주먹 소환
        {
            bossanimetor.SetTrigger("Attack5");             //애니메이션 재생
            Invoke("SpawnPunch", 0.56f);                     //해당 void 함수 실행, 시간 설정 설정 시간 지연후 함수 실행
            bossanimetor.SetTrigger("Attack5-1");
        }
        else if (SkillNum == 4)                              //스킬 4번 파이어 브레스
        {
            BreathSkill();
            faceControler = 0;
        }

        StartCoroutine(ChooseSkill());                                  //코루틴 시작(공격을 반복 시키기 위함)
                                                                        // Debug.Log(SkillNum.ToString());
    }

    void PunchSkill()
    {
        int FakeSkill = Random.Range(0, 3);                 //스킬 1번의 특수 기술 페이크 샷의 사용 유무
        Debug.Log("특수 스킬 번호 : " + FakeSkill.ToString());
        Vector2 dir = target.position - transform.position; //보스 기준 플레이어 좌우 위치 확인

        if (dir.x >= 3)                                      // 플레이어가 우측에 위치
        {
            if (FakeSkill == 2 && bossphase == 2)             //현재 페이즈와 페이크 스킬 사용 유무 체크
            {
                bossanimetor.SetTrigger("FastRAttack");     //페이크 스킬 사용
            }
            else
            {
                bossanimetor.SetTrigger("Attack2");         //일반 스킬 사용
            }
        }
        else                                                //플레이어가 좌측에 위치
        {
            if (FakeSkill == 2 && bossphase == 2)             //현재 페이즈와 페이크 스킬 사용 유무 체크
            {
                bossanimetor.SetTrigger("FastLAttack");     //페이크 스킬 사용
            }
            else
            {
                bossanimetor.SetTrigger("Attack1");         //일반 스킬 사용
            }
        }
    }

    void SpawnPunch()
    {
        GameObject punchSkill_instance = Instantiate(punchSkill, transform.position, Quaternion.identity) as GameObject;    //게임 오브젝트 생성
    }

    
    void BreathSkill()
    {
        breathControler = breathAngle;                              //Breath 스크립트에 넘길 변수 설정
        breath.gameObject.SetActive(true);                          //Breath 오브젝트 활성화
    }
    

    void FaceControl()
    {
        Vector3 toBoss = gameObject.transform.position;             //보스 위치값 저장
        Vector3 toTarget = target.position;                         //타켓 위치값 저장

        float Angle = GetAngle(toBoss, toTarget);                   //보스가 중심으로 플레이어의 각도 확인
        Debug.Log(Angle);
        if (Angle <-161)                           //1번 지역 시선 Angle >= -152 && Angle < -135
        {
            faceFront.gameObject.SetActive(false);
            faceLeft90.gameObject.SetActive(true);
            faceLeft45.gameObject.SetActive(false);
            faceRight90.gameObject.SetActive(false);
            faceRight45.gameObject.SetActive(false);
            breathAngle = 1;

            charSide = true;

        }
        else if (Angle >= -161 && Angle < -136)                           //2번 지역 시선
        {
            faceFront.gameObject.SetActive(false);
            faceLeft90.gameObject.SetActive(false);
            faceLeft45.gameObject.SetActive(true);
            faceRight90.gameObject.SetActive(false);
            faceRight45.gameObject.SetActive(false);
            breathAngle = 2;

            charSide = false;
        }
        else if (Angle >= -136 && Angle < -22)                           //3번 지역 시선
        {
            faceFront.gameObject.SetActive(true);
            faceLeft90.gameObject.SetActive(false);
            faceLeft45.gameObject.SetActive(false);
            faceRight90.gameObject.SetActive(false);
            faceRight45.gameObject.SetActive(false);
            breathAngle = 3;

            charSide = false;
        }
        else if (Angle >= -22 && Angle < -9.5)                           //4번 지역 시선
        {
            faceFront.gameObject.SetActive(false);
            faceLeft90.gameObject.SetActive(false);
            faceLeft45.gameObject.SetActive(false);
            faceRight90.gameObject.SetActive(false);
            faceRight45.gameObject.SetActive(true);
            breathAngle = 4;

            charSide = false;
        }
        else if (Angle >= -9.5)                           //5번 지역 시선
        {
            faceFront.gameObject.SetActive(false);
            faceLeft90.gameObject.SetActive(false);
            faceLeft45.gameObject.SetActive(false);
            faceRight90.gameObject.SetActive(true);
            faceRight45.gameObject.SetActive(false);
            breathAngle = 5;

            charSide = true;
        }
    }

    public static float GetAngle(Vector3 vBoss, Vector3 vPlayer)            //두개의 위칫값을 받아 angle값을 구한다.
    {
        Vector3 v = vPlayer - vBoss;
        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    

    #region UI 관련 메서드
    public void SetMaxHp()
    {
        SliderValueReciver svr = hpSlider.GetComponent<SliderValueReciver>();
        Slider hp_slider = hpSlider.GetComponent<Slider>();
        hp_slider.maxValue = bossHp;
    }
    public void ShowHpToSlider()
    {
        SliderValueReciver svr = hpSlider.GetComponent<SliderValueReciver>();
        svr.SliderValue = bossHp;
    }
    #endregion
}
