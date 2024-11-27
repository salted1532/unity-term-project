using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Plasma : MonoBehaviour
{
    Rigidbody rb;

    public GameObject BoomArea;
    public GameObject prefabEffect;
    Vector2 screenPoint;
    Ray ray;
    public float defaultBulletSpeed;

    public float speed = 10f; // �Ѿ��� �⺻ �ӵ�


    private Vector3 direction; // �Ѿ��� �̵� ����
 


    private void OnEnable()
    {
        direction = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z);

    }
    // Update is called once per frame
    void Update()
    {
        // ���� �̵�
        transform.position += direction.normalized * speed * Time.deltaTime;


  
        if ((transform.position - PlayerState.PlayerCurPos).sqrMagnitude > 500f)
        {
            Debug.Log("������ ��� �Ѿ� ���� ����!");

            CreateBoomArea();
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Item") && !other.CompareTag("Exception"))
        {
            Debug.Log("��ü�� ��Ƽ� �Ѿ� ���� ����!:  "+ other.name);  


                CreateBoomArea();

        }


    }
    void CreateBoomArea()
    {
        Instantiate(BoomArea,transform.position,Quaternion.identity,null);
        Instantiate(prefabEffect, transform.position, Quaternion.LookRotation(transform.position.normalized), null);
        Debug.Log("�ö�� ���� ����!");

        Destroy( gameObject);
    }

}
