using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSet : MonoBehaviour
{
    public AudioMixer bgmMixer;

    public Slider masterSli;
    public Text masterTex;

    public Slider backSli;
    public Text backTex;

    public Slider effectSli;
    public Text effectTex;

    [HideInInspector]
    public float masterVol;
    [HideInInspector]
    public float bgmVol;
    [HideInInspector]
    public float effectVol;


    public void LoadSoundSetting()
    {
        Setmaster();
        Setbackground();
        Seteffect();

        Debug.Log("사운드 로드완료");
    }

    public void Setmaster()
    {
        masterVol = masterSli.value; // 슬라이더 변수를 가져온다.
        masterTex.text = (int)(masterVol * 100) + " / " + 100; // 볼륨 정수로 표현
        bgmMixer.SetFloat("MasterMusicVol", Mathf.Log(masterVol) * 20); // MsterVolume에 해당하는 믹서 볼륨 조절
    }

    public void Setbackground()
    {
        bgmVol = backSli.value; // 슬라이더 변수를 가져온다.
        backTex.text = (int)(bgmVol * 100) + " / " + 100; // 볼륨 정수로 표현
        bgmMixer.SetFloat("BackMusicVol", Mathf.Log(bgmVol) * 20); // MsterVolume에 해당하는 믹서 볼륨 조절
    }

    public void Seteffect()
    {
        effectVol = effectSli.value; // 슬라이더 변수를 가져온다.
        effectTex.text = (int)(effectVol * 100) + " / " + 100; // 볼륨 정수로 표현
        bgmMixer.SetFloat("EffectMusicVol", Mathf.Log(effectVol) * 20); // MsterVolume에 해당하는 믹서 볼륨 조절
    }
}
