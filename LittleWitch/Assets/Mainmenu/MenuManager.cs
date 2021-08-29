using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject Mainpanel;
    public GameObject Settingpanel;

    public GameObject datasave;
    public Text user_name;

    public GameObject buildname_Panel;
    public GameObject multi_Panel;

    private void Start()
    {
    }
    public void StartGameScene()
    {
        SceneManager.LoadScene("DungeonScene");
    }

    public void ToSetting()
    {
        Mainpanel.SetActive(false);
        Settingpanel.SetActive(true);
    }
    public void ToMain()
    {
        Mainpanel.SetActive(true);
        Settingpanel.SetActive(false);

    }
    public void Build_Name_Panel()
    {
        multi_Panel.SetActive(false);
        buildname_Panel.SetActive(true);
    }
    public void Back_multi()
    {
        buildname_Panel.SetActive(false);
        multi_Panel.SetActive(true);
    }
    public void Name_Save()
    {
        string name = user_name.text;
        datasave.GetComponent<MultiSkillSelect>().username = name;
        buildname_Panel.SetActive(false);
        multi_Panel.SetActive(true);
    }
}
