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
    public Transform bulletT;
    public GameObject bulletMarks;
    public GameObject PreFebBullet;
    public float CooldownTime = 0.2f;
    public void HitScanRifle()
    {

        Debug.Log("발사");
        Instantiate(PreFebBullet, bulletT);

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 500f, ~((1 << 7) | (1 << 9))))
        {
            Debug.Log("히트된 물체" + hit.collider.name);

            if (hit.collider != null)
            {
                Instantiate(bulletMarks, hit.point, Quaternion.LookRotation(hit.normal));
                if (hit.collider.CompareTag("Enemy"))
                {
                    hit.collider.GetComponent<EnemyHealth>().EnemyTakeDamage(damage);
                }



                curBoulletCount -= 1;
                if (curBoulletCount <= 0)
                {
                    curBoulletCount = MaxBulletCount;

                }

            }
        }



    }

}
