using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonController : MonoBehaviour
{
    public float moveSpeed = 100f; // �̵� �ӵ�
    public float turnSpeed = 700f; // ȸ�� �ӵ�
    public float jumpForce = 10f; // ���� ��
    public Transform playerCamera; // ī�޶��� Transform
    private Vector3 cameraOffset = new Vector3(5 , 1f, 5); // ī�޶��� ������, ������ �κ�
    public float lookSensitivity = 300f; // ���콺 ����

    private Rigidbody rb;
    [SerializeField]
    private bool isGrounded;

    [SerializeField]
    private bool isAbleDash = false;
    [SerializeField]
    private bool isDashing = false;

    private float timer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Rigidbody�� ȸ�� ����

        // ���콺�� ȭ���� �߾ӿ� ����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMouseLook();
        MovePlayer();
        Jump();

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isAbleDash == true)
            {
                isDashing = true;
                OnDash();
            }
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            isAbleDash = true;
        }

        if (isAbleDash == true)
        {

            timer += Time.deltaTime;
            if (timer > 0.1f)
            {
                isAbleDash = false;
                timer = 0f;
            }
        }
    }

    private void LateUpdate()
    {
        FollowPlayer();
    }

    private void HandleMouseLook()
    {
        // ���콺 �Է��� ������� �÷��̾ ȸ����ŵ�ϴ�.
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;

        // ī�޶� �������� ȸ����ŵ�ϴ�.
        playerCamera.Rotate(Vector3.up * mouseX * Time.deltaTime);

        // �÷��̾ ī�޶��� ������ �ٶ󺸵��� �մϴ�.
        Vector3 targetDirection = playerCamera.forward;
        targetDirection.y = 0; // Y�� ������ 0���� �����Ͽ� ���� ȸ���� ����
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A, D, Left Arrow, Right Arrow
        float vertical = Input.GetAxis("Vertical"); // W, S, Up Arrow, Down Arrow

        // �÷��̾ �̵��� ������ �����մϴ�.
        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
       // moveDirection.y = 0; // Y�� �̵��� 0���� �����Ͽ� ���� �̵��� ����

        // �̵��� �����մϴ�.
        rb.MovePosition(transform.position + moveDirection.normalized * moveSpeed * Time.deltaTime);
    }

    private void OnDash() // 1
    {
        if (isAbleDash)
        {
            StartCoroutine(DashStart());
        }
    }

    private IEnumerator DashStart()
    {
        rb.AddForce(Vector3.forward * 0f, ForceMode.Impulse);

        isAbleDash = false;
        isDashing = false;

        yield return new WaitForSeconds(1f); // 1��
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    /*private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }*/

    private void FollowPlayer()
    {
        // ī�޶� �÷��̾��� ���ʰ� ���ʿ� ��ġ��ŵ�ϴ�.
        Vector3 desiredPosition = transform.position - transform.forward * cameraOffset.z + Vector3.up * cameraOffset.y;
        playerCamera.position = desiredPosition;
        playerCamera.LookAt(transform.position); // ī�޶� �׻� �÷��̾ �ٶ󺸰� �մϴ�.
    }
}