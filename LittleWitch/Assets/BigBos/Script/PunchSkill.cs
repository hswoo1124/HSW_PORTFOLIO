using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchSkill : MonoBehaviour
{
    GameObject playerTrans;
    Transform target;
    // Start is called before the first frame update
    void Start()
    {
        playerTrans = GameObject.FindWithTag("Player");
        target = playerTrans.transform;    // 게임오브젝트를 등록하여 그 오브젝트에 대한 정보 저장
        transform.position = new Vector3(target.position.x, target.position.y - 4.0f, target.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject,0.8f);
    }
}
