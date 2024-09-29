using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerWeaponDefault : MonoBehaviour
{
    public float defaultWeaponMaxDistance = 50f;
    public float damage = 10f;
    public UnityEvent<float> DamageEvent = new UnityEvent<float>();
    public GameObject PreFebBullet;
    public float DefaultCooldownTime = 0.35f;

    private void Awake()
    {
        PlayerState.PlayerAttackMaxCooldownTime = DefaultCooldownTime;
    }
    public void HitScan()
    {
        Debug.Log("È÷Æ®½ºÄµ ÁøÀÔ!");
        RaycastHit hit; 
        if (Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward,out hit, defaultWeaponMaxDistance))
        {
            if(hit.collider != null)
            {
                //damage function
                DamageEvent?.Invoke(damage);
                Instantiate(PreFebBullet);
                Debug.Log("½î±â ¼º°ø!");

            }
        }
    }
}
