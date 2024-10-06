using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBullet : MonoBehaviour
{
    public float defaultBulletSpeed = 2.5f;
    public Transform cam;
    Vector3 CamForward;
 
    void Start()
    {
        CamForward = Camera.main.transform.forward;
        transform.position = PlayerState.PlayerCurPos + Vector3.up;
        transform.rotation = Quaternion.Euler(CamForward);
        Debug.Log("�Ѿ� ���� ����!");
        Debug.Log(transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(CamForward * defaultBulletSpeed * Time.deltaTime);
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
