using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public float floatSpeed = 1.0f;
    public float floatAmplitude = 0.1f;

    public float rotationSpeed = 50.0f;

    private Vector3 startPosition;

    private int itemmanage = 0; //Switch문 이용해서 관리 

    void Start()
    {
        startPosition = transform.position;
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
            //코드 구현
            Destroy(gameObject);
        }
        else return;
    }
}
