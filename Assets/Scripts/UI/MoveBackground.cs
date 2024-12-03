using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{

    public float speed = 2f; // 움직이는 속도
    public float distance = 3f; // 이동 거리

    private Vector3 startPosition;

    void Start()
    {
        // 오브젝트의 초기 위치 저장
        startPosition = transform.position;
    }

    void Update()
    {
        // 새로운 위치 계산
        float offset = Mathf.Sin(Time.time * speed) * distance;

        // X축 이동
        transform.position = new Vector3(startPosition.x + offset, startPosition.y, startPosition.z);
    }
}
