using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public int selectMultiSkill1 = -1;
    public int selectMultiSkill2;
    public int selectMultiSkill3;

    [SerializeField]
    VolumeSet volumeSet;
    [SerializeField]
    MultSkillPresetUI multiSkillPresetUI;

    [SerializeField]
    GameObject saveWarning;
    [SerializeField]
    GameObject selectCanvas;
    [SerializeField]
    GameObject multiCanvas;
    //----------------------------------------------------------------
    [SerializeField]
    GameObject multiskillselect;


    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        LoadPlayerPrefsData();
    }

    public void SaveSoundData()
    {
        PlayerPrefs.SetFloat("masterSoundVol", volumeSet.masterVol);
        PlayerPrefs.SetFloat("bgmSoundVol", volumeSet.bgmVol);
        PlayerPrefs.SetFloat("effectSoundVol", volumeSet.effectVol);
        PlayerPrefs.SetString("isSoundSave", "soundSaved");

        Debug.Log("사운드 세이브 시도");
    }

    public void SaveSkillPresetData()
    {
        if(multiSkillPresetUI.selectedSkillCount==3)
        {
            for (int i = 0; i < 5; i++)
            {
                if (multiSkillPresetUI.isSelected[i])
                {
                    PlayerPrefs.SetInt("sms" + i.ToString(), 1);
                    multiskillselect.GetComponent<MultiSkillSelect>().selectSkillId_List.Add(i);
                }
                else
                {
                    PlayerPrefs.SetInt("sms" + i.ToString(), 0);
                }
            }

            PlayerPrefs.SetString("isSkillPresetSave", "skillPresetSaved");
            
            saveWarning.SetActive(false);
            selectCanvas.SetActive(false);
            multiCanvas.SetActive(true);
            Debug.Log("스킬프리셋 세이브 시도");
        }
        else
        {
            saveWarning.SetActive(false);
            saveWarning.SetActive(true);
            
        }
        
    }

    public void LoadPlayerPrefsData()
    {
        Debug.Log("로딩시도");
        #region 사운드 로드
        if(PlayerPrefs.HasKey("isSoundSave"))
        {
            volumeSet.masterSli.value = PlayerPrefs.GetFloat("masterSoundVol");
            volumeSet.backSli.value = PlayerPrefs.GetFloat("bgmSoundVol");
            volumeSet.effectSli.value = PlayerPrefs.GetFloat("effectSoundVol");
            volumeSet.LoadSoundSetting();
        }
        #endregion

        #region 스킬 프리셋 로드
        if(PlayerPrefs.HasKey("isSkillPresetSave"))
        {
            
            for (int i = 0; i < 5; i++)
            {
                int getInt = 0;
                getInt = PlayerPrefs.GetInt("sms" + i.ToString());
                if (getInt == 1)
                {
                    multiSkillPresetUI.isSelected[i] = true;
                }
                else if (getInt == 0)
                {
                    multiSkillPresetUI.isSelected[i] = false;
                }
            }
            multiSkillPresetUI.LoadSkillPreset();
        }
        #endregion

     }
}
