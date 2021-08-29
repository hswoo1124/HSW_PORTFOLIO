using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetWork_SkillProjecttile : MonoBehaviourPunCallbacks
{
    #region 스킬변수
    public float power;
    public float distance;
    public int rank;
    public SkillElement element;
    public float speed;
    public float coolTime;
    public SkillType type;
    public float damage;
    public float missileLifeT;
    #endregion

    #region 차지스킬 변수
    private float charge;
    public float chargeMax;
    public float chargeMulti;
    public float increaseSpeedMulti;
    public float increasePowerMulti;
    #endregion

    public bool penetrateAble;
    public bool isSplash;

    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;

     GameObject impactParticle1;
     GameObject projectileParticle1;
     GameObject muzzleParticle1;
    public GameObject player;

    public GameObject weaponCenterPos;
    [HideInInspector]
    public NetWork_ShootingManager shootingManager;
    [HideInInspector]
    public NetWork_CharacterStatus characterStatus;

    public GameObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.

    private bool hasCollided = false;
    public PhotonView pv;

    public void SetDamage(float attackPoint)
    {
        switch (type)
        {
            case SkillType.None:
                damage = power * attackPoint;
                break;
            case SkillType.Charge:
                damage = power * attackPoint * (1 + charge * increasePowerMulti);
                break;
            case SkillType.Spread:
                damage = power * attackPoint;
                break;
        }
    }

    //투사체 생성시 ShootingManager로부터 현재 차지 게이지를 받아옴
    public void SetChargeNum(float currentCharge)
    {
        charge = currentCharge;
       // characterStatus = player.GetComponent<NetWork_CharacterStatus>();
        SetDamage(characterStatus.attack_Point);
    }

    public void SetSkillData(float _power, int _rank, int _element, float _coolTime, int _type)    //장착된 스킬잼에 따라서 스킬능력치 변동됨
    {
        power = _power;
        rank = _rank;
        element = (SkillElement)_element;
        coolTime = _coolTime;
        type = (SkillType)_type;
    }

    public void AddChargeData(float _chargeMulti, float _increaseSpeedMulti, float _increasePowerMulti) //차지스킬인 경우 추가로 받아올 데이터
    {
        chargeMulti = _chargeMulti;
        increaseSpeedMulti = _increaseSpeedMulti;
        increasePowerMulti = _increasePowerMulti;
    }
    private void Start()
    {
        // pv = this.gameObject.GetComponent<PhotonView>();
        //GameObject player = GameObject.Find("Player");
        //characterStatus = player.GetComponent<CharacterStatus>();
        //SetDamage(characterStatus.attack_Point);

        //GameObject fireManager = GameObject.Find("FireManager");
        //shootingManager = fireManager.GetComponent<ShootingManager>();

        //weaponCenterPos = GameObject.Find("weaponCenterPos");
        if (pv.IsMine)
        {
            projectileParticle1 = PhotonNetwork.Instantiate(projectileParticle.name, transform.position, transform.rotation) as GameObject;
            //Debug.Log("프로젝트타일발사아");
            projectileParticle1.transform.parent = transform;
            projectileParticle1.GetComponent<Photon_EffectDestroy>().effectdelete(0.8f);

            if (muzzleParticle)
            {
                muzzleParticle1 = PhotonNetwork.Instantiate(muzzleParticle.name, transform.position, transform.rotation) as GameObject;
                //Debug.Log("머즐타일발사ㅏ아ㅏ");
                // muzzleParticle1.GetComponent<PhotonView>().RPC("DestroyRPC", RpcTarget.AllBuffered, muzzleParticle1, 2.1f);
                muzzleParticle1.GetComponent<Photon_EffectDestroy>().effectdelete(1.5f);
            }
        }
    }



    //void OnCollisionEnter(Collision hit)
    //{
    //    //try
    //    //{
    //    Debug.Log(hit.gameObject.tag);
    //    Rpc_OnCollisionEnter(hit);
    //    //pv.RPC("Rpc_OnCollisionEnter", RpcTarget.AllBuffered, hit);
    //    //}
    //    //catch
    //    //{
    //    //    Debug.Log("콜리전 엔터");
    //    //}
    //}

    void Rpc_OnCollisionEnter(Collision hit)
    {
        try
        {
            if (hit.gameObject.tag == "Impact_Trigger" || hit.gameObject.tag == "Impact")
            {
                return;
            }
            if (!hasCollided && penetrateAble == true)
            {
                if (hit.collider.tag != "Monsters" && hit.collider.tag != "Player")
                {
                    Debug.Log(hit.collider.tag);
                    impactParticle1 = PhotonNetwork.Instantiate(impactParticle.name, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
                    if (!pv.IsMine && hit.gameObject.GetComponent<PhotonView>().IsMine)
                    {
                        hit.gameObject.GetComponent<NetWork_CharacterStatus>().Hit_collision1(gameObject);
                    }

                    projectileParticle1.GetComponent<Photon_EffectDestroy>().effectdelete(1f);
                    impactParticle1.GetComponent<Photon_EffectDestroy>().effectdelete(1f);
                    pv.RPC("DMN", RpcTarget.AllBuffered);
                    //PhotonNetwork.Destroy(pv);
                    //pv.RPC("DestroyRPC", RpcTarget.AllBuffered, gameObject, 2.1f);
                    //Destroy(projectileParticle.gameObject, 2f);
                    //Destroy(impactParticle.gameObject, 1f);
                    //Destroy(gameObject);

                }
            }
            if (!hasCollided && isSplash == false && penetrateAble == false)
            {
                Debug.Log(hit.collider.tag);
                hasCollided = true;
                impactParticle1 = PhotonNetwork.Instantiate(impactParticle.name, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
                //if (!pv.IsMine && hit.gameObject.GetComponent<PhotonView>().IsMine)
                //{
                    //Debug.Log(hit.collider.tag);
                    hit.gameObject.GetComponent<NetWork_CharacterStatus>().Hit_collision1(gameObject);
                //}

                projectileParticle1.GetComponent<Photon_EffectDestroy>().effectdelete(1f);
                impactParticle1.GetComponent<Photon_EffectDestroy>().effectdelete(1f);
                pv.RPC("DMN", RpcTarget.AllBuffered);
                //PhotonNetwork.Destroy(pv);
                //pv.RPC("DestroyRPC", RpcTarget.AllBuffered, gameObject, 2.1f);


                ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
                //Component at [0] is that of the parent i.e. this object (if there is any)
                for (int i = 1; i < trails.Length; i++)
                {

                    ParticleSystem trail = trails[i];

                    if (trail.gameObject.name.Contains("Trail"))
                    {
                        trail.transform.SetParent(null);
                        Destroy(trail.gameObject, 2f);
                    }
                }
            }
        }
        catch
        {
            Debug.Log("zzzzz1");
        }

        if (!hasCollided && isSplash == true)
        {
            hasCollided = true;
            impactParticle1 = PhotonNetwork.Instantiate(impactParticle.name, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
            if (!pv.IsMine && hit.gameObject.GetComponent<PhotonView>().IsMine)
            {
                hit.gameObject.GetComponent<NetWork_CharacterStatus>().Hit_collision2(gameObject);
            }
            SplashDamage sd = impactParticle.GetComponent<SplashDamage>();
            sd.splashDamage = damage;
            sd.impactLifeT = coolTime;
            sd.SetElement((int)element);

            try
            {
                foreach (GameObject trail in trailParticles)
                {
                    GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
                    curTrail.transform.parent = null;
                    //pv.RPC("DestroyRPC", RpcTarget.AllBuffered, curTrail, 3f);
                    Destroy(curTrail.gameObject, 3f);
                }
            }
            catch
            {
                Debug.Log("zzzzz2");
            }
            projectileParticle1.GetComponent<Photon_EffectDestroy>().effectdelete(1f);
            impactParticle1.GetComponent<Photon_EffectDestroy>().effectdelete(1f);
            pv.RPC("DMN", RpcTarget.AllBuffered);
            // PhotonNetwork.Destroy(pv);
            //pv.RPC("DestroyRPC", RpcTarget.AllBuffered, gameObject, 2.1f);
            try
            {
                ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
                //Component at [0] is that of the parent i.e. this object (if there is any)
                for (int i = 1; i < trails.Length; i++)
                {

                    ParticleSystem trail = trails[i];

                    if (trail.gameObject.name.Contains("Trail"))
                    {
                        trail.transform.SetParent(null);
                        //pv.RPC("DestroyRPC", RpcTarget.AllBuffered, trail, 2f);
                        Destroy(trail, 2f);
                    }
                }
            }
            catch
            {
                Debug.Log("zzzzz3");
            }
        }
    }
    void OnTriggerEnter(Collider hit)
    {
        //try
        //{
            //Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            Rpc_OnTriggerEnter(hit);
           // pv.RPC("Rpc_OnTriggerEnter", RpcTarget.AllBuffered, hit);
        //}
        //catch
        //{
        //    Debug.Log("온트리거 엔터");
        //}
    }
    void Rpc_OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Impact_Trigger" || hit.gameObject.tag == "Impact")
        {
            return;
        }
        if (!hasCollided && penetrateAble == true)
        {
            Debug.Log(hit.gameObject.name);
            if (hit.GetComponent<Collider>().tag != "Monsters" || hit.GetComponent<Collider>().tag != "Player"
                || hit.GetComponent<Collider>().tag != "Projectile_Trigger")
            {
                impactParticle1 = PhotonNetwork.Instantiate(impactParticle.name, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
                if (!pv.IsMine && hit.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    hit.gameObject.GetComponent<NetWork_CharacterStatus>().Hit_collider1(gameObject);
                }
                projectileParticle1.GetComponent<Photon_EffectDestroy>().effectdelete(1f);
                impactParticle1.GetComponent<Photon_EffectDestroy>().effectdelete(1f);
                pv.RPC("DMN", RpcTarget.AllBuffered);
                // PhotonNetwork.Destroy(pv);
                //pv.RPC("DestroyRPC", RpcTarget.AllBuffered, gameObject, 2.1f);
            }
        }
        if (!hasCollided && isSplash == false && penetrateAble == false)
        {
            try
            {
                Debug.Log(hit.name);
            hasCollided = true;
            impactParticle1 = PhotonNetwork.Instantiate(impactParticle.name, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
                if (pv.IsMine && !hit.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    //PhotonView pvp = hit.gameObject.GetComponent<PhotonView>();
                    //pvp.RPC("Hit_collider1", RpcTarget.AllBuffered);
                    hit.gameObject.GetComponent<NetWork_CharacterStatus>().Hit_collider1(gameObject);
                    Debug.Log("맞았어욧");
                }

                foreach (GameObject trail in trailParticles)
                {
                    GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
                    curTrail.transform.parent = null;
                    Destroy(curTrail.gameObject, 3f);
                }

                projectileParticle1.GetComponent<Photon_EffectDestroy>().effectdelete(1f);
                impactParticle1.GetComponent<Photon_EffectDestroy>().effectdelete(1f);
                pv.RPC("DMN", RpcTarget.AllBuffered);
            }
            catch
            {
                Debug.Log("zz");
                //projectileParticle1.GetComponent<Photon_EffectDestroy>().effectdelete(1f);
                //impactParticle1.GetComponent<Photon_EffectDestroy>().effectdelete(1f);
                //pv.RPC("DMN", RpcTarget.AllBuffered);
            }
            //projectileParticle1.GetComponent<Photon_EffectDestroy>().effectdelete(1f);
            //impactParticle1.GetComponent<Photon_EffectDestroy>().effectdelete(1f);
            //pv.RPC("DMN", RpcTarget.AllBuffered);
            //PhotonNetwork.Destroy(pv);
            //pv.RPC("DestroyRPC", RpcTarget.AllBuffered, gameObject, 2.1f);
            try
            {
                ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
                //Component at [0] is that of the parent i.e. this object (if there is any)
                for (int i = 1; i < trails.Length; i++)
                {

                    ParticleSystem trail = trails[i];

                    if (trail.gameObject.name.Contains("Trail"))
                    {
                        trail.transform.SetParent(null);
                        //pv.RPC("DestroyRPC", RpcTarget.AllBuffered, trail, 2f);
                        Destroy(trail, 2f);
                    }
                }
            }
            catch
            {

            }
        }
        if (!hasCollided && isSplash == true)
        {
            Debug.Log(hit.gameObject.name);
            impactParticle1 = PhotonNetwork.Instantiate(impactParticle.name, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
            hasCollided = true;

            if (!pv.IsMine && hit.gameObject.GetComponent<PhotonView>().IsMine)
            {
                hit.gameObject.GetComponent<NetWork_CharacterStatus>().Hit_collider2(gameObject);
            }
            SplashDamage sd = impactParticle.GetComponent<SplashDamage>();
            sd.splashDamage = damage;
            sd.impactLifeT = coolTime;
            sd.SetElement((int)element);

            try
            {
                foreach (GameObject trail in trailParticles)
                {
                    GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
                    curTrail.transform.parent = null;
                    //pv.RPC("DestroyRPC", RpcTarget.AllBuffered, curTrail, 3f);
                    Destroy(curTrail, 3f);
                }
            }
            catch
            {

            }
            projectileParticle1.GetComponent<Photon_EffectDestroy>().effectdelete(1f);
            impactParticle1.GetComponent<Photon_EffectDestroy>().effectdelete(1f);
            pv.RPC("DMN", RpcTarget.AllBuffered);
            //PhotonNetwork.Destroy(pv);
            //pv.RPC("DestroyRPC", RpcTarget.AllBuffered, gameObject, 2.1f);
            try
            {
                ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
                //Component at [0] is that of the parent i.e. this object (if there is any)
                for (int i = 1; i < trails.Length; i++)
                {

                    ParticleSystem trail = trails[i];

                    if (trail.gameObject.name.Contains("Trail"))
                    {
                        trail.transform.SetParent(null);
                        // pv.RPC("DestroyRPC", RpcTarget.AllBuffered, trail, 2f);
                        Destroy(trail, 2f);
                    }
                }
            }
            catch
            {

            }
        }
    }
    private void FixedUpdate()
    {
        //try
        //{

            //DestroyMissile();
            pv.RPC("DestroyMissile", RpcTarget.AllBuffered);
   
        //}
        //catch
        //{
        //    Debug.Log("업뎃오류");
        //}
    }

    [PunRPC]
    void DestroyMissile()
    {
        //try
        //{
            missileLifeT -= Time.deltaTime * 1f;

            if (missileLifeT < 0)
            {
                //PhotonNetwork.Destroy(pv);
               // pv.RPC("DestroyRPC", RpcTarget.AllBuffered, gameObject, 0);
                Destroy(this.gameObject);
            }
        //}
        //catch
        //{
        //    Debug.Log("파티클 파괴");
        //}
    }
    [PunRPC]
    void DMN() => Destroy(gameObject);

   

    public enum SkillType
    {
        None,
        Charge,
        Spread
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
