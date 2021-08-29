using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSkillUI : MonoBehaviour
{
    public int sId //스킬아이디
    {
        get; set;
    }
    public Sprite sIcon //스킬아이디
    {
        get; set;
    }

    [SerializeField]
    GameObject[] setSkillUIList;

    [SerializeField]
    GameObject[] skillIconList_gameobject;
    [SerializeField]
    Image[] skillIconList;
    [SerializeField]
    ShootingManager sm;
    bool getSkill;

    public void ShowUI()
    {
        for(int i = 0; i < 4; i++)
        {
            setSkillUIList[i].SetActive(true);
        }
    }
    public void HideUI()
    {
        for (int i = 0; i < 4; i++)
        {
            setSkillUIList[i].SetActive(false);
        }
    }
    public void GetSkillJam(int skillId)    //스킬젬 획득시 해당 스킬번호 획득
    {
        sId = skillId;
    }
    public void GetSkillIcon(Sprite icon)    //스킬젬 획득시 해당 스킬번호 획득
    {
        sIcon = icon;
    }
    public void SetSkill(int slotNum)  //스킬추가 버튼 클릭시 해당 슬롯에 스킬추가
    {
        sm = GameObject.Find("FireManager").GetComponent<ShootingManager>();
        switch (slotNum)
        {
            case 0:
                sm.slot1_SkillId = sId;
                break;
            case 1:
                sm.slot2_SkillId = sId;
                break;
            case 2:
                sm.slot3_SkillId = sId;
                break;
        }

        skillIconList_gameobject[slotNum].SetActive(true);
        skillIconList[slotNum].sprite = sIcon;
        HideUI();
    }


}
