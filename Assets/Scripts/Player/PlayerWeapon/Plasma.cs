using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Plasma : MonoBehaviour
{
    Rigidbody rb;

    public GameObject BoomArea;


    public float defaultBulletSpeed;
    public Transform cam;
    Vector3 CamForward;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }
    void Start()
    {

        CamForward = Camera.main.transform.forward;
        transform.position = PlayerState.PlayerCurPos + Vector3.right;
        transform.rotation = Quaternion.Euler(CamForward);
    }

    // Update is called once per frame
    void Update()
    {

        rb.AddForce(CamForward * defaultBulletSpeed, ForceMode.Impulse);
        if ((transform.position - PlayerState.PlayerCurPos).sqrMagnitude > 250f)
        {
            Debug.Log("������ ��� �Ѿ� ���� ����!");

            CreateBoomArea();
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Debug.Log("��ü�� ��Ƽ� �Ѿ� ���� ����!:  "+ other.name);  


                CreateBoomArea();

        }


    }
    void CreateBoomArea()
    {
        Instantiate(BoomArea,transform);
        Debug.Log("�ö�� ���� ����!");

        gameObject.SetActive(false);

    }

}
