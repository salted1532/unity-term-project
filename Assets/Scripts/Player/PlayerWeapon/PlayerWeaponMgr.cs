using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PlayerWeaponMgr : MonoBehaviour
{
    public Animator anim;
    public bool IsShoting;
    public PlayerWeaponDefault DefaultGun;
    public PlayerWeaponShotGun ShotGun;
    public PlayerWeaponRifle RifleGun;
    public PlayerWeaponPlasmaGun PlasmaGun;

    public GameObject[] WeaponUIArr1;
    public GameObject[] WeaponUIArr2;
    public GameObject[] WeaponUIArr3;
    public GameObject[] WeaponUIArr4;

    public GameObject DefaultGunObj;
    public GameObject ShotGunObj;
    public GameObject RifleGunObj;
    public GameObject PlasmaGunObj;

    public GameObject[][] WeaponObjArr;


    public bool isReLoading;

    public int MaxBulletCount;

    public int curBoulletCount;

    public float maxReLodingTime;

    public float curReLodingTime;

    float curCooldownTime;

    public GameObject Player;

    private void Awake()
    {
        DefaultGunObj.SetActive(true);
        ShotGunObj.SetActive(false);
        RifleGunObj.SetActive(false);
        PlasmaGunObj.SetActive(false);

        MaxBulletCount = DefaultGun.MaxBulletCount;
        maxReLodingTime = DefaultGun.maxReLodingTime; 
        curBoulletCount = DefaultGun.curBoulletCount;
        curReLodingTime = DefaultGun.curReLodingTime;
        isReLoading = false;
    }
    private void Start()
    {
        WeaponObjArr = new GameObject[4][];
        WeaponObjArr[0] = WeaponUIArr4;
        WeaponObjArr[1] = WeaponUIArr1;
        WeaponObjArr[2] = WeaponUIArr2;
        WeaponObjArr[3] = WeaponUIArr3;

    }
    private void Update()
    {
        if(Player.GetComponent<PlayerController>().GetController() == false)
        {
            return;
        }

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

                    Debug.Log("�⺻�� ���");
                    DefaultGun.HitScan();
                    curCooldownTime = 0;

                    break;
                case 1:
                    SoundManager.Instance.PlaySound2D("EFFECT_Click_Mechanical");


                    Debug.Log("���� ���");
                    ShotGun.HitScanShotGun();
                    curCooldownTime = 0;

                    break;
                    
                case 2:
                    SoundManager.Instance.PlaySound2D("EFFECT_Click_Mechanical");


                    Debug.Log("������ ���");
                    RifleGun.HitScanRifle();
                    curCooldownTime = 0;
                    break;

                case 3:
                    SoundManager.Instance.PlaySound2D("EFFECT_Click_Mechanical");


                    Debug.Log("�ö�� ���");
                    PlasmaGun.CreatePlasma();
                    curCooldownTime = 0;
                    break;

                default:

                    break;
            }
            if (curBoulletCount <= 0)
            {
                isReLoading = true;
                Debug.Log("������ ����");

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
            Debug.Log("������ ��");
        }
        else
        {
            curReLodingTime += Time.deltaTime;
            Debug.Log("������ ��");

        }
    }
    public void SetCurWeaponData()
    {
        int[] arr = PlayerInventory.GetWeaponArray();
        for (int i = 0; WeaponObjArr.Length > i; i++)
        {
            for (int j = 0; WeaponObjArr[0].Length > j; j++)
            {
                if(WeaponObjArr[i][j] != null)
                WeaponObjArr[i][j].SetActive(false);
            }

        }
        SetCurWeaponObj(PlayerInventory.GetCurWeaponNum());
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
   void SetCurWeaponObj(int WeaponNum)
    {
        switch (WeaponNum)
        {
            case 0:
                DefaultGunObj.SetActive(true);
                ShotGunObj.SetActive(false);
                RifleGunObj.SetActive(false);
                PlasmaGunObj.SetActive(false);
                break;
            case 1:
                DefaultGunObj.SetActive(false);
                ShotGunObj.SetActive(true);
                RifleGunObj.SetActive(false);
                PlasmaGunObj.SetActive(false);
                break;
            case 2:
                DefaultGunObj.SetActive(false);
                ShotGunObj.SetActive(false);
                RifleGunObj.SetActive(true);
                PlasmaGunObj.SetActive(false);
                break;
            case 3:
                DefaultGunObj.SetActive(false);
                ShotGunObj.SetActive(false);
                RifleGunObj.SetActive(false);
                PlasmaGunObj.SetActive(true);
                break;
            default: break;

        }
        int[] arr = PlayerInventory.GetWeaponArray();
        for (int i = 0;arr.Length > i; i++)
        {

                WeaponObjArr[i][arr[i]].SetActive(true);

        }
    }
}
