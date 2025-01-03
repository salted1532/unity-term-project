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

    public UnityEvent PlayerDead;
    public bool isEventActive = false;

    public void Start()
    {
        Hpslider.maxValue = CurrentHp; 
        Hpslider.value = CurrentHp;
        Spslider.maxValue = 100;
        Spslider.value = CurrentSp;
        TakeDamageImage.SetActive(false);

        // ���콺�� ȭ�� ����� ������Ű�� �����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Invoke("KillPlayer", 1f);
    }

    public void KillPlayer()
    {
        //Time.timeScale = 0;
        TakeDamage(100);
    }

    //������ �޾ƿ��� 
    public void TakeDamage(float amount)
    {
        Debug.Log("������ ����");

        float TDamge = amount;
        int random = Random.Range(1,7);

        if (TDamge > 0)
        {
            if(CurrentSp <= 0)
            {
                CurrentHp -= TDamge;
            }
            else if((CurrentSp - TDamge) >= 0) {
                 CurrentSp -= TDamge;
            }
            SoundManager.Instance.PlaySound2D("FX_Fire_Magic_Impact_Small_0" + random,0f,false,SoundType.GUN);
        }
        if (TDamge < 0)
        {
            CurrentHp -= 1;
        }
        Spslider.value = CurrentSp;
        Hpslider.value = CurrentHp;
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
        if (CurrentHp > 100)
        {
            CurrentHp = 100;
        }
        Hpslider.value = CurrentHp;
        Debug.Log(Hpslider.value);
    }

    public void GetShield(float amount)
    {
        Debug.Log("ȸ�� ����");

        float GShield = amount;
        CurrentSp += GShield;
        if (CurrentSp > 100)
        {
            CurrentSp = 100;
        }
        Spslider.value = CurrentSp;
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
