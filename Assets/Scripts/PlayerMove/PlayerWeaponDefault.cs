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
        Debug.Log("히트스캔 진입!");
        RaycastHit hit; 
        if (Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward,out hit, defaultWeaponMaxDistance,7))
        {
            if(hit.collider != null)
            {
                //damage function
                DamageEvent?.Invoke(damage);
                Instantiate(PreFebBullet);
                Debug.Log("맞추기 성공!");
                Debug.Log("Hit " + hit.collider.name);
            }
        }
    }
}
