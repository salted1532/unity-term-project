using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWeaponRifle : MonoBehaviour
{
    public bool isReLoading;

    public int MaxBulletCount = 40;

    public int curBoulletCount = 40;

    public float maxReLodingTime = 3.8f;

    public float curReLodingTime;

    public float ShotGunMaxDistance = 50f;
    public float damage = 3.5f;
    public UnityEvent<float> DamageEvent = new UnityEvent<float>();
    public GameObject PreFebBullet;
    public float CooldownTime = 0.2f;
    public void HitScanShotGun()
    {



        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 500f, 7))
        {
            Debug.Log("히트된 물체" + hit.collider.name);

            if (hit.collider != null)
            {
                //damage function
                //DamageEvent?.Invoke(damage);
                hit.collider.GetComponent<EnemyHealth>().EnemyTakeDamage(damage);
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
