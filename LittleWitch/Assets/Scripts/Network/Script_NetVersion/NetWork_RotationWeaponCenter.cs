using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetWork_RotationWeaponCenter : MonoBehaviour
{
    [SerializeField]
    Transform weaponCenterPos;
    //[HideInInspector]
    public Transform uiCenterPos;



    private void Awake()
    {
        //s_StickRotation = GameObject.Find("S_StickRotation");
        //uiCenterPos = s_StickRotation.transform;
    }

    void Update()
    {
        try
        {
            weaponCenterPos.rotation = uiCenterPos.rotation;
        }
        catch
        {

        }
    }
}
