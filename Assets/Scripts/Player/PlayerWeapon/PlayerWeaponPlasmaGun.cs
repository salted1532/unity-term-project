using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWeaponPlasmaGun : MonoBehaviour
{
    public bool isReLoading;

    public int MaxBulletCount = 3;

    public int curBoulletCount = 3;

    public float maxReLodingTime = 3.8f;

    public float curReLodingTime;

    public float ShotGunMaxDistance = 50f;
    public float damage = 30f;
    public UnityEvent<float> DamageEvent = new UnityEvent<float>();
    public GameObject PreFebBullet;
    public float CooldownTime = 1.3f;
    public void CreatePlasma()
    {
        Instantiate(PreFebBullet);

        curBoulletCount -= 1;
        if (curBoulletCount <= 0)
        {
            curBoulletCount = MaxBulletCount;

        }
        Debug.Log("¼¦°Ç ÃÑ¾Ë ¹ß»ç ¼º°ø!");

    }

}
