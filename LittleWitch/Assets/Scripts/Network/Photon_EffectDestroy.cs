using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Photon_EffectDestroy : MonoBehaviourPunCallbacks
{
    public PhotonView pv;
    [PunRPC]
    void DestroyRPC1(float time)
    {
        Destroy(gameObject, time);
    }

    public void effectdelete(float time)
    {
        pv.RPC("DestroyRPC1", RpcTarget.AllBuffered, time);
    }
}
