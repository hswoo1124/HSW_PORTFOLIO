using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//던전 생성 로딩바이며 씬 로딩 바와는 관계없음
public class LoadingBar : MonoBehaviour
{

    [SerializeField]
    private GameObject sliderValueTarget;
    public float maxValue = 100;
    [SerializeField]
    public float sliderValue = 0f;
    float accelateTime = 0f;

    [SerializeField]
    GameObject loadingUI;

    [SerializeField]
    Transform dungeonEntrancePos;
    [SerializeField]
    Transform playerTransform;

    [SerializeField]
    Text loadingKText;

    [SerializeField]
    GameObject dungeonEntrance;

    float fsliderV = 0f;
    float ssliderV = 0f;
    float tsliderV = 0f;
    float valueSpacing = 10f;
    [SerializeField]
    float loadingDelayT = 0f;

    bool isLoadingText;

    private void Awake()
    {
        loadingDelayT = 0f;
        transform.GetComponent<Slider>().value = sliderValue;
        fsliderV = sliderValue + valueSpacing;
        ssliderV = fsliderV + valueSpacing;
        tsliderV = ssliderV + valueSpacing;
    }

    void FixedUpdate()
    {
        if (this.enabled)
        {
            UpdateLoadingValue();
            ShowValuePercentage();
            //ShowLoadingText();
            CheckDoorError();
            if(isLoadingText == false)
                StartCoroutine(LoadingText());
        }
        
    }

    void UpdateLoadingValue()
    {
        
        if (sliderValue < 100)
        {
            accelateTime = sliderValue*0.1f;
            sliderValue += Time.deltaTime + accelateTime;
        }
        if(sliderValue >= 20f)
        {
            playerTransform.position = new Vector3(dungeonEntrancePos.position.x, dungeonEntrancePos.position.y, dungeonEntrancePos.position.z-1f);
        }
        if(sliderValue >= 40f)
        {
            StageManager sm = GameObject.Find("StageManager").GetComponent<StageManager>();
            sm.SpawnMonster();
        }
        if (sliderValue > 100f)
        {
            sliderValue = 100f;
            PlayerMovement pm = FindObjectOfType<PlayerMovement>().GetComponent<PlayerMovement>();
            pm.t_moveAble = true;
            loadingUI.SetActive(false);
            sliderValue = 0;
            loadingDelayT = 0;
            DungeonEntrance de = dungeonEntrance.GetComponent<DungeonEntrance>();
            de.isSetPosition = true;
        }
    }
    void ShowLoadingText()
    {
        
        if (sliderValue < tsliderV)
        {
            loadingKText.text = "던전 생성 중...";
            if (sliderValue < ssliderV)
            {
                loadingKText.text = "던전 생성 중..";
                if (sliderValue < fsliderV)
                {
                    loadingKText.text = "던전 생성 중.";
                }
            }
        }
        if(sliderValue > tsliderV)
        {
            valueSpacing += valueSpacing;
            fsliderV = sliderValue + valueSpacing;
            ssliderV = fsliderV + valueSpacing;
            tsliderV = ssliderV + valueSpacing;
        }
    }
    void ShowValuePercentage()
    {
        transform.GetComponent<Slider>().value = sliderValue;

        sliderValueTarget.GetComponent<Text>().text = ((int)sliderValue).ToString() + "%";
    }

    void CheckDoorError()
    {
        loadingDelayT += Time.deltaTime * 1f;
        if (loadingDelayT > 2f)
        {
            if (sliderValue < 20f)
            {
                StageManager sm = GameObject.Find("StageManager").GetComponent<StageManager>();
                sm.ResetMap();
                loadingDelayT = 0f;
            }
            
        }
    }

    IEnumerator LoadingText()
    {
        isLoadingText = true;

        loadingKText.text = "던전 생성 중.";

        yield return new WaitForSeconds(0.6f);

        loadingKText.text = "던전 생성 중..";

        yield return new WaitForSeconds(0.6f);

        loadingKText.text = "던전 생성 중...";

        yield return new WaitForSeconds(0.6f);
        
        StopCoroutine(LoadingText());

        isLoadingText = false;
    }
}