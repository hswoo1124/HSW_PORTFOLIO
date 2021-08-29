using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//모든 스킬잼들의 부모클래스

public class SkillJam : MonoBehaviour
{
    [SerializeField]
    Sprite skill_Icon;
    [SerializeField]
    int skill_Id;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SetSkillUI ssu = GameObject.Find("SetSkillUI").GetComponent<SetSkillUI>();
            ssu.GetSkillJam(skill_Id);
            GameObject skillImageObject = this.transform.GetChild(0).gameObject;
            skill_Icon = skillImageObject.GetComponent<SpriteRenderer>().sprite;
            ssu.GetSkillIcon(skill_Icon);
            ssu.ShowUI();

            Destroy(this.gameObject, 0.1f);
        }
    }

}
