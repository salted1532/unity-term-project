using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Items : MonoBehaviour
{
    public float floatSpeed = 1.0f;
    public float floatAmplitude = 0.1f;

    public float rotationSpeed = 50.0f;
    private Vector3 startPosition;

    [SerializeField]
    private int itemmanage = 0; //Switch�� �̿��ؼ� ���� 
    //0 = ü��ȸ��, 1 = ����ȸ��

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

    public int GetNum() { return itemmanage; }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch(itemmanage)
        {
            case 10:
                    damageSystem.GetHealth(10f);
                    gameObject.SetActive(false);

            break;

            case 20:
                    damageSystem.GetShield(10f);
                    gameObject.SetActive(false);

            break;

            default:
            break;
        }
        }
        else return;
    }
}
