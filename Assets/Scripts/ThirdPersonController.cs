using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonController : MonoBehaviour
{
    public float moveSpeed = 5f; // �̵� �ӵ�
    public float turnSpeed = 700f; // ȸ�� �ӵ�
    public float jumpForce = 7f; // ���� ��
    public Transform playerCamera; // ī�޶��� Transform
    private Vector3 cameraOffset = new Vector3(5 , 2.5f, 5); // ī�޶��� ������, ������ �κ�
    public float lookSensitivity = 2f; // ���콺 ����

    private Rigidbody rb;
    private bool isGrounded;

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
        moveDirection.y = 0; // Y�� �̵��� 0���� �����Ͽ� ���� �̵��� ����

        // �̵��� �����մϴ�.
        rb.MovePosition(transform.position + moveDirection.normalized * moveSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void FollowPlayer()
    {
        // ī�޶� �÷��̾��� ���ʰ� ���ʿ� ��ġ��ŵ�ϴ�.
        Vector3 desiredPosition = transform.position - transform.forward * cameraOffset.z + Vector3.up * cameraOffset.y;
        playerCamera.position = desiredPosition;
        playerCamera.LookAt(transform.position); // ī�޶� �׻� �÷��̾ �ٶ󺸰� �մϴ�.
    }
}