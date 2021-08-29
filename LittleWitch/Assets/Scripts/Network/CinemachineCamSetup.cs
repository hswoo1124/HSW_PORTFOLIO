using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class CinemachineCamSetup : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        //만약 자신이 로컬 플레이어 라면
        if (photonView.IsMine)
        {
            //씬에 있는 가상 카메라를 찾기
            CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();
            //가상 카메라의 추적대상을 자신(의 트랜스폼)으로 변경
            followCam.Follow = transform;
            followCam.LookAt = transform;
        }
    }
}
