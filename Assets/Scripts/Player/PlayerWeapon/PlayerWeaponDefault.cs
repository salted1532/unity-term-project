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

    public float defaultWeaponMaxDistance = 500f;
    public float damage = 5f;
    public Transform PreFebBulletT;
    public Transform bulletMarksT;

    public GameObject PreFebBullet;
    public GameObject bulletMarks;
    public float CooldownTime = 0.35f;

    private void Awake()
    {
        PlayerState.PlayerAttackMaxCooldownTime = CooldownTime;
    }
    public void HitScan()
    {
        Debug.Log("발사");
        Instantiate(PreFebBullet, PreFebBulletT);

        RaycastHit hit;
        Debug.Log(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, defaultWeaponMaxDistance, ~((1 << 7) | (1 << 9))));
        if (Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward, out hit, defaultWeaponMaxDistance, ~((1 << 7) | (1 << 9))))
        {
                Debug.Log("히트된 물체 : " + hit.collider.name);
            Instantiate(bulletMarks, hit.point, Quaternion.LookRotation(hit.normal));

            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<EnemyHealth>().EnemyTakeDamage(damage);
            }

               




        }
        curBoulletCount -= 1;
        if (curBoulletCount <= 0)
        {
            curBoulletCount = MaxBulletCount;

        }

    }

}
