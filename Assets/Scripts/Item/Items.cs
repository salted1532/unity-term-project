using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public float floatSpeed = 1.0f;
    public float floatAmplitude = 0.1f;

    public float rotationSpeed = 50.0f;

    private Vector3 startPosition;

    [SerializeField]
    private int itemmanage = 0; //Switch문 이용해서 관리 
    //0 = 체력회복, 1 = 쉴드회복

    private DamageSystem damageSystem;

    void Start()
    {
        startPosition = transform.position;
        damageSystem = GameObject.Find("Player").GetComponent<DamageSystem>();  
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(itemmanage == 0)
            {
                damageSystem.GetHealth(10f);
            }
            if(itemmanage == 1)
            {
                damageSystem.GetShield(10f);
            }
            Destroy(gameObject);
        }
        else return;
    }
}
