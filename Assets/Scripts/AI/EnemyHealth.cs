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
        if (stagecontrol != null)
        {
            if(WhatStageEnemy == 1)
            {
                stagecontrol.stage1EnemyObj.Add(gameObject);
            }
            if (WhatStageEnemy == 2)
            {
                stagecontrol.stage2EnemyObj.Add(gameObject);
            }
            if (WhatStageEnemy == 3)
            {
                stagecontrol.stage3EnemyObj.Add(gameObject);
            }
            if (WhatStageEnemy == 4)
            {
                stagecontrol.stage4EnemyObj.Add(gameObject);
            }

        }
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
        Debug.Log("������ ����");

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
        //�׾����� Ȯ��
        if (EnemyHp <= 0)
        {
            //SoundManager.Instance.PlaySound3D("MON_FacelessOne_v2_death", gameObject, 0, 25, false, SoundType.MONSTER_SOUND);
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
