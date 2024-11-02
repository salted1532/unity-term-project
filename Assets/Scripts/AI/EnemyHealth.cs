using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float EnemyHp = 20f;
    public Slider EnemyHpslider;

    public Animator animator;

    public int WhatStageEnemy;

    private StageControl stagecontrol;

    // Start is called before the first frame update
    void Start()
    {
        EnemyHpslider.maxValue = EnemyHp;
        EnemyHpslider.value = EnemyHp;
        stagecontrol = GameObject.Find("StageManage").GetComponent<StageControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Y))
        {
            EnemyTakeDamage(10f);
        }
    }
    public void EnemyTakeDamage(float amount)
    {
        Debug.Log("데미지 받음");

        float TDamge = amount;

        if (TDamge > 0)
        {
            EnemyHp -= TDamge;
        }
        if (TDamge < 0)
        {
            EnemyHp -= 1;
        }
        EnemyHpslider.value = EnemyHp;
        Debug.Log(EnemyHpslider.value);
        //죽었는지 확인
        if (EnemyHp <= 0)
        {
            if (WhatStageEnemy == 1)
            {
                stagecontrol.howEnemyleft(1);
                animator.SetTrigger("Dead");
                Invoke(nameof(EnemyDead), 0.4f);
            }
            if (WhatStageEnemy == 2)
            {
                stagecontrol.howEnemyleft(2);
                animator.SetTrigger("Dead");
                Invoke(nameof(EnemyDead), 0.4f);
            }
            if (WhatStageEnemy == 3)
            {
                stagecontrol.howEnemyleft(3);
                animator.SetTrigger("Dead");
                Invoke(nameof(EnemyDead), 0.4f);
            }
            if (WhatStageEnemy == 4)
            {
                stagecontrol.howEnemyleft(4);
                animator.SetTrigger("Dead");
                Invoke(nameof(EnemyDead), 0.4f);
            }
            
        }
    }

    public void EnemyDead()
    {
        Destroy(gameObject);
    }
}
