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
        Debug.Log("총알 생성 성공!");
        Debug.Log(transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * defaultBulletSpeed * Time.deltaTime);
        if ((transform.position - PlayerState.PlayerCurPos).sqrMagnitude > 2500f)
        {
            Debug.Log("범위를 벗어나 총알 삭제 성공!");

            Destroy(gameObject);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Debug.Log("물체에 닿아서 총알 삭제 성공!");
            Destroy(gameObject);
        }


    }
}
