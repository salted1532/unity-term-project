using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerWeaponMgr : MonoBehaviour
{
    public Animator anim;
    public bool IsShoting;
    public PlayerWeaponDefault DefaultGun;
    public PlayerWeaponShotGun ShotGun;
    public PlayerWeaponRifle RifleGun;
    public PlayerWeaponPlasmaGun PlasmaGun;

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
        IsShoting = false;

        if (Input.GetMouseButtonDown(0) && !PlayerState.PlayerIsDashing && PlayerInventory.GetCurWeaponNum() != 2)
        {
            Fire();
            IsShoting = true;

        }
        //Weapon is Rifle
        if(Input.GetMouseButton(0) && !PlayerState.PlayerIsDashing && PlayerInventory.GetCurWeaponNum() == 2)
        {
            Fire();
            IsShoting = true;

        }
        if (curCooldownTime < PlayerState.PlayerAttackMaxCooldownTime)
        {
            curCooldownTime += Time.deltaTime;
        }
        if (isReLoading)
        {
            ReRoadGun();
            IsShoting = false;

        }
        anim.SetBool("IsShot", IsShoting);

    }

    void Fire()
    {

        if (curCooldownTime > PlayerState.PlayerAttackMaxCooldownTime && !isReLoading)
        {


            switch (PlayerInventory.GetCurWeaponNum())
            {

                case 0:
                    SoundManager.Instance.PlaySound2D("EFFECT_Click_Mechanical");

                    Debug.Log("ΩÓ±‚");
                    DefaultGun.HitScan();
                    curCooldownTime = 0;

                    break;
                case 1:
                    SoundManager.Instance.PlaySound2D("EFFECT_Click_Mechanical");


                    Debug.Log("ΩÓ±‚");
                    ShotGun.HitScanShotGun();
                    curCooldownTime = 0;

                    break;
                    
                case 2:
                    SoundManager.Instance.PlaySound2D("EFFECT_Click_Mechanical");


                    Debug.Log("ΩÓ±‚");
                    RifleGun.HitScanShotGun();
                    curCooldownTime = 0;
                    break;

                case 3:
                    SoundManager.Instance.PlaySound2D("EFFECT_Click_Mechanical");


                    Debug.Log("ΩÓ±‚");
                    PlasmaGun.CreatePlasma();
                    curCooldownTime = 0;
                    break;

                default:

                    break;
            }
            if (curBoulletCount <= 0)
            {
                isReLoading = true;
                Debug.Log("¿Á¿Â¿¸ Ω√¿€");

            }
            curBoulletCount--;

        }


    }
    void ReRoadGun()
    {
        if (curReLodingTime > maxReLodingTime)
        {
            curBoulletCount = MaxBulletCount;
            isReLoading = false;
            curReLodingTime = 0;
            Debug.Log("¿Á¿Â¿¸ ≥°");
        }
        else
        {
            curReLodingTime += Time.deltaTime;
            Debug.Log("¿Á¿Â¿¸ ¡ﬂ");

        }
    }
    public void SetCurWeaponData()
    {
        switch (PlayerInventory.GetCurWeaponNum())
        {
            case 0:
                MaxBulletCount = DefaultGun.MaxBulletCount;
                maxReLodingTime = DefaultGun.maxReLodingTime;
                curBoulletCount = DefaultGun.curBoulletCount;
                curReLodingTime = DefaultGun.curReLodingTime;
                PlayerState.SetMaxCooldownTIme(DefaultGun.CooldownTime);
                curCooldownTime = DefaultGun.CooldownTime - 0.115f;

                isReLoading = false;
                break;
            case 1:
                MaxBulletCount = ShotGun.MaxBulletCount;
                maxReLodingTime = ShotGun.maxReLodingTime;
                curBoulletCount = ShotGun.curBoulletCount;
                curReLodingTime = ShotGun.curReLodingTime;
                PlayerState.SetMaxCooldownTIme(ShotGun.CooldownTime);
                curCooldownTime = ShotGun.CooldownTime - 0.15f;
                isReLoading = false;

                break;
            case 2:
                MaxBulletCount = RifleGun.MaxBulletCount;
                maxReLodingTime = RifleGun.maxReLodingTime;
                curBoulletCount = RifleGun.curBoulletCount;
                curReLodingTime = RifleGun.curReLodingTime;
                PlayerState.SetMaxCooldownTIme(RifleGun.CooldownTime);
                curCooldownTime = RifleGun.CooldownTime - 0.3f;
                isReLoading = false;
                break;

            case 3:
                MaxBulletCount = PlasmaGun.MaxBulletCount;
                maxReLodingTime = PlasmaGun.maxReLodingTime;
                curBoulletCount = PlasmaGun.curBoulletCount;
                curReLodingTime = PlasmaGun.curReLodingTime;
                PlayerState.SetMaxCooldownTIme(PlasmaGun.CooldownTime);
                curCooldownTime = PlasmaGun.CooldownTime - 0.5f;
                isReLoading = false;
                break;
            default:
                break;
        }
    }
}
