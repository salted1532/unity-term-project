using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f; // 이동 속도 조정
    [SerializeField]
    private float turnSpeed = 700f; // 회전 속도
    [SerializeField]
    private float jumpForce = 10f; // 점프 힘
    public Transform playerCamera; // 카메라의 Transform
    private Vector3 cameraOffset = new Vector3(5, 1f, 5); // 카메라의 오프셋, 수정된 부분
    public float lookSensitivity = 300f; // 마우스 감도

    private Rigidbody rb;
    [SerializeField]
    private bool isGrounded;

    private Vector3 moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Rigidbody의 회전 동결

        // 마우스를 화면의 중앙에 고정
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
        // 마우스 입력을 기반으로 플레이어를 회전시킵니다.
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;

        // 카메라를 수평으로 회전시킵니다.
        playerCamera.Rotate(Vector3.up * mouseX * Time.deltaTime);

        // 플레이어가 카메라의 방향을 바라보도록 합니다.
        Vector3 targetDirection = playerCamera.forward;
        targetDirection.y = 0; // Y축 방향을 0으로 설정하여 수평 회전만 수행
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A, D, Left Arrow, Right Arrow
        float vertical = Input.GetAxis("Vertical"); // W, S, Up Arrow, Down Arrow

        // 플레이어가 이동할 방향을 설정합니다.
        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // 카메라의 방향을 기준으로 이동 방향을 변환합니다.
        moveDirection = playerCamera.TransformDirection(moveDirection);
        moveDirection.y = 0; // 수평 이동만

        // 이동을 적용합니다.
        rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);
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

    private void FollowPlayer()
    {
        // 카메라를 플레이어의 뒤쪽과 위쪽에 위치시킵니다.
        Vector3 desiredPosition = transform.position - transform.forward * cameraOffset.z + Vector3.up * cameraOffset.y;
        playerCamera.position = desiredPosition;
        playerCamera.LookAt(transform.position); // 카메라가 항상 플레이어를 바라보게 합니다.
    }
}
