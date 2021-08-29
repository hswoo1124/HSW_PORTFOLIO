using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breath : MonoBehaviour
{
    public GameObject breathFront;
    public GameObject breathLeft45;
    public GameObject breathLeft90;
    public GameObject breathRight45;
    public GameObject breathRight90;
   
    int num;                //BigBoss 스크립에서 앵글값 받아오기
    float timer = 0;        //발사 시간

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        var skillnum = GameObject.Find("BigBoss").GetComponent<Bigbosscontrol>(); //BigBoss게임 오브젝트의 BigBoss스크립트 등록

        num = skillnum.breathControler; //BigBoss스크립트안의 breathAngle 값 가져오기

        timer += Time.deltaTime;    //발사 시간 세기

        SkillRotation(num); //스킬 사용 함수에 등록

        if (timer > 2.0f)   //발사 시간이 2초 초과시
        {
            skillnum.faceControler = 1;

            timer = 0;
            gameObject.SetActive(false);    //오브젝트 비활성화
        }
    }

    void SkillRotation(int skillangle)
    {
        if(skillangle == 1)                 //현재 플레이어 위치가 1번구역일 때 위칫값
        {
            breathFront.gameObject.SetActive(false);
            breathLeft45.gameObject.SetActive(false);
            breathLeft90.gameObject.SetActive(true);
            breathRight45.gameObject.SetActive(false);
            breathRight90.gameObject.SetActive(false);

            //this.transform.position = new Vector3(8.6f, 2.3f, 16.3f);   
            // this.transform.rotation = Quaternion.Euler(new Vector3(-11.3f, 230, 73));
        }
        else if (skillangle == 2)                 //현재 플레이어 위치가 1번구역일 때 위칫값
        {
            breathFront.gameObject.SetActive(false);
            breathLeft45.gameObject.SetActive(true);
            breathLeft90.gameObject.SetActive(false);
            breathRight45.gameObject.SetActive(false);
            breathRight90.gameObject.SetActive(false);
            //this.transform.position = new Vector3(8.6f, 2.3f, 16.3f);
            //this.transform.rotation = Quaternion.Euler(new Vector3(4.2f, 212, 7));
        }
        else if (skillangle == 3)                 //현재 플레이어 위치가 1번구역일 때 위칫값
        {
            breathFront.gameObject.SetActive(true);
            breathLeft45.gameObject.SetActive(false);
            breathLeft90.gameObject.SetActive(false);
            breathRight45.gameObject.SetActive(false);
            breathRight90.gameObject.SetActive(false);
            // this.transform.position = new Vector3(10, 2.3f, 16.3f);
            //this.transform.rotation = Quaternion.Euler(new Vector3(7, 183, -3.5f));
        }
        else if (skillangle == 4)                 //현재 플레이어 위치가 1번구역일 때 위칫값
        {
            breathFront.gameObject.SetActive(false);
            breathLeft45.gameObject.SetActive(false);
            breathLeft90.gameObject.SetActive(false);
            breathRight45.gameObject.SetActive(true);
            breathRight90.gameObject.SetActive(false);
            //this.transform.position = new Vector3(10f, 2.3f, 16.3f);
            //this.transform.rotation = Quaternion.Euler(new Vector3(7, 140, -3.5f));
        }
        else if (skillangle == 5)                 //현재 플레이어 위치가 1번구역일 때 위칫값
        {
            breathFront.gameObject.SetActive(false);
            breathLeft45.gameObject.SetActive(false);
            breathLeft90.gameObject.SetActive(false);
            breathRight45.gameObject.SetActive(false);
            breathRight90.gameObject.SetActive(true);
            //this.transform.position = new Vector3(8.6f, 2.3f, 16.3f);
            // this.transform.rotation = Quaternion.Euler(new Vector3(1.7f, 120, -3.8f));
        }
        else
        {

        }
    }
}
