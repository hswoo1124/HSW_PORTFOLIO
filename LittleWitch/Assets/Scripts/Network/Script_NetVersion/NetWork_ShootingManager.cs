using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetWork_ShootingManager : MonoBehaviourPunCallbacks
{
    public GameObject[] projectiles;
    public Transform spawnPosition;
    public Transform spawnPos_Left1;
    public Transform spawnPos_Right1;
    //[HideInInspector]
    public int currentProjectile = 0;

    [SerializeField]
    Transform shootTarget;
    [SerializeField]
    Transform shootTarget_Left1;
    [SerializeField]
    Transform shootTarget_Right1;

    public FixedShootingJoystick sjs;

    bool skill1_cool_check = true;
    bool skill2_cool_check = true;
    bool skill3_cool_check = true;
    bool skill4_cool_check = true;
    bool skill5_cool_check = true;

    #region UI관련 변수
    public GameObject chargeSlider;

    public GameObject[] coolUIList;
    #endregion

    #region 차징스킬 변수
    public bool isPointerUp = false;
    public bool isCharge = false; //차지중인지 확인
    public float maxCharge = 1f;
    public float currentCharge = 0f;
    public float chargeMulti = 1f;
    public float speedMulti = 0f; //차지에 따른 스피드 증가 배율 (사거리 조절에 사용)
    public float increasedSpeed = 0f; //증가한 스피드
    #endregion

    NetWork_SkillProjecttile skillProjectile;
    PhotonView pv;
    int skillJamSize = 3;

    public float speed;
    float[] skillCoolList = new float[5];
    float[] skillCurrentCoolList = new float[5];

    private SkillType skillType;

    [SerializeField]
    GameObject wcp;
    GameObject prt;

    public int slot1_SkillId; //최초스킬 0으로 설정(테스트)
    public int slot2_SkillId;
    public int slot3_SkillId;



    void Create()
    {
        if (pv.IsMine)
        {
            string skillname = projectiles[currentProjectile].gameObject.name;
            GameObject projectile = PhotonNetwork.Instantiate(skillname, spawnPosition.position, Quaternion.identity);
            prt = projectile;
            GameObject player = gameObject.transform.parent.gameObject;
            NetWork_SkillProjecttile sp = projectile.GetComponent<NetWork_SkillProjecttile>();
            sp.weaponCenterPos = player.transform.GetChild(3).gameObject;
            wcp = sp.weaponCenterPos;
            sp.player = player;
            sp.characterStatus = player.GetComponent<NetWork_CharacterStatus>();
            sp.shootingManager = this.gameObject.GetComponent<NetWork_ShootingManager>();
            sp.SetDamage(player.GetComponent<NetWork_CharacterStatus>().attack_Point);
            projectile.transform.LookAt(shootTarget);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * (speed + increasedSpeed));
        }
    }
    void Create_Spread()
    {
        if (pv.IsMine)
        {
            string skillname_Left1 = projectiles[currentProjectile].gameObject.name;
            GameObject projectile_Left1 = PhotonNetwork.Instantiate(skillname_Left1, spawnPos_Left1.position, Quaternion.identity);
            GameObject player_Left1 = this.gameObject.transform.parent.gameObject;
            NetWork_SkillProjecttile sp_Left1 = projectile_Left1.GetComponent<NetWork_SkillProjecttile>();
            sp_Left1.weaponCenterPos = player_Left1.transform.GetChild(3).gameObject;
            sp_Left1.player = player_Left1;
            sp_Left1.characterStatus = player_Left1.GetComponent<NetWork_CharacterStatus>();
            sp_Left1.shootingManager = this.gameObject.GetComponent<NetWork_ShootingManager>();
            sp_Left1.SetDamage(player_Left1.GetComponent<NetWork_CharacterStatus>().attack_Point);
            projectile_Left1.transform.LookAt(shootTarget_Left1);
            projectile_Left1.GetComponent<Rigidbody>().AddForce(projectile_Left1.transform.forward * (speed + increasedSpeed));

            string skillname_Right1 = projectiles[currentProjectile].gameObject.name;
            GameObject projectile_Right1 = PhotonNetwork.Instantiate(skillname_Right1, spawnPos_Right1.position, Quaternion.identity);
            GameObject player_Right1 = this.gameObject.transform.parent.gameObject;
            NetWork_SkillProjecttile sp_Right1 = projectile_Right1.GetComponent<NetWork_SkillProjecttile>();
            sp_Right1.weaponCenterPos = player_Right1.transform.GetChild(3).gameObject;
            sp_Right1.player = player_Right1;
            sp_Right1.characterStatus = player_Right1.GetComponent< NetWork_CharacterStatus>();
            sp_Right1.shootingManager = this.gameObject.GetComponent<NetWork_ShootingManager>();
            sp_Right1.SetDamage(player_Right1.GetComponent<NetWork_CharacterStatus>().attack_Point);
            projectile_Right1.transform.LookAt(shootTarget_Right1);
            projectile_Right1.GetComponent<Rigidbody>().AddForce(projectile_Right1.transform.forward * (speed + increasedSpeed));

            wcp = sp_Right1.weaponCenterPos;
        }
    }
    void Start()
    {
        pv = gameObject.GetComponent<PhotonView>();
        //sjs = FindObjectOfType<FixedShootingJoystick>();
        skill1_cool_check = true;

        SelectSkill(0);
    }
    public void ShootMissile()
    {
      //  Debug.Log("히힣발싸" + pv.ViewID);
        isCharge = false;

        if (skill1_cool_check == true && currentProjectile == 0)
        {
            Create();


            if (skillType == SkillType.Spread)
            {
                Create_Spread();
            }

            NetWork_SkillProjecttile sp_instance = prt.GetComponent<NetWork_SkillProjecttile>();
            if ((SkillType)sp_instance.type == SkillType.Charge)
            {
                sp_instance.SetChargeNum(currentCharge);
            }

            skillCurrentCoolList[0] = skillCoolList[0];
            StartCoroutine(Skill1_Cool_Wait());
        }
        if (skill2_cool_check == true && currentProjectile == 1)
        {
            Create();


            if (skillType == SkillType.Spread)
            {
                Create_Spread();
            }

            NetWork_SkillProjecttile sp_instance = prt.GetComponent<NetWork_SkillProjecttile>();
            if ((SkillType)sp_instance.type == SkillType.Charge)
            {
                sp_instance.SetChargeNum(currentCharge);
            }

            skillCurrentCoolList[1] = skillCoolList[1];
            StartCoroutine(Skill2_Cool_Wait());
        }
        if (skill3_cool_check == true && currentProjectile == 2)
        {
            Create();


            if (skillType == SkillType.Spread)
            {
                Create_Spread();
            }

            NetWork_SkillProjecttile sp_instance = prt.GetComponent<NetWork_SkillProjecttile>();
            if ((SkillType)sp_instance.type == SkillType.Charge)
            {
                sp_instance.SetChargeNum(currentCharge);
            }

            skillCurrentCoolList[2] = skillCoolList[2];
            StartCoroutine(Skill3_Cool_Wait());
        }

        #region 테스트용버전
        if (skill4_cool_check == true && currentProjectile == 3)
        {
            Create();


            if (skillType == SkillType.Spread)
            {
                Create_Spread();

            }

            NetWork_SkillProjecttile sp_instance = prt.GetComponent<NetWork_SkillProjecttile>();
            if ((SkillType)sp_instance.type == SkillType.Charge)
            {
                sp_instance.SetChargeNum(currentCharge);
            }

            skillCurrentCoolList[3] = skillCoolList[3];
            StartCoroutine(Skill4_Cool_Wait());
        }
        if (skill5_cool_check == true && currentProjectile == 4)
        {
            Create();


            if (skillType == SkillType.Spread)
            {
                Create_Spread();
            }

            NetWork_SkillProjecttile sp_instance = prt.GetComponent<NetWork_SkillProjecttile>();
            if ((SkillType)sp_instance.type == SkillType.Charge)
            {
                sp_instance.SetChargeNum(currentCharge);
            }

            skillCurrentCoolList[4] = skillCoolList[4];
            StartCoroutine(Skill5_Cool_Wait());
        }

        #endregion

        increasedSpeed = 0;
        currentCharge = 0f;

        ShowChargeSlider();
    }
    public void CheckJoystickUp(bool upCheck)
    {
        isPointerUp = upCheck;
    }
    void Update()
    {
        if (!pv.IsMine)
        {
            return;
        }
        #region 쿨타임
        Decrease_Current_Cool();
        #endregion
        //try
        //{
            #region ui
            ShowChargeSlider();
            ShowSkillCoolSlider();
            if (isCharge == true)
                chargeSlider.SetActive(true);
            else
            {
                currentCharge = 0f;
                ShowChargeSlider();
                chargeSlider.SetActive(false);
            }
            #endregion

            if (Input.GetKeyDown(KeyCode.D))
            {
                nextEffect();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                previousEffect();
            }

            float radian = CalculateRadian();

            if (radian >= 0.9f)
            {
                if (skillType == SkillType.None)
                {
                    ShootMissile();
                   // Debug.Log("1번쨰 슈팅");
                }
                else if (skillType == SkillType.Charge)
                {
                    if (skill1_cool_check == true)
                        StartCoroutine(Skill_Charging());
                }
                else if (skillType == SkillType.Spread)
                {
                    ShootMissile();
                    Debug.Log("2번쨰 슈팅");
                }
            }
            else if (0f < radian && radian < 0.9f)
            {
                if (skillType == SkillType.Charge)
                {
                    StopCoroutine(Skill_Charging());
                    currentCharge = 0f;
                    isCharge = false;
                }
            }
            if (isPointerUp == true)
            {
                if (isCharge == true)
                {
                    ShootMissile();
                    Debug.Log("3번쨰 슈팅");
                    ShowChargeSlider();
                    StopCoroutine(Skill_Charging());
                }
            }
        //}
        //catch
        //{
        //    Debug.Log("슈팅미사일 오류");
        //}
    }

    float CalculateRadian()
    {
        float h = sjs.Horizontal;
        if (h <= 0)
            h = -h;
        float a = Mathf.Atan(sjs.Vertical / sjs.Horizontal); //원 각도 구함
        //각도를 이용하여 cos사용하고 점과 중점사이의 거리를 구함
        float r = h / Mathf.Cos(a);
        //거리가 0.9 이상일때만 스킬 사용

        return r;
    }

    public void nextEffect()
    {
        if (currentProjectile < projectiles.Length - 1)
            currentProjectile++;
        else
            currentProjectile = 0;
        ChangeSkill();
    }

    public void previousEffect()
    {
        if (currentProjectile > 0)
            currentProjectile--;
        else
            currentProjectile = projectiles.Length - 1;
        ChangeSkill();
    }

    #region 스킬에서 받아오는 정보
    public void ClickSkill1Button()
    {
        currentProjectile = slot1_SkillId;
        ChangeSkill();
    }
    public void ClickSkill2Button()
    {
        if (slot2_SkillId < 10)
        {
            currentProjectile = slot2_SkillId;
            ChangeSkill();
        }
    }
    public void ClickSkill3Button()
    {
        if (slot3_SkillId < 10)
        {
            currentProjectile = slot3_SkillId;
            ChangeSkill();
        }
    }
    public void SelectSkill(int selectSkillIndex)
    {
        currentProjectile = selectSkillIndex;
        ChangeSkill();
    }
    public void ChangeSkill()
    {
        //try
        //{
            skillProjectile = projectiles[currentProjectile].GetComponent<NetWork_SkillProjecttile>();
            AdjustSpeed(skillProjectile.speed);
            AdjustCool(skillProjectile.coolTime);
            GetSkillType((int)skillProjectile.type);
            if (skillType == SkillType.None || skillType == SkillType.Spread)
            {
                StopCoroutine(Skill_Charging());
                increasedSpeed = 0f;
                currentCharge = 0f;
            }
            else if (skillType == SkillType.Charge)
            {

                GetChargeSkillInfo(skillProjectile.chargeMulti, skillProjectile.chargeMax);
                GetChargeSkillIncreaseSpeedMulti(skillProjectile.increaseSpeedMulti);
            }
        //}
        //catch
        //{
        //    Debug.Log(currentProjectile);
        //}

    }

    public void AdjustSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public void AdjustCool(float newCool)
    {
        skillCoolList[currentProjectile] = newCool;
    }
    public void GetSkillType(int typeIdx)
    {
        skillType = (SkillType)typeIdx;
    }
    public void GetChargeSkillInfo(float multi, float max)
    {
        chargeMulti = multi;
        maxCharge = max;
    }
    public void GetChargeSkillIncreaseSpeedMulti(float isp)
    {
        speedMulti = isp;
    }
    #endregion

    #region 쿨타임 관련 코루틴
    IEnumerator Skill1_Cool_Wait()
    {
        skill1_cool_check = false;
        yield return new WaitForSeconds(skillCoolList[currentProjectile]);
        skill1_cool_check = true;
        StopCoroutine(Skill1_Cool_Wait());
    }

    IEnumerator Skill2_Cool_Wait()
    {
        skill2_cool_check = false;
        yield return new WaitForSeconds(skillCoolList[currentProjectile]);
        skill2_cool_check = true;
        StopCoroutine(Skill2_Cool_Wait());
    }

    IEnumerator Skill3_Cool_Wait()
    {
        skill3_cool_check = false;
        yield return new WaitForSeconds(skillCoolList[currentProjectile]);
        skill3_cool_check = true;
        StopCoroutine(Skill3_Cool_Wait());
    }

    #region 테스트버전
    IEnumerator Skill4_Cool_Wait()
    {
        skill4_cool_check = false;
        yield return new WaitForSeconds(skillCoolList[currentProjectile]);
        skill4_cool_check = true;
        StopCoroutine(Skill4_Cool_Wait());
    }
    IEnumerator Skill5_Cool_Wait()
    {
        skill5_cool_check = false;
        yield return new WaitForSeconds(skillCoolList[currentProjectile]);
        skill5_cool_check = true;
        StopCoroutine(Skill5_Cool_Wait());
    }
    #endregion

    IEnumerator Skill_Charging()
    {
        isCharge = true;

        if (speedMulti > 0)
        {
            increasedSpeed += 0.01f * speedMulti;
        }
        if (maxCharge > currentCharge)
        {
            currentCharge += 0.01f * chargeMulti;
        }
        else
        {
            ShootMissile();
            StopCoroutine(Skill_Charging());
            isCharge = false;
        }
        yield return new WaitForSeconds(0.01f);
    }

    void Decrease_Current_Cool()
    {
        for (int i = 0; i < skillCurrentCoolList.Length; i++)
        {
            if (skillCurrentCoolList[i] > 0)
                skillCurrentCoolList[i] -= Time.deltaTime * 1f;
            else if (skillCurrentCoolList[i] < 0)
                skillCurrentCoolList[i] = 0;
        }
    }

    /*IEnumerator Decrease_Current_Cool()
    {
        yield return new WaitForSeconds(0.01f);
        for(int i = 0; i < skillCurrentCoolList.Length; i++)
        {
            if (skillCurrentCoolList[i] > 0)
                skillCurrentCoolList[i] -= 0.01f;
            else if (skillCurrentCoolList[i] < 0)
                skillCurrentCoolList[i] = 0;
        }
    }*/
    #endregion

    #region UI 관련 메서드
    void ShowChargeSlider()
    {
        try
        {
            SliderValueReciver svr = chargeSlider.GetComponent<SliderValueReciver>();
            svr.SliderValue = currentCharge;
        }
        catch
        {

        }
    }
    void ShowSkillCoolSlider()
    {
        try
        {
            for (int i = 0; i < skillJamSize; i++)
            {
                if (projectiles[i])
                {
                    SliderValueReciver svr_skillcool = coolUIList[i].GetComponent<SliderValueReciver>();
                    Slider slider = coolUIList[i].GetComponent<Slider>();
                    slider.maxValue = skillCoolList[i];
                    svr_skillcool.SliderValue = skillCurrentCoolList[i];
                }
            }
        }
        catch
        {

        }
    }
    #endregion

    enum SkillType
    {
        None,
        Charge,
        Spread
    }
}
