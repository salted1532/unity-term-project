using System.Collections;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public GameObject Door1, Door2;
    private Vector3 door1OpenPosition, door2OpenPosition;
    private Vector3 door1ClosedPosition, door2ClosedPosition;
    private bool isOpening = false;
    private bool isClosing = false;
    private float speed = 2.0f; // 문이 열리고 닫히는 속도

    public bool RightAngle;

    // Start is called before the first frame update
    void Start()
    {
        // 초기 위치 설정
        door1ClosedPosition = Door1.transform.position;
        door2ClosedPosition = Door2.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpening)
        {
            if(RightAngle == false)
            {
                // 문이 열릴 목표 위치 설정
                door1OpenPosition = door1ClosedPosition + new Vector3(0, 0, 6);
                door2OpenPosition = door2ClosedPosition + new Vector3(0, 0, -6);

                // Door1과 Door2를 목표 위치로 이동
                Door1.transform.position = Vector3.Lerp(Door1.transform.position, door1OpenPosition, Time.deltaTime * speed);
                Door2.transform.position = Vector3.Lerp(Door2.transform.position, door2OpenPosition, Time.deltaTime * speed);

                // 도달 여부 확인 후 정지
                if (Vector3.Distance(Door1.transform.position, door1OpenPosition) < 0.01f &&
                    Vector3.Distance(Door2.transform.position, door2OpenPosition) < 0.01f)
                {
                    isOpening = false;
                }
            }
            else
            {
                // 문이 열릴 목표 위치 설정
                door1OpenPosition = door1ClosedPosition + new Vector3(-6, 0, 0);
                door2OpenPosition = door2ClosedPosition + new Vector3(6, 0, 0);

                // Door1과 Door2를 목표 위치로 이동
                Door1.transform.position = Vector3.Lerp(Door1.transform.position, door1OpenPosition, Time.deltaTime * speed);
                Door2.transform.position = Vector3.Lerp(Door2.transform.position, door2OpenPosition, Time.deltaTime * speed);

                // 도달 여부 확인 후 정지
                if (Vector3.Distance(Door1.transform.position, door1OpenPosition) < 0.01f &&
                    Vector3.Distance(Door2.transform.position, door2OpenPosition) < 0.01f)
                {
                    isOpening = false;
                }
            }

        }
        else if (isClosing)
        {
            if (RightAngle == false)
            {
                // 문이 열릴 목표 위치 설정
                door1OpenPosition = door1ClosedPosition + new Vector3(0, 0, 6);
                door2OpenPosition = door2ClosedPosition + new Vector3(0, 0, -6);

                // Door1과 Door2를 닫힌 위치로 이동
                Door1.transform.position = Vector3.Lerp(Door1.transform.position, door1ClosedPosition, Time.deltaTime * speed);
                Door2.transform.position = Vector3.Lerp(Door2.transform.position, door2ClosedPosition, Time.deltaTime * speed);

                // 도달 여부 확인 후 정지
                if (Vector3.Distance(Door1.transform.position, door1ClosedPosition) < 0.01f &&
                    Vector3.Distance(Door2.transform.position, door2ClosedPosition) < 0.01f)
                {
                    isClosing = false;
                }
            }
            else
            {
                // 문이 열릴 목표 위치 설정
                door1OpenPosition = door1ClosedPosition + new Vector3(-6, 0, 0);
                door2OpenPosition = door2ClosedPosition + new Vector3(6, 0, 0);
                // Door1과 Door2를 닫힌 위치로 이동
                Door1.transform.position = Vector3.Lerp(Door1.transform.position, door1ClosedPosition, Time.deltaTime * speed);
                Door2.transform.position = Vector3.Lerp(Door2.transform.position, door2ClosedPosition, Time.deltaTime * speed);

                // 도달 여부 확인 후 정지
                if (Vector3.Distance(Door1.transform.position, door1ClosedPosition) < 0.01f &&
                    Vector3.Distance(Door2.transform.position, door2ClosedPosition) < 0.01f)
                {
                    isClosing = false;
                }
            }

        }
    }

    public void StageClear()
    {
        isOpening = true;
    }

    void OnTriggerEnter(Collider other)
    {
        // 플레이어가 충돌할 때 문 열기
        if (other.gameObject.CompareTag("Player"))
        {
            isOpening = true;
            isClosing = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // 플레이어가 충돌 영역에서 나갈 때 문 닫기
        if (other.gameObject.CompareTag("Player"))
        {
            isOpening = false;
            isClosing = true;
        }
    }
}
