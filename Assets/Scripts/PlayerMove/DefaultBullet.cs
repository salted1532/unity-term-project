using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBullet : MonoBehaviour
{
    public float defaultBulletSpeed = 1f;
    public Transform cam;
    Quaternion rot;
    private void Awake()
    {

    }
    void Start()
    {
        rot = Camera.main.transform.rotation;
        transform.position = PlayerState.PlayerCurPos + Vector3.up;
        transform.rotation = rot;
        Debug.Log("�Ѿ� ���� ����!");
        Debug.Log(transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * defaultBulletSpeed * Time.deltaTime);
        if ((transform.position - PlayerState.PlayerCurPos).sqrMagnitude > 2500f)
        {
            Debug.Log("������ ��� �Ѿ� ���� ����!");

            Destroy(gameObject);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Debug.Log("��ü�� ��Ƽ� �Ѿ� ���� ����!");
            Destroy(gameObject);
        }


    }
}
