using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class BoomArea : MonoBehaviour
{
    public UnityEvent<float> DamageEvent = new UnityEvent<float>();
    public float damage = 30f;

    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnEnable()
    {
        Debug.Log("�� ����");  
        Debug.Log(transform.position);
        SoundManager.Instance.PlaySound3D("energy_sing_explosion2",gameObject, 0, 25, false, SoundType.GUN);
        Destroy(gameObject, 1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().EnemyTakeDamage(damage);
        }

    }
}
