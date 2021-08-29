using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetWorkUiHide : MonoBehaviourPun
{
    [SerializeField]
    PhotonView pv;

    public Canvas canvas;
    //public Text countdown;
    //int countTime = 60;
    //bool countcheck = false;

    // Start is called before the first frame update
    void Start()
    {
        canvas.worldCamera = FindObjectOfType<Camera>();
        pv = this.gameObject.GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            StartCoroutine("Count");

        }
    }
    IEnumerator Count()
    {
        for (; ; )
        {
            GameObject g = GameObject.Find("NetWorkPlayer(Clone)");
            if (g != null)
            {
                yield return new WaitForSeconds(2f);
                this.gameObject.SetActive(false);
                StopCoroutine("Count");
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (!pv.IsMine)
        {
            return;
        }
        //countdown.text = string.Format("{0}", countTime);
        //if(PhotonNetwork.PlayerList.Length >= 2 && countcheck == false)
        //{
        //    StartCoroutine("BattleCount");
        //}
    }

    //IEnumerator BattleCount()
    //{

    //  if(countTime !=0)
    //  {
    //     countcheck = true;
    //     countTime--;
    //     yield return new WaitForSeconds(1f);
    //        countcheck = false;
    //  }
    //  else if(countTime == 0)
    //    {
    //        StopCoroutine("BattleCount");
    //    }
        
  
    //}
}
