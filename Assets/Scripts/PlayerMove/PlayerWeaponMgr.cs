using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWeaponMgr : MonoBehaviour
{
    
    public UnityEvent WeaponAction;

    float curCooldownTime;

    private void Awake()
    {
     
    }
    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && !PlayerState.PlayerIsDashing)
        {
            Fire();
        }
        if(curCooldownTime < PlayerState.PlayerAttackMaxCooldownTime)
        {
            curCooldownTime += Time.deltaTime;
        }
    }
    
    void Fire()
    {
        SoundManager.Instance.PlaySound2D("EFFECT_Click_Mechanical");
        Debug.Log("�� üũ ����");
        if (curCooldownTime > PlayerState.PlayerAttackMaxCooldownTime)
        {
            Debug.Log("��� ����");

            WeaponAction.Invoke();
            curCooldownTime = 0;

        }
    }

}
