using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetWork_CharacterStatus : MonoBehaviourPunCallbacks
{
    public static NetWork_CharacterStatus instance;
    #region 플레이어 스텟정보
    public GameObject hpSlider;
    public Slider manaSlider;
    public float max_Hp;
    public float hp_Point;
    public float attack_Point;
    public float move_Speed;
    public float max_Mana;
    public float mana_Point;
    [SerializeField]
    private string get_Item;
    public string[] skill_use;
    public float char_Stic;
    #endregion
    #region 플레이어 스텟+ 아이템
    private float php_Point;
    private float pattack_Point;
    private float pattack_Speed;
    private float pmove_Speed;
    private float pattack_Range;
    #endregion
    public Tribe playerTribe;

    public State playerState;

    public GameObject item_Set_Position;

    List<GameObject> item_List = new List<GameObject>();
    Rigidbody rg;

    bool hasCollided = false;
    bool hasTriggered = false;
    public MonsterElement element;
    float decreaseMoveSpeed;
    float decreaseAttackSpeed;
    [SerializeField]
    PhotonView pv;
    #region 스킬관련 변수
    public float skillProjectileDamage = 0f;
    public int skillProjectileElement = 0;
    public float skillImpactDamage = 0f;
    public int skillImpactElement = 0;
    NetWork_SkillProjecttile sp;
    #endregion

    #region 속성효과 변수
    bool isIgnite = false;
    bool igniteCheck = false;
    float igniteCool = 1f;
    int igniteCount = 0;

    bool isFreeze = false;
    bool freezeCheck = false;
    float freezeCool = 1f;
    int freezeMulti = 0;
    int freezeCount = 0;
    bool isFrozen = false;
    bool frozenCheck = false;
    float frozenCool = 1f;
    int frozenCount = 0;

    bool isToxic = false;
    bool toxicCheck = false;
    float toxicCool = 1f;
    int toxicMulti = 0;
    int toxicCount = 0;

    bool isShock = false;
    bool shockCheck = false;
    float shockCool = 1f;
    int shockMulti = 0;
    int shockCount = 0;
    #endregion

    #region 속성효과 이펙트
    public GameObject igniteEffect;
    public GameObject freezeEffect;
    public GameObject[] toxicEffectList;
    public GameObject[] shockEffectList;
    public GameObject frozenEffect;
    #endregion
    #region 종족, 상태
    public enum Tribe
    {
        Human = 1,
        Demon = 2,
        Angel = 3
    }

    public enum State
    {
        Wating = 1,
        Move = 2,
        Dash = 3,
        Left = 4,
        Right = 5,
        Attack = 6,
        Skill = 7,
        Adnormal = 8,
        Die = 9
    }
    public enum MonsterElement
    {
        Arcane,
        Fire,
        Ice,
        Poison,
        Lightning,
        None
    }
    #endregion

    #region PlayerMovement 관련 변수
    PlayerMovement pm; //객체받아오자
    NetWork_PlayerMovement npm;
    public ItemInfo ii;
    #endregion

    #region 초기값 설정
    private void Start()
    {
        instance = this;
        hp_Point = 1000f;
        attack_Point = 100f;
        move_Speed = 0.06f;

        playerTribe = Tribe.Demon;
        playerState = State.Wating;

        for (int i = 0; ; i++)
        {
            try
            {
                GameObject c = item_Set_Position.transform.GetChild(i).gameObject;
                item_List.Add(c);
            }
            catch
            {
                break;
            }
        }
    }
    #endregion
    #region 충돌시 판정
    public void GetSkillDamage(float damage)
    {
        skillProjectileDamage = damage;
    }
    public void GetSkillElement(int element)
    {
        skillProjectileElement = element;
    }

    public void GetSkillImpactDamage(float damage)
    {
        skillImpactDamage = damage;
    }
    public void GetSkillImpactElement(int element)
    {
        skillImpactElement = element;
    }
    //==============================================================================================
    
    public void Hit_collision1(object other)
    {
        Debug.Log("충돌발동");
        if (!hasCollided)
        {
            GameObject hit = (GameObject)other;
                sp = hit.GetComponent<NetWork_SkillProjecttile>();
                skillProjectileDamage = sp.damage;
                skillProjectileElement = (int)sp.element;
            Debug.Log(skillProjectileDamage);
                switch (skillProjectileElement)
                {
                    case 0:                 //스킬속성이 비전(Arcane)일 경우
                        if (element == MonsterElement.None)
                        {
                            hp_Point -= skillProjectileDamage * 1.2f;
                        }
                        else if (element == 0)
                        {
                        hp_Point -= skillProjectileDamage * 1.5f;
                        }
                        else
                        {
                        hp_Point -= skillProjectileDamage;
                        }
                        break;
                    case 1:                 //스킬속성이 화염(fire)일 경우
                        if (element == MonsterElement.Fire)
                        {
                        hp_Point -= skillProjectileDamage * 0.5f;
                        }
                        else if (element == MonsterElement.Ice)
                        {
                        hp_Point -= skillProjectileDamage * 1.5f;
                        }
                        else if (element == MonsterElement.None)
                        {
                            igniteCount = 0;
                            igniteEffect.SetActive(true);
                            if (isIgnite == false)
                            {

                                StartCoroutine(Ignite());
                            }
                        hp_Point -= skillProjectileDamage * 1.2f;
                        }
                        else
                        {
                            igniteCount = 0;
                            igniteEffect.SetActive(true);
                            if (isIgnite == false)
                            {

                                StartCoroutine(Ignite());
                            }
                        hp_Point -= skillProjectileDamage;
                        }
                        break;
                    case 2:                 //스킬속성이 냉기(ice)일 경우
                        if (element == MonsterElement.Fire)
                        {
                        hp_Point -= skillProjectileDamage * 1.5f;
                        }
                        else if (element == MonsterElement.Ice)
                        {
                        hp_Point -= skillProjectileDamage * 0.5f;
                        }
                        else if (element == MonsterElement.None)
                        {
                            freezeCount = 0;
                            if (freezeMulti < 7 && isFreeze == true)    //냉기중첩
                            {
                                freezeMulti++;
                            }
                            if (isFreeze == false && isFrozen == false)
                            {
                                freezeMulti++;
                                freezeEffect.SetActive(true);
                                StartCoroutine(Freeze());
                            }
                            if (freezeMulti == 7)    //빙결상태 on
                            {
                                StopCoroutine(Freeze());
                                frozenEffect.SetActive(true);
                                isFreeze = false;
                                StartCoroutine(Frozen());
                            }
                        hp_Point -= skillProjectileDamage * 1.2f;
                        }
                        else
                        {
                        //냉기상태 보류
                        hp_Point -= skillProjectileDamage;
                        }
                        break;
                    case 3:                 //스킬속성이 맹독(Poison)일 경우
                        if (element == MonsterElement.Fire)
                        {
                            toxicCount = 0;
                            if (toxicMulti < 5 && isToxic == true)
                            {
                                toxicMulti++;
                                toxicEffectList[toxicMulti - 1].SetActive(true);
                            }
                            if (isToxic == false)
                            {
                                toxicMulti++;
                                toxicEffectList[toxicMulti - 1].SetActive(true);
                                StartCoroutine(Toxic());
                            }
                        hp_Point -= skillProjectileDamage * 1.2f;
                        }
                        else if (element == MonsterElement.Poison)
                        {
                        hp_Point -= skillProjectileDamage * 0.5f;
                        }
                        else if (element == MonsterElement.None)
                        {
                            toxicCount = 0;
                            if (toxicMulti < 5 && isToxic == true)
                            {
                                toxicMulti++;
                                toxicEffectList[toxicMulti - 1].SetActive(true);
                            }
                            if (isToxic == false)
                            {
                                toxicMulti++;
                                toxicEffectList[toxicMulti - 1].SetActive(true);
                                StartCoroutine(Toxic());
                            }
                        hp_Point -= skillProjectileDamage * 1.2f;
                        }
                        else
                        {
                            toxicCount = 0;
                            if (toxicMulti < 5 && isToxic == true)
                            {
                                toxicMulti++;
                                toxicEffectList[toxicMulti - 1].SetActive(true);
                            }
                            if (isToxic == false)
                            {
                                toxicMulti++;
                                toxicEffectList[toxicMulti - 1].SetActive(true);
                                StartCoroutine(Toxic());
                            }
                        hp_Point -= skillProjectileDamage;
                        }
                        break;
                    case 4:                 //스킬속성이 번개(Lightning)일 경우
                        if (element == MonsterElement.Ice)
                        {
                            shockCount = 0;
                            if (shockMulti < 5 && isShock == true)
                            {
                                shockMulti++;
                                shockEffectList[shockMulti - 1].SetActive(true);
                            }
                            if (isShock == false)
                            {
                                shockMulti++;
                                shockEffectList[shockMulti - 1].SetActive(true);
                                StartCoroutine(Shock());
                            }
                        hp_Point -= (skillProjectileDamage * 1.2f) * (1f + shockMulti * 0.1f);
                        }
                        else if (element == MonsterElement.Lightning)
                        {
                        hp_Point -= skillProjectileDamage * 0.5f;
                        }
                        else if (element == MonsterElement.None)
                        {
                            shockCount = 0;
                            if (shockMulti < 5 && isShock == true)
                            {
                                shockMulti++;
                                shockEffectList[shockMulti - 1].SetActive(true);
                            }
                            if (isShock == false)
                            {
                                shockMulti++;
                                shockEffectList[shockMulti - 1].SetActive(true);
                                StartCoroutine(Shock());
                            }
                        hp_Point -= (skillProjectileDamage * 1.2f) * (1f + shockMulti * 0.1f);
                        }
                        else
                        {
                            shockCount = 0;
                            if (shockMulti < 5 && isShock == true)
                            {
                                shockMulti++;
                                shockEffectList[shockMulti - 1].SetActive(true);
                            }
                            if (isShock == false)
                            {
                                shockMulti++;
                                shockEffectList[shockMulti - 1].SetActive(true);
                                StartCoroutine(Shock());
                            }
                        hp_Point -= skillProjectileDamage * (1f + shockMulti * 0.1f);
                        }
                        break;
                    case 5:
                    hp_Point -= skillProjectileDamage;
                        break;
                }
        }
    }
  
    public void Hit_collision2(object other)
    {
        if (!hasCollided)
        {
            GameObject hit = (GameObject)other;
            SplashDamage sd = hit.GetComponent<SplashDamage>();
            skillImpactDamage = sd.splashDamage;
            skillImpactElement = (int)sd.element;
            switch (skillImpactElement)
            {
                case 0:                 //스킬속성이 비전(Arcane)일 경우
                    if (element == MonsterElement.None)
                    {
                        hp_Point -= skillImpactDamage * 1.2f;
                    }
                    else if (element == 0)
                    {
                        hp_Point -= skillImpactDamage * 1.5f;
                    }
                    else
                    {
                        hp_Point -= skillImpactDamage;
                    }
                    break;
                case 1:                 //스킬속성이 화염(fire)일 경우
                    if (element == MonsterElement.Fire)
                    {
                        hp_Point -= skillImpactDamage * 0.5f;
                    }
                    else if (element == MonsterElement.Ice)
                    {
                        hp_Point -= skillImpactDamage * 1.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        igniteCount = 0;
                        igniteEffect.SetActive(true);
                        if (isIgnite == false)
                        {

                            StartCoroutine(Ignite());
                        }
                        hp_Point -= skillImpactDamage * 1.2f;
                    }
                    else
                    {
                        igniteCount = 0;
                        igniteEffect.SetActive(true);
                        if (isIgnite == false)
                        {

                            StartCoroutine(Ignite());
                        }
                        hp_Point -= skillImpactDamage;
                    }
                    break;
                case 2:                 //스킬속성이 냉기(ice)일 경우
                    if (element == MonsterElement.Fire)
                    {
                        hp_Point -= skillImpactDamage * 1.5f;
                    }
                    else if (element == MonsterElement.Ice)
                    {
                        hp_Point -= skillImpactDamage * 0.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        freezeCount = 0;
                        if (freezeMulti < 7 && isFreeze == true)    //냉기중첩
                        {
                            freezeMulti++;
                        }
                        if (isFreeze == false && isFrozen == false)
                        {
                            freezeMulti++;
                            freezeEffect.SetActive(true);
                            StartCoroutine(Freeze());
                        }
                        if (freezeMulti == 7)    //빙결상태 on
                        {
                            StopCoroutine(Freeze());
                            frozenEffect.SetActive(true);
                            isFreeze = false;
                            StartCoroutine(Frozen());
                        }
                        hp_Point -= skillImpactDamage * 1.2f;
                    }
                    else
                    {
                        //냉기상태 보류
                        hp_Point -= skillImpactDamage;
                    }
                    break;
                case 3:                 //스킬속성이 맹독(Poison)일 경우
                    if (element == MonsterElement.Fire)
                    {
                        toxicCount = 0;
                        if (toxicMulti < 5 && isToxic == true)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                        }
                        if (isToxic == false)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                            StartCoroutine(Toxic());
                        }
                        hp_Point -= skillImpactDamage * 1.2f;
                    }
                    else if (element == MonsterElement.Poison)
                    {
                        hp_Point -= skillImpactDamage * 0.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        toxicCount = 0;
                        if (toxicMulti < 5 && isToxic == true)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                        }
                        if (isToxic == false)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                            StartCoroutine(Toxic());
                        }
                        hp_Point -= skillImpactDamage * 1.2f;
                    }
                    else
                    {
                        toxicCount = 0;
                        if (toxicMulti < 5 && isToxic == true)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                        }
                        if (isToxic == false)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                            StartCoroutine(Toxic());
                        }
                        hp_Point -= skillImpactDamage;
                    }
                    break;
                case 4:                 //스킬속성이 번개(Lightning)일 경우
                    if (element == MonsterElement.Ice)
                    {
                        shockCount = 0;
                        if (shockMulti < 5 && isShock == true)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                        }
                        if (isShock == false)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                            StartCoroutine(Shock());
                        }
                        hp_Point -= (skillImpactDamage * 1.2f) * (1f + shockMulti * 0.1f);
                    }
                    else if (element == MonsterElement.Lightning)
                    {
                        hp_Point -= skillImpactDamage * 0.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        shockCount = 0;
                        if (shockMulti < 5 && isShock == true)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                        }
                        if (isShock == false)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                            StartCoroutine(Shock());
                        }
                        hp_Point -= (skillImpactDamage * 1.2f) * (1f + shockMulti * 0.1f);
                    }
                    else
                    {
                        shockCount = 0;
                        if (shockMulti < 5 && isShock == true)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                        }
                        if (isShock == false)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                            StartCoroutine(Shock());
                        }
                        hp_Point -= skillImpactDamage * (1f + shockMulti * 0.1f);
                    }
                    break;
                case 5:
                    hp_Point -= skillImpactDamage;
                    break;
            }
        }
    }
   
    public void Hit_collider1(object other)
    {
        if (!hasTriggered)
        {
            GameObject hit = (GameObject)other;
            sp = hit.GetComponent<NetWork_SkillProjecttile>();
            skillProjectileDamage = sp.damage;
            skillProjectileElement = (int)sp.element;
            pv.RPC("Hit_ColliderSur", RpcTarget.AllBuffered, skillProjectileDamage, skillProjectileElement);
        }
    }
    
    public void Hit_collider2(object other)
    {
        if (!hasTriggered)
        {
            GameObject hit = (GameObject)other;
            SplashDamage sd = hit.GetComponent<SplashDamage>();
            skillImpactDamage = sd.splashDamage;
            skillImpactElement = (int)sd.element;
            switch (skillImpactElement)
            {
                case 0:                 //스킬속성이 비전(Arcane)일 경우
                    if (element == MonsterElement.None)
                    {
                        hp_Point -= skillImpactDamage * 1.2f;
                    }
                    else if (element == 0)
                    {
                        hp_Point -= skillImpactDamage * 1.5f;
                    }
                    else
                    {
                        hp_Point -= skillImpactDamage;
                    }
                    break;
                case 1:                 //스킬속성이 화염(fire)일 경우
                    if (element == MonsterElement.Fire)
                    {
                        hp_Point -= skillImpactDamage * 0.5f;
                    }
                    else if (element == MonsterElement.Ice)
                    {
                        hp_Point -= skillImpactDamage * 1.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        igniteCount = 0;
                        igniteEffect.SetActive(true);
                        if (isIgnite == false)
                        {

                            StartCoroutine(Ignite());
                        }
                        hp_Point -= skillImpactDamage * 1.2f;
                    }
                    else
                    {
                        igniteCount = 0;
                        igniteEffect.SetActive(true);
                        if (isIgnite == false)
                        {

                            StartCoroutine(Ignite());
                        }
                        hp_Point -= skillImpactDamage;
                    }
                    break;
                case 2:                 //스킬속성이 냉기(ice)일 경우
                    if (element == MonsterElement.Fire)
                    {
                        hp_Point -= skillImpactDamage * 1.5f;
                    }
                    else if (element == MonsterElement.Ice)
                    {
                        hp_Point -= skillImpactDamage * 0.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        freezeCount = 0;
                        if (freezeMulti < 7 && isFreeze == true)    //냉기중첩
                        {
                            freezeMulti++;
                        }
                        if (isFreeze == false && isFrozen == false)
                        {
                            freezeMulti++;
                            freezeEffect.SetActive(true);
                            StartCoroutine(Freeze());
                        }
                        if (freezeMulti == 7)    //빙결상태 on
                        {
                            StopCoroutine(Freeze());
                            frozenEffect.SetActive(true);
                            isFreeze = false;
                            StartCoroutine(Frozen());
                        }
                        hp_Point -= skillImpactDamage * 1.2f;
                    }
                    else
                    {
                        //냉기상태 보류
                        hp_Point -= skillImpactDamage;
                    }
                    break;
                case 3:                 //스킬속성이 맹독(Poison)일 경우
                    if (element == MonsterElement.Fire)
                    {
                        toxicCount = 0;
                        if (toxicMulti < 5 && isToxic == true)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                        }
                        if (isToxic == false)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                            StartCoroutine(Toxic());
                        }
                        hp_Point -= skillImpactDamage * 1.2f;
                    }
                    else if (element == MonsterElement.Poison)
                    {
                        hp_Point -= skillImpactDamage * 0.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        toxicCount = 0;
                        if (toxicMulti < 5 && isToxic == true)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                        }
                        if (isToxic == false)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                            StartCoroutine(Toxic());
                        }
                        hp_Point -= skillImpactDamage * 1.2f;
                    }
                    else
                    {
                        toxicCount = 0;
                        if (toxicMulti < 5 && isToxic == true)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                        }
                        if (isToxic == false)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                            StartCoroutine(Toxic());
                        }
                        hp_Point -= skillImpactDamage;
                    }
                    break;
                case 4:                 //스킬속성이 번개(Lightning)일 경우
                    if (element == MonsterElement.Ice)
                    {
                        shockCount = 0;
                        if (shockMulti < 5 && isShock == true)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                        }
                        if (isShock == false)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                            StartCoroutine(Shock());
                        }
                        hp_Point -= (skillImpactDamage * 1.2f) * (1f + shockMulti * 0.1f);
                    }
                    else if (element == MonsterElement.Lightning)
                    {
                        hp_Point -= skillImpactDamage * 0.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        shockCount = 0;
                        if (shockMulti < 5 && isShock == true)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                        }
                        if (isShock == false)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                            StartCoroutine(Shock());
                        }
                        hp_Point -= (skillImpactDamage * 1.2f) * (1f + shockMulti * 0.1f);
                    }
                    else
                    {
                        shockCount = 0;
                        if (shockMulti < 5 && isShock == true)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                        }
                        if (isShock == false)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                            StartCoroutine(Shock());
                        }
                        hp_Point -= skillImpactDamage * (1f + shockMulti * 0.1f);
                    }
                    break;
                case 5:
                    hp_Point -= skillImpactDamage;
                    break;
            }
        }
    }


    [PunRPC]
    void Hit_ColliderSur(float damage, int skilltype)
    {
        switch (skilltype)
        {
            case 0:                 //스킬속성이 비전(Arcane)일 경우
                if (element == MonsterElement.None)
                {
                    hp_Point -= damage * 1.2f;
                }
                else if (element == 0)
                {
                    hp_Point -= damage * 1.5f;
                }
                else
                {
                    hp_Point -= damage;
                }
                break;
            case 1:                 //스킬속성이 화염(fire)일 경우
                if (element == MonsterElement.Fire)
                {
                    hp_Point -= damage * 0.5f;
                }
                else if (element == MonsterElement.Ice)
                {
                    hp_Point -= damage * 1.5f;
                }
                else if (element == MonsterElement.None)
                {
                    igniteCount = 0;
                    igniteEffect.SetActive(true);
                    if (isIgnite == false)
                    {

                        StartCoroutine(Ignite());
                    }
                    hp_Point -= damage * 1.2f;
                }
                else
                {
                    igniteCount = 0;
                    igniteEffect.SetActive(true);
                    if (isIgnite == false)
                    {

                        StartCoroutine(Ignite());
                    }
                    hp_Point -= damage;
                }
                break;
            case 2:                 //스킬속성이 냉기(ice)일 경우
                if (element == MonsterElement.Fire)
                {
                    hp_Point -= damage * 1.5f;
                }
                else if (element == MonsterElement.Ice)
                {
                    hp_Point -= damage * 0.5f;
                }
                else if (element == MonsterElement.None)
                {
                    freezeCount = 0;
                    if (freezeMulti < 7 && isFreeze == true)    //냉기중첩
                    {
                        freezeMulti++;
                    }
                    if (isFreeze == false && isFrozen == false)
                    {
                        freezeMulti++;
                        freezeEffect.SetActive(true);
                        StartCoroutine(Freeze());
                    }
                    if (freezeMulti == 7)    //빙결상태 on
                    {
                        StopCoroutine(Freeze());
                        frozenEffect.SetActive(true);
                        isFreeze = false;
                        StartCoroutine(Frozen());
                    }
                    hp_Point -= damage * 1.2f;
                }
                else
                {
                    //냉기상태 보류
                    hp_Point -= damage;
                }
                break;
            case 3:                 //스킬속성이 맹독(Poison)일 경우
                if (element == MonsterElement.Fire)
                {
                    toxicCount = 0;
                    if (toxicMulti < 5 && isToxic == true)
                    {
                        toxicMulti++;
                        toxicEffectList[toxicMulti - 1].SetActive(true);
                    }
                    if (isToxic == false)
                    {
                        toxicMulti++;
                        toxicEffectList[toxicMulti - 1].SetActive(true);
                        StartCoroutine(Toxic());
                    }
                    hp_Point -= damage * 1.2f;
                }
                else if (element == MonsterElement.Poison)
                {
                    hp_Point -= damage * 0.5f;
                }
                else if (element == MonsterElement.None)
                {
                    toxicCount = 0;
                    if (toxicMulti < 5 && isToxic == true)
                    {
                        toxicMulti++;
                        toxicEffectList[toxicMulti - 1].SetActive(true);
                    }
                    if (isToxic == false)
                    {
                        toxicMulti++;
                        toxicEffectList[toxicMulti - 1].SetActive(true);
                        StartCoroutine(Toxic());
                    }
                    hp_Point -= damage * 1.2f;
                }
                else
                {
                    toxicCount = 0;
                    if (toxicMulti < 5 && isToxic == true)
                    {
                        toxicMulti++;
                        toxicEffectList[toxicMulti - 1].SetActive(true);
                    }
                    if (isToxic == false)
                    {
                        toxicMulti++;
                        toxicEffectList[toxicMulti - 1].SetActive(true);
                        StartCoroutine(Toxic());
                    }
                    hp_Point -= damage;
                }
                break;
            case 4:                 //스킬속성이 번개(Lightning)일 경우
                if (element == MonsterElement.Ice)
                {
                    shockCount = 0;
                    if (shockMulti < 5 && isShock == true)
                    {
                        shockMulti++;
                        shockEffectList[shockMulti - 1].SetActive(true);
                    }
                    if (isShock == false)
                    {
                        shockMulti++;
                        shockEffectList[shockMulti - 1].SetActive(true);
                        StartCoroutine(Shock());
                    }
                    hp_Point -= (damage * 1.2f) * (1f + shockMulti * 0.1f);
                }
                else if (element == MonsterElement.Lightning)
                {
                    hp_Point -= damage * 0.5f;
                }
                else if (element == MonsterElement.None)
                {
                    shockCount = 0;
                    if (shockMulti < 5 && isShock == true)
                    {
                        shockMulti++;
                        shockEffectList[shockMulti - 1].SetActive(true);
                    }
                    if (isShock == false)
                    {
                        shockMulti++;
                        shockEffectList[shockMulti - 1].SetActive(true);
                        StartCoroutine(Shock());
                    }
                    hp_Point -= (damage * 1.2f) * (1f + shockMulti * 0.1f);
                }
                else
                {
                    shockCount = 0;
                    if (shockMulti < 5 && isShock == true)
                    {
                        shockMulti++;
                        shockEffectList[shockMulti - 1].SetActive(true);
                    }
                    if (isShock == false)
                    {
                        shockMulti++;
                        shockEffectList[shockMulti - 1].SetActive(true);
                        StartCoroutine(Shock());
                    }
                    hp_Point -= damage * (1f + shockMulti * 0.1f);
                }
                break;
            case 5:
                hp_Point -= damage;
                break;
        }
    }
    #endregion
    private void Update()
    {
        try
        {
            //#region 속성효과
            //SetIgnite();
            //SetFreeze();
            //SetToxic();
            //SetShock();
            //#endregion
            GenerateHpAndMp();
            //ShowManaSlider();
            ShowHpToSlider();
            if (hp_Point <= 0)
            {
                pv.RPC("die", RpcTarget.AllBuffered);
            }
        }
        catch
        {
            Debug.Log("캐릭스텟 업뎃오류");
        }
    }
    [PunRPC]
    void die()
    {
        StartCoroutine("TimeDie");
       // Destroy(gameObject, 2f);
    }
    IEnumerator TimeDie()
    {
        yield return new WaitForSeconds(3f);
        //PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("startscene");
    }
    void GenerateHpAndMp()
    {
        if (mana_Point < max_Mana)
            mana_Point += Time.deltaTime * 5f;
        else
            mana_Point = max_Mana;

        if (hp_Point < max_Hp)
            hp_Point += Time.deltaTime * 1f;
        else
            hp_Point = max_Hp;
    }

    #region UI 관련 메서드
    void ShowHpToSlider()
    {
        SliderValueReciver svr = hpSlider.GetComponent<SliderValueReciver>();
        svr.SliderValue = hp_Point;
    }
    void ShowManaSlider()
    {
        SliderValueReciver svr = manaSlider.GetComponent<SliderValueReciver>();
        svr.SliderValue = mana_Point;
    }
    #endregion
    #region 속성효과 메서드
    void SetIgnite()
    {
        if (igniteCheck == true)
        {
            StartCoroutine(Ignite());


        }
        if (isIgnite == false)
        {
            igniteEffect.SetActive(false);
        }
    }

    void SetFreeze()
    {
        if (freezeCheck == true)
        {
            StartCoroutine(Freeze());
        }
        if (isFreeze == true)
        {
            //냉기상태일시 decreaseMoveSpeed, decreaseAttackSpeed를 적용
        }
        if (isFreeze == false && freezeEffect)
        {
            freezeEffect.SetActive(false);
        }
        if (isFrozen == false)
        {
            StopCoroutine(Frozen());
        }
    }

    void SetToxic()
    {
        if (toxicCheck)
        {
            StartCoroutine(Toxic());
        }
        if (isToxic == false)
        {
            for (int i = 0; i < 5; i++)
            {
                toxicEffectList[i].SetActive(false);
            }
        }
    }

    void SetShock()
    {
        if (shockCheck)
        {
            StartCoroutine(Shock());
        }
        if (isShock == false)
        {
            for (int i = 0; i < 5; i++)
            {
                shockEffectList[i].SetActive(false);
            }
        }
    }

    #endregion

    #region 속성효과 코루틴
    IEnumerator Ignite()
    {
        isIgnite = true;
        igniteCheck = false;
        igniteCount++;
        hp_Point -= skillProjectileDamage * 1f;

        yield return new WaitForSeconds(igniteCool);

        igniteCheck = true;
        if (igniteCount >= 5)
        {
            igniteCount = 0;
            isIgnite = false;
            igniteCheck = false;
            StopCoroutine(Ignite());
        }
    }

    IEnumerator Freeze()
    {
        isFreeze = true;
        freezeCheck = false;
        freezeCount++;
        if (freezeMulti < 7)
        {
            decreaseMoveSpeed = move_Speed * (1 - 0.05f * freezeMulti);
        }
        yield return new WaitForSeconds(freezeCool);

        freezeCheck = true;
        if (freezeCount >= 2)
        {
            freezeCount = 0;
            isFreeze = false;
            freezeCheck = false;
            freezeMulti = 0;
            StopCoroutine(Freeze());
        }
    }

    IEnumerator Toxic()
    {
        isToxic = true;
        toxicCheck = false;
        toxicCount++;
        hp_Point -= skillProjectileDamage * 0.3f * toxicMulti;

        yield return new WaitForSeconds(toxicCool);

        toxicCheck = true;
        if (toxicCount >= 5)
        {
            toxicCount = 0;
            isToxic = false;
            toxicCheck = false;
            toxicMulti = 0;
            StopCoroutine(Toxic());
        }
    }

    IEnumerator Shock()
    {
        isShock = true;
        shockCheck = false;
        shockCount++;

        yield return new WaitForSeconds(shockCool);

        shockCheck = true;
        if (shockCount >= 5)
        {
            shockCount = 0;
            isShock = false;
            shockCheck = false;
            shockMulti = 0;
            StopCoroutine(Shock());
        }
    }

    IEnumerator Frozen()
    {
        decreaseMoveSpeed = 0;
        decreaseAttackSpeed = 0;
        isFrozen = true;
        frozenCheck = false;
        frozenCount++;

        yield return new WaitForSeconds(frozenCool);

        frozenCheck = true;
        if (frozenCount >= 1)
        {
            frozenEffect.SetActive(false);
            frozenCount = 0;
            isFrozen = false;
            frozenCheck = false;
            StopCoroutine(Frozen());
        }
    }
    #endregion
    

   
   
    
}