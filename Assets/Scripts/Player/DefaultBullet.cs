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
        Debug.Log("총알 생성 성공!");
        Debug.Log(transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(CamForward * defaultBulletSpeed * Time.deltaTime);
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
