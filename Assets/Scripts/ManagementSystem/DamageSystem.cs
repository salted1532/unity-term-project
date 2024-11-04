using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DamageSystem : MonoBehaviour
{
    public float CurrentHp;
    public float CurrentSp;
    public Slider Hpslider;
    public Slider Spslider;

    public GameObject TakeDamageImage;

    public void Start()
    {
        Hpslider.maxValue = CurrentHp;
        Hpslider.value = CurrentHp;
        Spslider.maxValue = 100;
        Spslider.value = CurrentSp;
        TakeDamageImage.SetActive(false);
    }
    //데미지 받아오기 
    public void TakeDamage(float amount)
    {
        Debug.Log("데미지 받음");

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
        Debug.Log("회복 받음");

        float GHealth = amount;
        CurrentHp += GHealth;
        Hpslider.value = CurrentHp;
        Debug.Log(Hpslider.value);
    }

    public void GetShield(float amount)
    {
        Debug.Log("회복 받음");

        float GShield = amount;
        CurrentSp += GShield;
        Spslider.value = CurrentSp;
        Debug.Log(Spslider.value);
    }

    private void Update()
    {
        //죽었는지 확인
        if (CurrentHp <= 0)
        {
            Destroy(gameObject);

        }
    }
}
