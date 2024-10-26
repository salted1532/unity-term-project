using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWeaponMgr : MonoBehaviour
{

    public PlayerWeaponDefault DefaultGun;
    public PlayerWeaponShotGun ShotGun;

    public bool isReLoading;

    public int MaxBulletCount;

    public int curBoulletCount;

    public float maxReLodingTime;

    public float curReLodingTime;

    float curCooldownTime;

    private void Awake()
    {
        MaxBulletCount = DefaultGun.MaxBulletCount;
        maxReLodingTime = DefaultGun.maxReLodingTime; 
        curBoulletCount = DefaultGun.curBoulletCount;
        curReLodingTime = DefaultGun.curReLodingTime;
        isReLoading = false;
    }
    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && !PlayerState.PlayerIsDashing)
        {
            Fire();
        }
        if(curCooldownTime < PlayerState.PlayerAttackMaxCooldownTime)
        {
            curCooldownTime += Time.deltaTime;
        }
        if (isReLoading)
        {
            ReRoadGun();
        }
    }
    
    void Fire()
    {

        if (curCooldownTime > PlayerState.PlayerAttackMaxCooldownTime && !isReLoading)
        {

            curBoulletCount--;
            if (curBoulletCount <= 0)
            {
                isReLoading = true;
                Debug.Log("재장전 시작");

            }
            switch (PlayerInventory.GetCurWeaponNum())
            {
                case 0:
                    SoundManager.Instance.PlaySound2D("EFFECT_Click_Mechanical");

                    Debug.Log("쏘기");
                    DefaultGun.HitScan();
                    curCooldownTime = 0;

                    break;
                case 1:
                    SoundManager.Instance.PlaySound2D("EFFECT_Click_Mechanical");


                    Debug.Log("쏘기");
                    ShotGun.HitScanShotGun();
                    curCooldownTime = 0;

                    break;
                default:

                    break;
            }


        }


    }
    void ReRoadGun()
    {
        if (curReLodingTime > maxReLodingTime)
        {
            curBoulletCount = MaxBulletCount;
            isReLoading = false;
            curReLodingTime = 0;
            Debug.Log("재장전 끝");
        }
        else
        {
            curReLodingTime += Time.deltaTime;
            Debug.Log("재장전 중");

        }
    }
}
