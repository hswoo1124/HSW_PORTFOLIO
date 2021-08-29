using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteo : MonoBehaviour
{
    GameObject playerTrans;
    GameObject meeoPos_instance;
    public GameObject impactParticle;
    public GameObject meteoPos;
    bool hasCollided = false;
    Transform target;

    public Vector3 impactNormal;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = GameObject.FindWithTag("Player");
        target = playerTrans.transform;    // 게임오브젝트를 등록하여 그 오브젝트에 대한 정보 저장
        transform.position = new Vector3(target.position.x, target.position.y + 25.0f, target.position.z);
        meeoPos_instance = Instantiate(meteoPos, transform.position, Quaternion.identity) as GameObject;

        meeoPos_instance.transform.position = new Vector3(target.position.x, 2.0f, target.position.z);
        meeoPos_instance.transform.rotation = Quaternion.Euler(new Vector3(90f, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
            if (!hasCollided && other.GetComponent<Collider>().tag != "Ground_Trigger")
            {
            hasCollided = true;
            GameObject impactParticle_instance = Instantiate(impactParticle, transform.position, Quaternion.identity) as GameObject;

            Destroy(meeoPos_instance);
            Destroy(gameObject);
            }
    }
}
