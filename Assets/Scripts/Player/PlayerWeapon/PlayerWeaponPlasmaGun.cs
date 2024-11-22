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
    public Transform PlasmaT;
    public GameObject PreFebBullet;
    public float CooldownTime = 1.3f;
    public void CreatePlasma()
    {
        Instantiate(PreFebBullet, PlasmaT.position,Camera.main.transform.rotation ,null);

        curBoulletCount -= 1;
        if (curBoulletCount <= 0)
        {
            curBoulletCount = MaxBulletCount;

        }
        Debug.Log("플라즈마 총알 발사 성공!");

    }

}
