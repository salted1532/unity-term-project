using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using MagicPigGames;
using UnityEngine.Events;

public class DamageSystem : MonoBehaviour
{
    public float CurrentHp;
    public float CurrentSp;
    public Slider Hpslider;
    public Slider Spslider;

    public GameObject TakeDamageImage;
    public GameObject HealthBar;
    public GameObject ShieldBar;

    public UnityEvent PlayerDead;
    public bool isEventActive = false;

    public void Start()
    {
        Hpslider.maxValue = CurrentHp;
        Hpslider.value = CurrentHp;
        Spslider.maxValue = 100;
        Spslider.value = CurrentSp;
        TakeDamageImage.SetActive(false);

        SetHealth(CurrentHp);
        SetShield(CurrentSp);

        // ���콺�� ȭ�� ����� ������Ű�� �����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Invoke("KillPlayer", 1f);
    }

    public void KillPlayer()
    {
        //Time.timeScale = 0;
        //TakeDamage(100);
    }

    public void SetHealth(float amount)
    {
        HealthBar.GetComponent<ProgressBarInspectorTest>().progress = (amount/100);
    }

    public void SetShield(float amount)
    {
        ShieldBar.GetComponent<ProgressBarInspectorTest>().progress = (amount/100);
    }

    //������ �޾ƿ��� 
    public void TakeDamage(float amount)
    {
        Debug.Log("������ ����");

        float TDamge = amount;

        if (TDamge > 0)
        {
            CurrentHp -= TDamge;
        }
        if (TDamge < 0)
        {
            CurrentHp -= 1;
        }
        Hpslider.value = CurrentHp;
        SetHealth(CurrentHp);
        TakeDamageEffect();
        Debug.Log(Hpslider.value);
    }

    public void TakeDamageEffect()
    {
        TakeDamageImage.SetActive(true);
        Invoke(nameof(TakeDamageEffectoff), 1f);
    }
    public void TakeDamageEffectoff()
    {
        TakeDamageImage.SetActive(false);
    }
    public void GetHealth(float amount)
    {
        Debug.Log("ȸ�� ����");

        float GHealth = amount;
        CurrentHp += GHealth;
        SetHealth(CurrentHp);
        Hpslider.value = CurrentHp;
        Debug.Log(Hpslider.value);
    }

    public void GetShield(float amount)
    {
        Debug.Log("ȸ�� ����");

        float GShield = amount;
        CurrentSp += GShield;
        Spslider.value = CurrentSp;
        SetShield(CurrentSp);
        Debug.Log(Spslider.value);
    }

    private void Update()
    {
        //�׾����� Ȯ��
        if (CurrentHp <= 0)
        {
            if(isEventActive == false)
            {
                isEventActive = true;
                PlayerDead.Invoke();
            }
        }
    }
}
