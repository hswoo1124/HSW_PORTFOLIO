using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class Photon_PlayerSpawn : MonoBehaviourPunCallbacks//, IPunObservable
{
    public List<GameObject> players = new List<GameObject>();
    public List<GameObject> players_ui = new List<GameObject>();
    public Transform[] spawnpoint;
    [SerializeField]
    PhotonView pv;

    public Text countdown;
    [SerializeField]
    GameObject screen;
    int countTime = 60;

    bool countcheck = false;
    bool ccheck = false;
    [SerializeField]
    float player1_hp;
    [SerializeField]
    float player2_hp;
    [SerializeField]
    GameObject mycharacter;
    [SerializeField]
    GameObject enemycharacter;
    [SerializeField]
    int endtime = 120;
    bool endcountcheck = false;

    [SerializeField]
    GameObject skill_set;


    

    private void Start()
    {
        OnJoinedRoom();
        
    }

    //호스트가 들어오면
    public override void OnJoinedRoom()
    {
        CreatePlayer();
        //Debug.Log(player_count);
    }


    void CreatePlayer()
    {
        try
        {
            //if (player_count == 0)
            //{
            // 플레이어, ui조이스틱 소환-------------------------
            skill_set = GameObject.Find("Multi_Skill_Select");
            //PhotonNetwork.NickName = skill_set.GetComponent<MultiSkillSelect>().username;
        int idx = 0;
        Debug.Log(PhotonNetwork.PlayerList.Length);
        if (PhotonNetwork.PlayerList.Length > 1)
        {
            Debug.Log("d");
            idx = 1;
        }

        GameObject player = PhotonNetwork.Instantiate("NetWorkPlayer", spawnpoint[idx].position, Quaternion.identity);
        player.transform.parent = transform;
        GameObject player_ui = PhotonNetwork.Instantiate("NetWorkInGameUi", Vector3.zero, Quaternion.identity);
        player_ui.transform.parent = transform;
        Text player_name = player.transform.GetChild(2).GetChild(0).GetChild(4).gameObject.GetComponent<Text>();
        
        pv = player.GetComponent<PhotonView>();


        Debug.Log(pv.ViewID);
        //---------------------------------------------------
        //해당 플레이어와 조이스틱 연동---
        //if (pv.IsMine)
        //{
        if(player.GetComponent<PhotonView>().IsMine)
        {
            mycharacter = player;
            NetWork_ShootingManager sm1 = player.transform.GetChild(4).gameObject.GetComponent<NetWork_ShootingManager>();
            sm1.slot1_SkillId = skill_set.GetComponent<MultiSkillSelect>().selectSkillId_List[0];
            sm1.slot2_SkillId = skill_set.GetComponent<MultiSkillSelect>().selectSkillId_List[1];
            sm1.slot3_SkillId = skill_set.GetComponent<MultiSkillSelect>().selectSkillId_List[2];
        }

            player_name.text = player.GetComponent<PhotonView>().IsMine ? PhotonNetwork.NickName : player.GetComponent<PhotonView>().Owner.NickName;
            player_name.color = player.GetComponent<PhotonView>().IsMine ? Color.white : Color.red;

            if (pv.ViewID == 1001)
        {
            player.name = "player";
            player_ui.name = "playerj_ui";
        }
        else if (pv.ViewID == 2001)
        {
            player.name = "player";
            player_ui.name = "playerj_ui";
        }
        NetWork_PlayerMovement pm = player.GetComponent<NetWork_PlayerMovement>();
        pm.joystick = player_ui.transform.GetChild(0).GetChild(0).gameObject.GetComponent<MovementJoystick>();
        pm.sJoystick = player_ui.transform.GetChild(0).GetChild(3).gameObject.GetComponent<FixedShootingJoystick>();
        NetWork_RotationWeaponCenter rwc = player.transform.GetChild(3).gameObject.GetComponent<NetWork_RotationWeaponCenter>();
        rwc.uiCenterPos = player_ui.transform.GetChild(0).GetChild(3).GetChild(1);
        //플레이어 안에있는 firemanager 안의 슈팅매니저에 슬라이더 넣어주기 -----
        NetWork_ShootingManager sm = player.transform.GetChild(4).gameObject.GetComponent<NetWork_ShootingManager>();
        sm.sjs = player_ui.transform.GetChild(0).GetChild(3).gameObject.GetComponent<FixedShootingJoystick>(); //sjs잡아주기
        sm.chargeSlider = player_ui.transform.GetChild(1).GetChild(0).GetChild(1).gameObject;
        //슈팅조이스틱에 슈팅매니저 넣어주기
        FixedShootingJoystick fsj = player_ui.transform.GetChild(0).GetChild(3).gameObject.GetComponent<FixedShootingJoystick>();
        fsj.networkshootingmanager = sm;
        fsj.networkplayermovement = pm;
        MovementJoystick mjs = player_ui.transform.GetChild(0).GetChild(0).gameObject.GetComponent<MovementJoystick>();
        mjs.networkplayermovement = pm;

        NetWork_RotationArms nra = player.transform.GetChild(1).GetChild(3).gameObject.GetComponent<NetWork_RotationArms>();
        nra.s_StickRotation = player_ui.transform.GetChild(0).GetChild(3).GetChild(1).gameObject;
        //스킬버튼 원클릭에 함수 채우기 + 슈팅매니저 cooluilist 담기----------------------------------------------
        for (int i = 0; i < player_ui.transform.GetChild(0).GetChild(2).childCount; i++)
        {
            sm.coolUIList[i] = player_ui.transform.GetChild(0).GetChild(2).GetChild(i).GetChild(2).gameObject;
        }
        //--------------------------------------------------------------------------------------------------------
        #region  버튼 함수넣기 
        Button skill_Buttons1, skill_Buttons2, skill_Buttons3;
        skill_Buttons1 = player_ui.transform.GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<Button>();
        skill_Buttons1.onClick.AddListener(() => sm.ClickSkill1Button());//sm.SelectSkill(0));

        skill_Buttons2 = player_ui.transform.GetChild(0).GetChild(2).GetChild(1).gameObject.GetComponent<Button>();
        skill_Buttons2.onClick.AddListener(() => sm.ClickSkill2Button());//sm.SelectSkill(1));

        skill_Buttons3 = player_ui.transform.GetChild(0).GetChild(2).GetChild(2).gameObject.GetComponent<Button>();
        skill_Buttons3.onClick.AddListener(() => sm.ClickSkill3Button());//sm.SelectSkill(2));
        #endregion
        //리스트에 담기.
        players.Add(player);
        players_ui.Add(player_ui);
        //}
    }     
        catch
        {
            Debug.Log("플레이어 소환 오류");
        }
    }
    private void Update()
    {
        try
        {
            //if (pv.IsMine)
            //{
            //  player1_hp = GameObject.Find("PlayerSpawnPoint").transform.GetChild(2).gameObject.GetComponent<NetWork_CharacterStatus>().hp_Point;
            if(enemycharacter == null)
            {
                enemycharacter = GameObject.Find("NetWorkPlayer(Clone)");
                enemycharacter.transform.GetChild(2).GetChild(0).GetChild(4).gameObject.GetComponent<Text>().text = enemycharacter.GetComponent<PhotonView>().Owner.NickName;
                enemycharacter.transform.GetChild(2).GetChild(0).GetChild(4).gameObject.GetComponent<Text>().color = Color.red;
            }
            player1_hp = mycharacter.GetComponent<NetWork_CharacterStatus>().hp_Point;
            player2_hp = enemycharacter.GetComponent<NetWork_CharacterStatus>().hp_Point;
            if (enemycharacter != null)
            {
                ccheck = true;
            }
        
        if (PhotonNetwork.PlayerList.Length >= 2 && countcheck == false && ccheck == true)
        {
                countdown.text = string.Format("{0}", countTime);
                screen.SetActive(false);
                //countdown.gameObject.SetActive(true);
         StartCoroutine("BattleCount");
        }

        if(player1_hp < 0)
        {
                ccheck = false;
                screen.SetActive(true);
                countdown.text = "패배";
                StopCoroutine("BattleCount");
                //if (endcountcheck == false)
                //{
                //    StartCoroutine("EndCount");
                //}
            }
        else if(player2_hp < 0)
        {
                ccheck = false;
                screen.SetActive(true);
                countdown.text = "승리!!";
                StopCoroutine("BattleCount");
                //if (endcountcheck == false)
                //{
                //    StartCoroutine("EndCount");
                //}
            }

            if(countTime == 0)
            {
                ccheck = false;
                if(player1_hp > player2_hp)
                {
                    screen.SetActive(true);
                    countdown.text = "승리!!";
                    StopCoroutine("BattleCount");
                    //if (endcountcheck == false)
                    //{
                    //    StartCoroutine("EndCount");
                    //}
                }
                else if(player2_hp < player1_hp)
                {
                    screen.SetActive(true);
                    countdown.text = "패배";
                    StopCoroutine("BattleCount");
                    //if (endcountcheck == false)
                    //{
                    //    StartCoroutine("EndCount");
                    //}
                }
                else if(player1_hp == player2_hp)
                {
                    screen.SetActive(true);
                    countdown.text = "무승부";
                    StopCoroutine("BattleCount");
                    //if (endcountcheck == false)
                    //{
                    //    StartCoroutine("EndCount");
                    //}
                }
            }
    }
        catch
        {
            Debug.Log("업뎃카운트");
        }
    }

    IEnumerator BattleCount()
    {
        if (countTime != 0)
        {
            countcheck = true;
            countTime--;
            yield return new WaitForSeconds(1f);
            countcheck = false;
        }
        else if (countTime == 0)
        {
            StopCoroutine("BattleCount");
        }
    }
    //IEnumerator EndCount()
    //{
    //    if (endtime != 0)
    //    {
    //        endcountcheck = true;
    //        endtime--;
    //        yield return new WaitForSeconds(1f);
    //        endcountcheck = false;
    //    }
    //    else if (endtime == 0)
    //    {
    //        StopCoroutine("EndCount");
    //        //PhotonNetwork.LeaveRoom();
    //        //SceneManager.LoadScene("SampleScene");
    //    }
    //}

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(player1_hp);
    //    }
    //    else
    //    {
    //        player2_hp = (float)stream.ReceiveNext();
    //    }
    //}
}
