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

    public float speed = 10f; // 총알의 기본 속도


    private Vector3 direction; // 총알의 이동 방향
 


    private void OnEnable()
    {
        direction = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z);

    }
    // Update is called once per frame
    void Update()
    {
        // 직진 이동
        transform.position += direction.normalized * speed * Time.deltaTime;


  
        if ((transform.position - PlayerState.PlayerCurPos).sqrMagnitude > 500f)
        {
            Debug.Log("범위를 벗어나 총알 삭제 성공!");

            CreateBoomArea();
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Item"))
        {
            Debug.Log("물체에 닿아서 총알 삭제 성공!:  "+ other.name);  


                CreateBoomArea();

        }


    }
    void CreateBoomArea()
    {
        Instantiate(BoomArea,transform.position,Quaternion.identity,null);
        Instantiate(prefabEffect, transform.position, Quaternion.LookRotation(transform.position.normalized), null);
        Debug.Log("플라즈마 생성 성공!");

       Destroy( gameObject);

    }

}
