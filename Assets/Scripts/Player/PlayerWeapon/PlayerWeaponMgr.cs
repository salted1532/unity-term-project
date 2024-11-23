using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public TMP_Text bulletText;

    public bool isReLoading;

    public int MaxBulletCount;

    public int curBulletCount;

    public float maxReLodingTime;

    public float curReLodingTime;

    float curCooldownTime;

    public GameObject Player;

    private Color targetColor;
    private Color currentColor;

    public Color color1 = Color.yellow; // 첫 번째 색상
    public Color color2 = Color.red;    // 두 번째 색상
    public float colorChangeSpeed = 0.5f; // 색상 변경 속도
    public float dotChangeInterval = 0.5f; // 점 개수 변경 간격
    private Coroutine reloadCoroutine = null;
    private void Awake()
    {
        DefaultGunObj.SetActive(true);
        ShotGunObj.SetActive(false);
        RifleGunObj.SetActive(false);
        PlasmaGunObj.SetActive(false);

        MaxBulletCount = DefaultGun.MaxBulletCount;
        maxReLodingTime = DefaultGun.maxReLodingTime; 
        curBulletCount = DefaultGun.curBoulletCount;
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
        UpdateBulletText();
        SmoothColorTransition();
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

                    
                    DefaultGun.HitScan();
                    curCooldownTime = 0;

                    break;
                case 1:
                    SoundManager.Instance.PlaySound2D("EFFECT_Click_Mechanical");


                   
                    ShotGun.HitScanShotGun();
                    curCooldownTime = 0;

                    break;
                    
                case 2:
                    SoundManager.Instance.PlaySound2D("EFFECT_Click_Mechanical");


                   
                    RifleGun.HitScanRifle();
                    curCooldownTime = 0;
                    break;

                case 3:
                    SoundManager.Instance.PlaySound2D("EFFECT_Click_Mechanical");


                   
                    PlasmaGun.CreatePlasma();
                    curCooldownTime = 0;
                    break;

                default:

                    break;
            }
            curBulletCount--;

            if (curBulletCount <= 0)
            {
                isReLoading = true;
                

            }

        }


    }
    void ReRoadGun()
    {
        if (curReLodingTime > maxReLodingTime)
        {
            curBulletCount = MaxBulletCount;
            isReLoading = false;
            curReLodingTime = 0;
            
        }
        else
        {
            curReLodingTime += Time.deltaTime;
            

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
                curBulletCount = DefaultGun.curBoulletCount;
                curReLodingTime = DefaultGun.curReLodingTime;
                PlayerState.SetMaxCooldownTIme(DefaultGun.CooldownTime);
                curCooldownTime = DefaultGun.CooldownTime - 0.115f;

                isReLoading = false;
                break;
            case 1:
                MaxBulletCount = ShotGun.MaxBulletCount;
                maxReLodingTime = ShotGun.maxReLodingTime;
                curBulletCount = ShotGun.curBoulletCount;
                curReLodingTime = ShotGun.curReLodingTime;
                PlayerState.SetMaxCooldownTIme(ShotGun.CooldownTime);
                curCooldownTime = ShotGun.CooldownTime - 0.15f;
                isReLoading = false;

                break;
            case 2:
                MaxBulletCount = RifleGun.MaxBulletCount;
                maxReLodingTime = RifleGun.maxReLodingTime;
                curBulletCount = RifleGun.curBoulletCount;
                curReLodingTime = RifleGun.curReLodingTime;
                PlayerState.SetMaxCooldownTIme(RifleGun.CooldownTime);
                curCooldownTime = RifleGun.CooldownTime - 0.3f;
                isReLoading = false;
                break;

            case 3:
                MaxBulletCount = PlasmaGun.MaxBulletCount;
                maxReLodingTime = PlasmaGun.maxReLodingTime;
                curBulletCount = PlasmaGun.curBoulletCount;
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

    void UpdateBulletText()
    {
        if (isReLoading)
        {
            if(reloadCoroutine == null)
            reloadCoroutine = StartCoroutine(ReloadingEffect());
        }
        else
        {
            if (reloadCoroutine != null)
            {
                StopCoroutine(reloadCoroutine);
                reloadCoroutine = null;
            }

            string bulletColor = curBulletCount <= MaxBulletCount * 0.2f ? "FF4500" : "FFFFFF";
            bulletText.text = $"<color=#{bulletColor}>{curBulletCount}</color> / <color=#00FF00>{MaxBulletCount}</color>";

            targetColor = curBulletCount <= MaxBulletCount * 0.2f ? new Color(1f, 0.27f, 0f) : Color.white; // 주황색 또는 흰색
        }
    }

    void SmoothColorTransition()
    {
        if (!isReLoading)
        {
            // 색상을 점진적으로 변경
            currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * 5f);
            bulletText.color = currentColor;
        }
    }

    public void StartReloading()
    {
        if (isReLoading) return;

        isReLoading = true;
        StartCoroutine(ReloadingEffect());
    }

    IEnumerator ReloadingEffect()
    {
        float colorTimer = 0;
        int dotCount = 0;

        while (isReLoading)
        {
            // 색상 변경 (Lerp로 천천히 전환)
            colorTimer += Time.deltaTime * colorChangeSpeed;
            bulletText.color = Color.Lerp(color1, color2, Mathf.PingPong(colorTimer, 1));

            // 점 개수 변경
            dotCount = (dotCount + 1) % 4; // 0, 1, 2, 3 반복
            bulletText.text = "<i>Reloading" + new string('.', dotCount) + "</i>";
            Debug.Log(bulletText.text);
            yield return new WaitForSeconds(dotChangeInterval);
        }

        bulletText.text = ""; // 재장전 끝나면 텍스트 초기화
    }

}

