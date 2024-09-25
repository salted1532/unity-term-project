using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DamageSystem : MonoBehaviour
{
    public float CurrentHp;
    public Slider Hpslider;

    public void Start()
    {
        Hpslider.maxValue = CurrentHp;
        Hpslider.value = CurrentHp;
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
        Debug.Log(Hpslider.value);

    }

    private void Update()
    {
        //죽었는지 확인
        if (CurrentHp <= 0)
        {
            Destroy(this);
        }
    }
}
