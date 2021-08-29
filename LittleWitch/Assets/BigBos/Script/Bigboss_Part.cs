using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bigboss_Part : MonoBehaviour
{
    SkillProjectile sp;

    [SerializeField]
    Bigbosscontrol bbc;

    bool isSplash;
    bool hasTriggered = false;
    bool isHit = false;

    float skillProjectileDamage;
    float skillImpactDamage;



    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered)
        {
            hasTriggered = true;
            if (other.gameObject.tag == "Projectile_Trigger")
            {
                sp = other.gameObject.GetComponent<SkillProjectile>();
                if (sp.isSplash == false)
                {
                    isSplash = false;
                    skillProjectileDamage = sp.damage;
                    Debug.Log(skillProjectileDamage.ToString());
                    if (isHit == false)
                    {

                        StartCoroutine(DamagedFromProjectile());
                    }
                }

            }
            else if (other.gameObject.tag == "Impact_Trigger")
            {
                SplashDamage sd = other.gameObject.GetComponent<SplashDamage>();
                skillImpactDamage = sd.splashDamage;
                if (isHit == false)
                {
                    StartCoroutine(DamagedFromImpact());
                }
            }
            hasTriggered = false;
        }
    }

    #region 피해관련 메서드

    IEnumerator DamagedFromProjectile()
    {
        isHit = true;
        bbc.bossHp -= skillProjectileDamage * 1f;

        yield return new WaitForSeconds(0.1f);

        isHit = false;
        StopCoroutine(DamagedFromProjectile());
    }
    IEnumerator DamagedFromImpact()
    {
        isHit = true;
        bbc.bossHp -= skillImpactDamage * 1f;

        yield return new WaitForSeconds(0.1f);

        StopCoroutine(DamagedFromImpact());
        isHit = false;
    }
    #endregion
}
