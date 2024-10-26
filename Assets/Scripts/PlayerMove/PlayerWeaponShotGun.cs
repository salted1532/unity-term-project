
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerWeaponShotGun : MonoBehaviour
{
    public bool isReLoading;

    public int MaxBulletCount = 5;

    public int curBoulletCount = 5;

    public float maxReLodingTime = 3.8f;

    public float curReLodingTime;

    public float ShotGunMaxDistance = 50f;
    public float damage = 8f;
    public UnityEvent<float> DamageEvent = new UnityEvent<float>();
    public GameObject PreFebBullet;
    public float ShotGunCooldownTime = 0.65f;
    int ShotBulletCount = 10;
    float spreadRadius = 0.5f;
    public float spreadAngle = 10f;    // 분포 각도
    public void HitScanShotGun()
    {

            for (int i = 0; i < ShotBulletCount; ++i)
            {
                // 랜덤한 각도로 회전하여 레이 방향 설정
                Vector2 randomCircle = Random.insideUnitCircle * spreadRadius;
                Vector3 spreadDirection = Camera.main.transform.forward +
                                          transform.right * randomCircle.x +
                                          transform.up * randomCircle.y;

                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, spreadDirection, out hit, ShotGunMaxDistance, 7))
                {

                    //damage function
                    DamageEvent?.Invoke(damage);


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
