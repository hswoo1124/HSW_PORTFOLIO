using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class PhotonNetworkConnect : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private string gameversion = "1";
    public Text connectionInfoText;
    public Button joinButton;

    [SerializeField]
    bool check_net = false;
    [SerializeField]
    bool check_skillset = false;

    [SerializeField]
    GameObject skill_set;


    // Start is called before the first frame update
    void Start()
    {
        //게임 접속에 필요한 정보(게임정보) 설정
        PhotonNetwork.GameVersion = gameversion;

        //설정한 정보를 가지고 마스터 서버 접속 시도
        PhotonNetwork.ConnectUsingSettings();

        //룸 접속 버튼을 잠시 비활성화
        check_net = false;

        //접속을 시도중임을 텍스트로 표시
        connectionInfoText.text = "마스터 서버에 접속중....";
    }
    private void Update()
    {
        if(skill_set.GetComponent<MultiSkillSelect>().selectSkillId_List.Count == 3)
        {
            check_skillset = true;
        }
        else
        {
            check_skillset = false;
        }

        if(check_net == true && check_skillset == true)
        {
            joinButton.interactable = true;
        }
        else
        {
            joinButton.interactable = false;
        }

    }

    //마스터 서버 접속시 자동 실행
    public override void OnConnectedToMaster()
    {
        //룸 접속 버튼 활성화
        check_net = true;

        //접속완료를  텍스트로 표시
        connectionInfoText.text = "온라인: 마스터 서버에 접속";
    }
    //마스터 서버 접속 실패시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        //룸 접속 버튼 비활성화
        check_net = false;

        //재접속을 시도중임을 텍스트로 표시
        connectionInfoText.text = "오프라인: 마스터 서버에 접속불가..재시도중";

        //접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    //룸 접속 시도
    public void Connect()
    {
        //중복접속 시도 방지용 버튼 비활성화
        check_net = false;

        //마스터 서버에 접속중 이라면
        if (PhotonNetwork.IsConnected)
        {
            try
            {
                //랜덤 룸에 접속 시도
                connectionInfoText.text = "룸에 접속...";
                PhotonNetwork.NickName = skill_set.GetComponent<MultiSkillSelect>().username;
                PhotonNetwork.JoinRandomRoom();
            }
            catch
            {
                Debug.Log("방 접속 오류");
                connectionInfoText.text = "접속오류_닉네임";
            }
        }
        else
        {
            //마스터 서버에 접속중이 아니라면  마스터 서버에 재접속 시도
            connectionInfoText.text = "오프라인: 마스터서버와 연결되지 않음..재시도중..";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //빈방이 없어서 랜덤 룸 참가에 실패한 경우 자동 실행
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //접속상태 표시
        connectionInfoText.text = "빈방이 없음, 새로운 방 생성중";
        //최대 4인 참여가능한 룸 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    //룸에 참가가 완료된 경우 자동 실행
    public override void OnJoinedRoom()
    {
        //접속상태 표시
        connectionInfoText.text = "방 참가 성공";
        Debug.Log(PhotonNetwork.CloudRegion);
        //모든 룸 참가자들이 메인씬을 로드
        PhotonNetwork.LoadLevel("MultiScene");
    }
}
