using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class ShootingManager : MonoBehaviour
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
    [SerializeField]
    FixedShootingJoystick sjs;

    bool shootAble = false;
    bool skill1_cool_check = true;
    bool skill2_cool_check = true;
    bool skill3_cool_check = true;
    bool skill4_cool_check = true;
    bool skill5_cool_check = true;

    #region UI관련 변수
    public GameObject chargeSlider;
    [SerializeField]
    GameObject[] coolUIList;
    public bool isEndpoint = false;
    public int slot1_SkillId = 0; //최초스킬 0으로 설정(테스트)
    public int slot2_SkillId = 100;
    public int slot3_SkillId = 100;

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

    SkillProjectile skillProjectile;

    int skillJamSize = 3;

    private float speed;
    [SerializeField]
    private float mana;

    float[] skillCoolList = new float[5];
    float[] skillCurrentCoolList = new float[5];

    private SkillType skillType;
    CharacterStatus characterStatus;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        try
        {
            sjs = FindObjectOfType<FixedShootingJoystick>();
            skill1_cool_check = true;
            GameObject player = GameObject.Find("Player");
            characterStatus = player.GetComponent<CharacterStatus>();
            SelectSkill(0);
        }
        catch
        {
            Debug.Log("멀티모드 슈밍매니저 오류^^");
        }
    }
    public void ShootMissile()
    {
        isCharge = false;
        if (skill1_cool_check == true && currentProjectile == 0)
        {
            characterStatus.mana_Point -= mana;
            Debug.Log("ShootMissile");
            GameObject projectile = Instantiate(projectiles[currentProjectile], spawnPosition.position, Quaternion.identity) as GameObject;
            projectile.transform.LookAt(shootTarget);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * (speed + increasedSpeed));


            if (skillType == SkillType.Spread)
            {
                GameObject projectile_Left1 = Instantiate(projectiles[currentProjectile], spawnPos_Left1.position, Quaternion.identity) as GameObject;
                projectile_Left1.transform.LookAt(shootTarget_Left1);
                projectile_Left1.GetComponent<Rigidbody>().AddForce(projectile_Left1.transform.forward * (speed + increasedSpeed));

                GameObject projectile_Right1 = Instantiate(projectiles[currentProjectile], spawnPos_Right1.position, Quaternion.identity) as GameObject;
                projectile_Right1.transform.LookAt(shootTarget_Right1);
                projectile_Right1.GetComponent<Rigidbody>().AddForce(projectile_Right1.transform.forward * (speed + increasedSpeed));

            }

            SkillProjectile sp_instance = projectile.GetComponent<SkillProjectile>();
            if ((SkillType)sp_instance.type == SkillType.Charge)
            {
                sp_instance.SetChargeNum(currentCharge);
            }

            skillCurrentCoolList[0] = skillCoolList[0];
            StartCoroutine(Skill1_Cool_Wait());
        }
        if (skill2_cool_check == true && currentProjectile == 1)
        {
            characterStatus.mana_Point -= mana;
            Debug.Log("ShootMissile");
            GameObject projectile = Instantiate(projectiles[currentProjectile], spawnPosition.position, Quaternion.identity) as GameObject;
            projectile.transform.LookAt(shootTarget);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * (speed + increasedSpeed));


            if (skillType == SkillType.Spread)
            {
                GameObject projectile_Left1 = Instantiate(projectiles[currentProjectile], spawnPos_Left1.position, Quaternion.identity) as GameObject;
                projectile_Left1.transform.LookAt(shootTarget_Left1);
                projectile_Left1.GetComponent<Rigidbody>().AddForce(projectile_Left1.transform.forward * (speed + increasedSpeed));

                GameObject projectile_Right1 = Instantiate(projectiles[currentProjectile], spawnPos_Right1.position, Quaternion.identity) as GameObject;
                projectile_Right1.transform.LookAt(shootTarget_Right1);
                projectile_Right1.GetComponent<Rigidbody>().AddForce(projectile_Right1.transform.forward * (speed + increasedSpeed));

            }

            SkillProjectile sp_instance = projectile.GetComponent<SkillProjectile>();
            if ((SkillType)sp_instance.type == SkillType.Charge)
            {
                sp_instance.SetChargeNum(currentCharge);
            }

            skillCurrentCoolList[1] = skillCoolList[1];
            StartCoroutine(Skill2_Cool_Wait());
        }
        if (skill3_cool_check == true && currentProjectile == 2)
        {
            characterStatus.mana_Point -= mana;
            Debug.Log("ShootMissile");
            GameObject projectile = Instantiate(projectiles[currentProjectile], spawnPosition.position, Quaternion.identity) as GameObject;
            projectile.transform.LookAt(shootTarget);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * (speed + increasedSpeed));


            if (skillType == SkillType.Spread)
            {
                GameObject projectile_Left1 = Instantiate(projectiles[currentProjectile], spawnPos_Left1.position, Quaternion.identity) as GameObject;
                projectile_Left1.transform.LookAt(shootTarget_Left1);
                projectile_Left1.GetComponent<Rigidbody>().AddForce(projectile_Left1.transform.forward * (speed + increasedSpeed));

                GameObject projectile_Right1 = Instantiate(projectiles[currentProjectile], spawnPos_Right1.position, Quaternion.identity) as GameObject;
                projectile_Right1.transform.LookAt(shootTarget_Right1);
                projectile_Right1.GetComponent<Rigidbody>().AddForce(projectile_Right1.transform.forward * (speed + increasedSpeed));

            }

            SkillProjectile sp_instance = projectile.GetComponent<SkillProjectile>();
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
            characterStatus.mana_Point -= mana;
            Debug.Log("ShootMissile");
            GameObject projectile = Instantiate(projectiles[currentProjectile], spawnPosition.position, Quaternion.identity) as GameObject;
            projectile.transform.LookAt(shootTarget);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * (speed + increasedSpeed));


            if (skillType == SkillType.Spread)
            {
                GameObject projectile_Left1 = Instantiate(projectiles[currentProjectile], spawnPos_Left1.position, Quaternion.identity) as GameObject;
                projectile_Left1.transform.LookAt(shootTarget_Left1);
                projectile_Left1.GetComponent<Rigidbody>().AddForce(projectile_Left1.transform.forward * (speed + increasedSpeed));

                GameObject projectile_Right1 = Instantiate(projectiles[currentProjectile], spawnPos_Right1.position, Quaternion.identity) as GameObject;
                projectile_Right1.transform.LookAt(shootTarget_Right1);
                projectile_Right1.GetComponent<Rigidbody>().AddForce(projectile_Right1.transform.forward * (speed + increasedSpeed));

            }

            SkillProjectile sp_instance = projectile.GetComponent<SkillProjectile>();
            if ((SkillType)sp_instance.type == SkillType.Charge)
            {
                sp_instance.SetChargeNum(currentCharge);
            }

            skillCurrentCoolList[3] = skillCoolList[3];
            StartCoroutine(Skill4_Cool_Wait());
        }
        if (skill5_cool_check == true && currentProjectile == 4)
        {
            characterStatus.mana_Point -= mana;
            Debug.Log("ShootMissile");
            GameObject projectile = Instantiate(projectiles[currentProjectile], spawnPosition.position, Quaternion.identity) as GameObject;
            projectile.transform.LookAt(shootTarget);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * (speed + increasedSpeed));


            if (skillType == SkillType.Spread)
            {
                GameObject projectile_Left1 = Instantiate(projectiles[currentProjectile], spawnPos_Left1.position, Quaternion.identity) as GameObject;
                projectile_Left1.transform.LookAt(shootTarget_Left1);
                projectile_Left1.GetComponent<Rigidbody>().AddForce(projectile_Left1.transform.forward * (speed + increasedSpeed));

                GameObject projectile_Right1 = Instantiate(projectiles[currentProjectile], spawnPos_Right1.position, Quaternion.identity) as GameObject;
                projectile_Right1.transform.LookAt(shootTarget_Right1);
                projectile_Right1.GetComponent<Rigidbody>().AddForce(projectile_Right1.transform.forward * (speed + increasedSpeed));
            }

            SkillProjectile sp_instance = projectile.GetComponent<SkillProjectile>();
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
    void FixedUpdate()
    {
        try
        {
            if (mana <= characterStatus.mana_Point)
            {
                shootAble = true;
            }
            else
            {
                shootAble = false;
            }
            #region 쿨타임
            Decrease_Current_Cool();
            #endregion
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
            #region 스킬(투사체) 사용관련
            if (shootAble == true)
            {
                if (isEndpoint == true)
                {
                    if (skillType == SkillType.None)
                        ShootMissile();
                    else if (skillType == SkillType.Charge)
                    {
                        if (skill1_cool_check == true)
                            StartCoroutine(Skill_Charging());
                    }
                    else if (skillType == SkillType.Spread)
                        ShootMissile();
                }
                else if (isEndpoint == false)
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
                        ShowChargeSlider();
                        StopCoroutine(Skill_Charging());
                    }
                }
            }
            #endregion
        }
        catch
        {

        }
        
    }
    #region 스킬에서 받아오는 정보
    public void ClickSkill1Button()
    {
        currentProjectile = slot1_SkillId;
        ChangeSkill();
    }
    public void ClickSkill2Button()
    {
        if(slot2_SkillId <10)
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
        skillProjectile = projectiles[currentProjectile].GetComponent<SkillProjectile>();
        AdjustSpeed(skillProjectile.speed);
        AdjustCool(skillProjectile.coolTime);
        AdjustMana(skillProjectile.manaCost);
        GetSkillType((int)skillProjectile.type);
        if(skillType == SkillType.None || skillType == SkillType.Spread)
        {
            StopCoroutine(Skill_Charging());
            increasedSpeed = 0f;
            currentCharge = 0f;
        }
        else if(skillType == SkillType.Charge)
        {

            GetChargeSkillInfo(skillProjectile.chargeMulti, skillProjectile.chargeMax);
            GetChargeSkillIncreaseSpeedMulti(skillProjectile.increaseSpeedMulti);
        }
        
    }
    public void AdjustSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public void AdjustCool(float newCool)
    {
        skillCoolList[currentProjectile] = newCool;
    }
    public void AdjustMana(float newMana)
    {
        mana = newMana;
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

    #endregion

    #region UI 관련 메서드
    void ShowChargeSlider()
    {
        SliderValueReciver svr = chargeSlider.GetComponent<SliderValueReciver>();
        svr.SliderValue = currentCharge;
    }
    void ShowSkillCoolSlider()
    {

        //for (int i = 0; i < skillJamSize; i++)
        //{
        //    if (projectiles[i])
        //    {
        //        SliderValueReciver svr_skillcool = coolUIList[i].GetComponent<SliderValueReciver>();
        //        Slider slider = coolUIList[i].GetComponent<Slider>();
        //        slider.maxValue = skillCoolList[i];
        //        svr_skillcool.SliderValue = skillCurrentCoolList[i];
        //    }
        //}

        #region 시연용
        SliderValueReciver svr_skillcool1 = coolUIList[0].GetComponent<SliderValueReciver>();
        Slider slider1 = coolUIList[0].GetComponent<Slider>();
        slider1.maxValue = skillCoolList[slot1_SkillId];
        svr_skillcool1.SliderValue = skillCurrentCoolList[slot1_SkillId];

        SliderValueReciver svr_skillcool2 = coolUIList[1].GetComponent<SliderValueReciver>();
        Slider slider2 = coolUIList[1].GetComponent<Slider>();
        slider2.maxValue = skillCoolList[slot2_SkillId];
        svr_skillcool2.SliderValue = skillCurrentCoolList[slot2_SkillId];

        SliderValueReciver svr_skillcool3 = coolUIList[2].GetComponent<SliderValueReciver>();
        Slider slider3 = coolUIList[2].GetComponent<Slider>();
        slider3.maxValue = skillCoolList[slot3_SkillId];
        svr_skillcool3.SliderValue = skillCurrentCoolList[slot3_SkillId];
        #endregion
    }
    #endregion

    enum SkillType
    {
        None,
        Charge,
        Spread
    }
}