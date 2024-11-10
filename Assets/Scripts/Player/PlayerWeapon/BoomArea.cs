using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoomArea : MonoBehaviour
{
    public UnityEvent<float> DamageEvent = new UnityEvent<float>();
    public float damage = 30f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,0.4f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        other.GetComponent<EnemyHealth>().EnemyTakeDamage(damage);

    }
}
