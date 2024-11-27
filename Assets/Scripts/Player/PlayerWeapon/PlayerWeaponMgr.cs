using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


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

    public Color color1 = Color.yellow; // ù ��° ����
    public Color color2 = Color.red;    // �� ��° ����
    public float colorChangeSpeed = 0.5f; // ���� ���� �ӵ�
    public float dotChangeInterval = 0.5f; // �� ���� ���� ����
    private Coroutine reloadCoroutine = null;

    private String GunReloadSound;
    private bool isGunSoundPlayed;

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

        GunReloadSound = "pistol_reload1";
        isGunSoundPlayed = false;
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
            int random = Random.Range(1,3);

            switch (PlayerInventory.GetCurWeaponNum())
            {
                case 0:
                    SoundManager.Instance.PlaySound2D("pistol_fire" + random,0f,false,SoundType.GUN);

                    
                    DefaultGun.HitScan();
                    curCooldownTime = 0;

                    break;
                case 1:
                    SoundManager.Instance.PlaySound2D("shotgun_fire" + random,0f,false,SoundType.GUN);
                    Debug.Log("shotgun_fire" + random);


                   
                    ShotGun.HitScanShotGun();
                    curCooldownTime = 0;

                    break;
                    
                case 2:
                    SoundManager.Instance.PlaySound2D("smg1_fireburst1",0f,false,SoundType.GUN);


                   
                    RifleGun.HitScanRifle();
                    curCooldownTime = 0;
                    break;

                case 3:
                    SoundManager.Instance.PlaySound2D("airboat_gun_energy"+ random,0f,false,SoundType.GUN);


                   
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
            isGunSoundPlayed = false;
            curReLodingTime = 0;
            
            SoundManager.Instance.StopLoopSound("shotgun_reload3");
            if(ShotGunObj.activeSelf == true)
            {
                SoundManager.Instance.PlaySound2D("shotgun_cock",0f,false,SoundType.GUN);
            }
            else if(PlasmaGunObj.activeSelf == true)
            {
                SoundManager.Instance.PlaySound2D("energy_bounce2",0f,false,SoundType.GUN,0.5f);
            }
        }
        else
        {
            if(isGunSoundPlayed == false)
            {
                if(GunReloadSound == "shotgun_reload3")
                {
                    SoundManager.Instance.PlaySound2D(GunReloadSound,0f,true,SoundType.GUN);
                }
                else
                {
                    SoundManager.Instance.PlaySound2D(GunReloadSound,0f,false,SoundType.GUN);
                }
                isGunSoundPlayed = true;
            }
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
                GunReloadSound = "pistol_reload1";
                SoundManager.Instance.PlaySound2D("pistol_reload1",0f,false,SoundType.GUN);

                isReLoading = false;
                break;
            case 1:
                MaxBulletCount = ShotGun.MaxBulletCount;
                maxReLodingTime = ShotGun.maxReLodingTime;
                curBulletCount = ShotGun.curBoulletCount;
                curReLodingTime = ShotGun.curReLodingTime;
                PlayerState.SetMaxCooldownTIme(ShotGun.CooldownTime);
                curCooldownTime = ShotGun.CooldownTime - 0.15f;
                GunReloadSound = "shotgun_reload3";
                SoundManager.Instance.PlaySound2D("shotgun_cock",0f,false,SoundType.GUN);

                isReLoading = false;
                break;
            case 2:
                MaxBulletCount = RifleGun.MaxBulletCount;
                maxReLodingTime = RifleGun.maxReLodingTime;
                curBulletCount = RifleGun.curBoulletCount;
                curReLodingTime = RifleGun.curReLodingTime;
                PlayerState.SetMaxCooldownTIme(RifleGun.CooldownTime);
                curCooldownTime = RifleGun.CooldownTime - 0.3f;
                GunReloadSound = "smg1_reload";
                SoundManager.Instance.PlaySound2D("switch_burst",0f,false,SoundType.GUN);

                isReLoading = false;
                break;
            case 3:
                MaxBulletCount = PlasmaGun.MaxBulletCount;
                maxReLodingTime = PlasmaGun.maxReLodingTime;
                curBulletCount = PlasmaGun.curBoulletCount;
                curReLodingTime = PlasmaGun.curReLodingTime;
                PlayerState.SetMaxCooldownTIme(PlasmaGun.CooldownTime);
                curCooldownTime = PlasmaGun.CooldownTime - 0.5f;
                GunReloadSound = "hold_loop";
                SoundManager.Instance.PlaySound2D("energy_bounce2",0f,false,SoundType.GUN,0.5f);

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

            targetColor = curBulletCount <= MaxBulletCount * 0.2f ? new Color(1f, 0.27f, 0f) : Color.white; // ��Ȳ�� �Ǵ� ���
        }
    }

    void SmoothColorTransition()
    {
        if (!isReLoading)
        {
            // ������ ���������� ����
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
            // ���� ���� (Lerp�� õõ�� ��ȯ)
            colorTimer += Time.deltaTime * colorChangeSpeed;
            bulletText.color = Color.Lerp(color1, color2, Mathf.PingPong(colorTimer, 1));

            // �� ���� ����
            dotCount = (dotCount + 1) % 4; // 0, 1, 2, 3 �ݺ�
            bulletText.text = "<i>Reloading" + new string('.', dotCount) + "</i>";
            Debug.Log(bulletText.text);
            yield return new WaitForSeconds(dotChangeInterval);
        }

        bulletText.text = ""; // ������ ������ �ؽ�Ʈ �ʱ�ȭ
    }

}

