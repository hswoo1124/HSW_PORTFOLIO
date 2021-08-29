using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StageManager sm = GameObject.Find("StageManager").GetComponent<StageManager>();
            sm.CountToNextStage();
        }
    }
}
