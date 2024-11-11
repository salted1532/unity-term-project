using System.Collections;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public GameObject Door1, Door2;
    private Vector3 door1OpenPosition, door2OpenPosition;
    private Vector3 door1ClosedPosition, door2ClosedPosition;
    [SerializeField]
    private bool isOpening = false;
    [SerializeField]
    private bool isClosing = false;
    private float speed = 2.0f; // ���� ������ ������ �ӵ�

    public bool RightAngle;

    // Start is called before the first frame update
    void Start()
    {
        // �ʱ� ��ġ ����
        door1ClosedPosition = Door1.transform.position;
        door2ClosedPosition = Door2.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpening == true)
        {
            if(RightAngle == false)
            {
                // ���� ���� ��ǥ ��ġ ����
                door1OpenPosition = door1ClosedPosition + new Vector3(0, 0, 6);
                door2OpenPosition = door2ClosedPosition + new Vector3(0, 0, -6);

                // Door1�� Door2�� ��ǥ ��ġ�� �̵�
                Door1.transform.position = Vector3.Lerp(Door1.transform.position, door1OpenPosition, Time.deltaTime * speed);
                Door2.transform.position = Vector3.Lerp(Door2.transform.position, door2OpenPosition, Time.deltaTime * speed);

                // ���� ���� Ȯ�� �� ����
                if (Vector3.Distance(Door1.transform.position, door1OpenPosition) < 0.01f &&
                    Vector3.Distance(Door2.transform.position, door2OpenPosition) < 0.01f)
                {
                    isOpening = false;
                }
            }
            else if(RightAngle == true)
            {
                // ���� ���� ��ǥ ��ġ ����
                door1OpenPosition = door1ClosedPosition + new Vector3(-6, 0, 0);
                door2OpenPosition = door2ClosedPosition + new Vector3(6, 0, 0);

                // Door1�� Door2�� ��ǥ ��ġ�� �̵�
                Door1.transform.position = Vector3.Lerp(Door1.transform.position, door1OpenPosition, Time.deltaTime * speed);
                Door2.transform.position = Vector3.Lerp(Door2.transform.position, door2OpenPosition, Time.deltaTime * speed);

                // ���� ���� Ȯ�� �� ����
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
                // ���� ���� ��ǥ ��ġ ����
                door1OpenPosition = door1ClosedPosition + new Vector3(0, 0, 6);
                door2OpenPosition = door2ClosedPosition + new Vector3(0, 0, -6);

                // Door1�� Door2�� ���� ��ġ�� �̵�
                Door1.transform.position = Vector3.Lerp(Door1.transform.position, door1ClosedPosition, Time.deltaTime * speed);
                Door2.transform.position = Vector3.Lerp(Door2.transform.position, door2ClosedPosition, Time.deltaTime * speed);

                // ���� ���� Ȯ�� �� ����
                if (Vector3.Distance(Door1.transform.position, door1ClosedPosition) < 0.01f &&
                    Vector3.Distance(Door2.transform.position, door2ClosedPosition) < 0.01f)
                {
                    isClosing = false;
                }
            }
            else
            {
                // ���� ���� ��ǥ ��ġ ����
                door1OpenPosition = door1ClosedPosition + new Vector3(-6, 0, 0);
                door2OpenPosition = door2ClosedPosition + new Vector3(6, 0, 0);
                // Door1�� Door2�� ���� ��ġ�� �̵�
                Door1.transform.position = Vector3.Lerp(Door1.transform.position, door1ClosedPosition, Time.deltaTime * speed);
                Door2.transform.position = Vector3.Lerp(Door2.transform.position, door2ClosedPosition, Time.deltaTime * speed);

                // ���� ���� Ȯ�� �� ����
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
        // �÷��̾ �浹�� �� �� ����
        if (other.gameObject.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySound3D("heavy_metal_move1", Door1);
            isOpening = true;
            isClosing = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // �÷��̾ �浹 �������� ���� �� �� �ݱ�
        if (other.gameObject.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySound3D("heavy_metal_move1", Door1);
            isOpening = false;
            isClosing = true;
        }
    }
}
