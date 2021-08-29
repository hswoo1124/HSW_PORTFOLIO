using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetWork_SplashDamage : MonoBehaviourPunCallbacks
{
    public float splashDamage;
    public SkillElement element;
    bool hasCollided = false;
    public float impactLifeT;
    PhotonView pv;
    private void Awake()
    {
        pv = this.gameObject.GetPhotonView();
    }
    public void SetElement(int _element)
    {
        element = (SkillElement)_element;
    }


    [PunRPC]
    void DestroyRPC() => Destroy(gameObject, 0.8f);
    void OnCollisionEnter(Collision hit)
    {
        if (!hasCollided && this.gameObject.tag == "Impact")
        {
            hasCollided = true;
            pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }

    }
    void OnTriggerEnter(Collider hit)
    {
        if (!hasCollided && this.gameObject.tag == "Impact_Trigger")
        {
            hasCollided = true;

            pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
    }

    public enum SkillElement
    {
        Arcane,
        Fire,
        Ice,
        Poison,
        Lightning,
        None
    }
}
