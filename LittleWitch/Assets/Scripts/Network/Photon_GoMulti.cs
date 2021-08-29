using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Photon_GoMulti : MonoBehaviourPunCallbacks
{
    public Text message;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        message.text = string.Format("참여인원 : {0} / 2", PhotonNetwork.PlayerList.Length);
        if(PhotonNetwork.PlayerList.Length == 2)
        {
            PhotonNetwork.LoadLevel("MultiScene");
        }
    }

   
}
