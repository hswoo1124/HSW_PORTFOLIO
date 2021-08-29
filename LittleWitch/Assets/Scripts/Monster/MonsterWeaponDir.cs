using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeaponDir : MonoBehaviour
{
    GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    void FixedUpdate()
    {
        this.gameObject.transform.LookAt(player.transform);
    }
}
