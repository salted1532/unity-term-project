using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerWeaponDefault : MonoBehaviour
{
    public bool isReLoading;

    public int MaxBulletCount = 5;

    public int curBoulletCount = 5;

    public float maxReLodingTime = 2.4f;

    public float curReLodingTime;

    public float defaultWeaponMaxDistance = 100f;
    public float damage = 10f;
    public UnityEvent<float> DamageEvent = new UnityEvent<float>();
    public GameObject PreFebBullet;
    public float CooldownTime = 0.35f;

    private void Awake()
    {
        PlayerState.PlayerAttackMaxCooldownTime = CooldownTime;
    }
    public void HitScan()
    {

        RaycastHit hit; 
        if (Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward, out hit, defaultWeaponMaxDistance,7))
        {
                Debug.Log("히트된 물체 : " + hit.collider.name);

                if (hit.collider != null)
                {
                //damage function
                //DamageEvent?.Invoke(damage);
                if (hit.collider.CompareTag("Enemy"))
                {
                    hit.collider.GetComponent<EnemyHealth>().EnemyTakeDamage(damage);
                }
                Instantiate(PreFebBullet);


                curBoulletCount -= 1;
                if (curBoulletCount <= 0)
                {
                    curBoulletCount = MaxBulletCount;

                }

            }
        }


    }

}
