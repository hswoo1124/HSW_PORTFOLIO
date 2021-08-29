using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSkill_Icon : MonoBehaviour
{
    [SerializeField]
    GameObject skill_set;

    public GameObject[] icons1;
    public GameObject[] icons2;
    public GameObject[] icons3;

    int skill1;
    int skill2;
    int skill3;
    // Start is called before the first frame update
    void Start()
    {
        skill_set = GameObject.Find("Multi_Skill_Select");
        skill1 = skill_set.GetComponent<MultiSkillSelect>().selectSkillId_List[0];
        skill2 = skill_set.GetComponent<MultiSkillSelect>().selectSkillId_List[1];
        skill3 = skill_set.GetComponent<MultiSkillSelect>().selectSkillId_List[2];
        Icon_Change();
    }

    void Icon_Change()
    {
        icons1[skill1].SetActive(true);
        icons2[skill2].SetActive(true);
        icons3[skill3].SetActive(true);
    }
 
}
