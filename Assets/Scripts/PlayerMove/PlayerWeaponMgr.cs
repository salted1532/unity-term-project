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
        Debug.Log("ƒ √º≈© ¡¯¿‘");
        if (curCooldownTime > PlayerState.PlayerAttackMaxCooldownTime)
        {
            Debug.Log("ΩÓ±‚ ¡¯¿‘");

            WeaponAction.Invoke();
            curCooldownTime = 0;

        }
    }

}
