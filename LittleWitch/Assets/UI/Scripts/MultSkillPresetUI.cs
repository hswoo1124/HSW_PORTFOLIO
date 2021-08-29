using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultSkillPresetUI : MonoBehaviour
{
    public GameObject[] selectedSkillUI_List;
    public Button[] skillPresetBtn_List;
    [HideInInspector]
    public List<int> selectSkillId_List = new List<int>();

    public bool[] isSelected;
    [HideInInspector]
    public int selectedSkillCount = 0;


    private void Awake()
    {
    }

    public void LoadSkillPreset()
    {
        for (int i = 0; i < 5; i++)
        {
            if(isSelected[i])
            {
                selectedSkillUI_List[i].SetActive(true);
                skillPresetBtn_List[i].enabled = true;

                selectSkillId_List.Add(i);
            }
            else
            {
                selectedSkillUI_List[i].SetActive(false);
                skillPresetBtn_List[i].enabled = false;
            }

        }
        selectedSkillCount = 3;
        Debug.Log("스킬프리셋 로드완료");
    }

    public void SelectSkill(int skillPresetNum)
    {
        if(isSelected[skillPresetNum])
        {
            selectedSkillCount--;
            isSelected[skillPresetNum] = false;
            selectedSkillUI_List[skillPresetNum].SetActive(false);
        }
        else
        {
            selectedSkillCount++;
            isSelected[skillPresetNum] = true;
            selectedSkillUI_List[skillPresetNum].SetActive(true);
        }
        CheckSelectedSkillPreset();
    }

    void CheckSelectedSkillPreset()
    {
        if(selectSkillId_List.Count>0)
            selectSkillId_List.Clear();
        if (selectedSkillCount >= 3)
        {
            for(int i = 0; i < skillPresetBtn_List.Length; i++)
            {
                if (!isSelected[i])
                {
                    skillPresetBtn_List[i].enabled = false;

                }
                else
                {
                    selectSkillId_List.Add(i);
                }
            }
        }
        else
        {
            for (int i = 0; i < skillPresetBtn_List.Length; i++)
            {
                skillPresetBtn_List[i].enabled = true;
            }
        }
    }
}
