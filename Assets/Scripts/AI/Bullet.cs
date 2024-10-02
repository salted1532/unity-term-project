using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private DamageSystem player;

    public int AttackDamage;

    public void Start()
    {
        player = GameObject.Find("Player").GetComponent<DamageSystem>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(AttackDamage);
            Destroy(gameObject);
        }
    }
}
