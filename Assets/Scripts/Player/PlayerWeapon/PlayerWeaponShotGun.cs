
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWeaponShotGun : MonoBehaviour
{
    public bool isReLoading;

    public int MaxBulletCount = 5;

    public int curBoulletCount = 5;

    public float maxReLodingTime = 3.8f;

    public float curReLodingTime;

    public float ShotGunMaxDistance = 100f;
    public float damage;
    public GameObject PreFebBullet;
    public GameObject bulletMarks;  
    public Transform bulletT;
    public float CooldownTime = 0.65f;
    int ShotBulletCount = 10;
    float spreadRadius;
    public float spreadAngle = 10f;    // 분포 각도
    public void HitScanShotGun()
    {
        Debug.Log("발사");
        Instantiate(PreFebBullet, bulletT);
        spreadRadius = PlayerState.PlayerIsZooming ? 200f : 150f;
        for (int i = 0; i < ShotBulletCount; ++i)
            {
            Vector2 randomCircle = Random.insideUnitCircle * spreadRadius; // 스크린 기준 원 안의 랜덤 점
            Vector2 screenPoint = new Vector2(Screen.width / 2f + randomCircle.x, Screen.height / 2f + randomCircle.y);

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);
            Debug.Log("산탄 값 : " + spreadRadius);
            if (Physics.Raycast(ray.origin, ray.direction, out hit, ShotGunMaxDistance, ~((1 << 7) | (1 << 9))))
                {
                Instantiate(bulletMarks, hit.point, Quaternion.LookRotation(hit.normal));
                Debug.Log("히트된 물체 : " + hit.collider.name);


                if (hit.collider.CompareTag("Enemy"))
                    {
                        hit.collider.GetComponent<EnemyHealth>().EnemyTakeDamage(damage);
                    }



                }
            }
        curBoulletCount -= 1;
        if (curBoulletCount <= 0)
        {
            curBoulletCount = MaxBulletCount;

        }
        Debug.Log("샷건 총알 발사 성공!");

    }

}
