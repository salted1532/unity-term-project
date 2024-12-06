
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour
{
     Animator anim;

    public CinemachineFreeLook freeLookCamera;
    private float targetFOV;

    public PlayerWeaponMgr playerWeaponMgr;
    public PlayerScanner playerScanner;
    public float MaxHP = 500;
    //cur == Current
    float CurHP;

    public CharacterController playerControl;
    public Transform cam;
    public Transform Aim;
    public Transform Aim2;
    Vector3 originalPos;
     
    public Vector3 movement = Vector3.zero;
    private Vector3 dir = Vector3.zero;

    private List<KeyCode> pressedKeysX = new List<KeyCode>();
    private List<KeyCode> pressedKeysZ = new List<KeyCode>();

    [SerializeField]
    float deltaSpeed = 1.8f;
    [SerializeField]
    float MaxSpeed = 6f;

    [SerializeField]
    public float dashDuration = 0.5f;    
    [SerializeField]
    private float dashTimeRemaining;
    Vector3 dashDir;

    [SerializeField]
    float NomalGravity = 28;

    [SerializeField]
    float MinGravity = 20;

    float gravity;

    float Velocity = 0f;
    float VelocityY = 0f;
    [SerializeField]
    float jumpForce = 8f;
    float terminalVelocity = -20f;
    float acceleration;

    bool isHoldingJump;
    bool startedJump;
    bool isJumpingFirst;

    bool isCallJump;
    [SerializeField]
    float dashSpeed = 8f;
    bool startDash;
    bool canDash;
    bool isDashing;
    bool isZoomIn;

    bool canDoubleJump;
    [SerializeField]
    bool DoubleJumpLock;

    [SerializeField]
    int layerMask = 3;
    [SerializeField]
    float MaxSwapDelay;
    float SwapDelayDeltaTime;

    Vector3[] RayTestArr = new Vector3[10];

    private float currentTime = 0f;
    public bool isground;
    public bool IsJumping;
    public bool isControllerActive;

    public UnityEvent ChangePauseState;

    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        RayTest();
    }
    private void Start()
    {
        originalPos = transform.position;
        LookAtCam();
        PlayerInventory.ReSetInventory();
        targetFOV = freeLookCamera.m_Lens.FieldOfView;
        isControllerActive = true;
        IsJumping = false;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isSettingScreen() == true)
            {
                GameObject.Find("PauseUI").GetComponent<ShowPauseUI>().DisableSettingScreen();
            }
            else
            {
                ChangePause();
            }
        }

        if(isControllerActive == false)
        {
            return;
        }
        
        isground = IsCheckGrounded();
        currentTime += Time.deltaTime;
        if(IsWalking())
        {
            if(SoundManager.Instance.IsSoundInLoop("concrete1") == false)
            {
                SoundManager.Instance.PlaySound3D("concrete1", gameObject, 0, 25, true);
            }
        }
        else
        {
            if(SoundManager.Instance.IsSoundInLoop("concrete1") == true)
            {
                SoundManager.Instance.StopLoopSound("concrete1");
            }
        }

        
        SwapWeaponListener();
        if (isDashing)
        {
            Dash();
        }
        else
        {
            ControlPlayer();
        }
        zoomInCam();

        AnimPlay();

        //Debug.Log(playerControl.isGrounded);
        
    }
    void RayTest()
    {
        for (int i = 0; i < RayTestArr.Length-1; ++i)
        {
            // 랜덤한 각도로 회전하여 레이 방향 설정
            Vector2 randomCircle = Random.insideUnitCircle * 0.12f;
            Vector3 spreadDirection = Camera.main.transform.forward +
                                      transform.right * randomCircle.x +
                                      transform.up * randomCircle.y;
            RayTestArr[i] = spreadDirection;

        }
    }
    private void FixedUpdate()
    {

 

    }
    private void LateUpdate()
    {

        PlayerState.PlayerCurPos = transform.position;

        LookAtCam();
    }

    void SwapWeaponListener()
    {
        SwapDelayDeltaTime += Time.deltaTime;
        if (MaxSwapDelay < SwapDelayDeltaTime)
        {
            float wheelInput = Input.GetAxis("Mouse ScrollWheel");
            if (PlayerInventory.SwapWeapon(wheelInput))
            {
                playerWeaponMgr.SetCurWeaponData();
                SwapDelayDeltaTime = 0;

            }

        }

    }
    public void OnDamage(float Damage)
    {
        if (Damage < CurHP)
        {
            CurHP -= Damage;
        }
        else
        {
            PlayerDead();
        }
    }
    void PlayerDead()
    {
        Restart();
        CurHP = MaxHP;
    }

    void ControlPlayer()
    {
        if ( isJumpingFirst &&  playerControl.isGrounded)
        {
            isJumpingFirst = false;

        }
        if (canDoubleJump && playerControl.isGrounded )
        {
            canDoubleJump = false;
        }
        if (isCallJump == false) 
        {
            if (!isCallJump && !canDoubleJump && !isJumpingFirst && Input.GetButtonDown("Jump"))
            {
                Jump();
            }
            else if (!DoubleJumpLock && canDoubleJump && isJumpingFirst && Input.GetButtonDown("Jump"))    //doubleJump
            {
                DoubleJump();
            }
        }
            if (isCallJump && playerControl.isGrounded)                            //callbackJump
            {
                Jump();
                isCallJump = false;

            }
            PlayerMove();

            /*if (transform.position.y < -1.3)
            {
                Restart();
                return;
            }*/


            playerControl.Move(new Vector3(0, VelocityY, 0) * Time.deltaTime);
            Gravity();                                                                  //Gravity

            if (IsFalling())                                                                //callBackJumpSwicth
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, Vector3.down, out hit, 3.8f))
                {
                    Debug.DrawRay(transform.position, Vector3.down * 3.8f, Color.red);

                    if (hit.collider.CompareTag("Ground") && Input.GetButtonDown("Jump"))
                    {
                        isCallJump = true;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                DashStart();
            }
            if(IsJumping == true && playerControl.isGrounded == true)
            {
                IsJumping = false;
            }
    }

    void PlayerMove()
    {
        if (Input.anyKey || Input.anyKeyDown)
        {
            InputKeyListEvent();
            // Determine movement direction
            if (pressedKeysX.Count > 0 || pressedKeysZ.Count > 0)
            {
                if (pressedKeysX.Count > 0)
                {
                    KeyCode directionKeyX = pressedKeysX[pressedKeysX.Count - 1]; // Get the most recently pressed key
                    switch (directionKeyX)
                    {

                        case KeyCode.A:
                            movement.x = -1;
                            break;
                        case KeyCode.D:
                            movement.x = 1;
                            break;
                    }

                }
                // Determine movement direction
                if (pressedKeysZ.Count > 0)
                {
                    KeyCode directionKeyZ = pressedKeysZ[pressedKeysZ.Count - 1]; // Get the most recently pressed key
                    switch (directionKeyZ)
                    {

                        case KeyCode.W:
                            movement.z = 1;
                            break;
                        case KeyCode.S:
                            movement.z = -1;
                            break;
                    }

                }

                if (pressedKeysX.Count == 0)
                {
                    movement.x = 0;
                }
                else if (pressedKeysZ.Count == 0)
                {
                    movement.z = 0;
                }
                PlayerGroundMove();
            }
        }else
        {
            if (pressedKeysX.Count > 0)
            {
                pressedKeysX.Clear();
                movement.x = 0;

            }
            if (pressedKeysZ.Count > 0)
            {
                pressedKeysZ.Clear();
                movement.z = 0;
            }

            if (Velocity != 0)
            {
                Velocity = 0;
            }
        }
    }


    bool CheckForInput()
    {
        if (movement.x != 0 || movement.z != 0)
            return true;
        else
            return false;
    }
    //Check if character is falling
    bool IsFalling()
    {
        if (!playerControl.isGrounded && VelocityY <= 0)
        {
            return true;
        }
        else return false;
    }

    bool IsFallingForMove()
    {
        if (!playerControl.isGrounded && VelocityY < -10)
        {
            return true;
        }
        else return false;
    }

    bool IsJumPing()
    {
        if (!playerControl.isGrounded && VelocityY > 1)
        {
            return true;
        }
        else return false;
    }

    
    public bool IsCheckGrounded()
    {
        if (playerControl.isGrounded) return true;
        
        var ray = new Ray(this.transform.position + Vector3.up * 0.1f, Vector3.down);
        var maxDistance = 4f;

        //Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * maxDistance, Color.red);
        return Physics.Raycast(ray, maxDistance);
    }

    bool IsWalking()
    {
        if(IsFallingForMove() || !(IsCheckGrounded()) || IsJumping == true || (movement.x == 0 && movement.z == 0))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //Apply gravity to the character
    void Gravity()
    {
        //Set isHoldingJump relative to if the player is holding the jump button
        if (Input.GetButton("Jump"))
        {
            isHoldingJump = true;
        }
        else
        {
            isHoldingJump = false;
        }


        //If character is grounded and he isn't jumping, apply a miniscule amount of gravity.
        if (playerControl.isGrounded && !startedJump)
        {
            gravity = MinGravity;
            VelocityY = 0;
        }

        //If the player is jumping (not falling) and they aren't holding the jump button, increase their gravity
        //Creates short-hop like jump
        if (!IsFalling() && !playerControl.isGrounded)
        {
            if (!isHoldingJump)
            {
                gravity = NomalGravity;
            }
            else
                gravity = MinGravity;
        }

        //If character is airborne, subtract gravity from velocity.
        if (!playerControl.isGrounded)
        {
            VelocityY -= gravity * Time.deltaTime;
            startedJump = false;
        }
        if (startedJump)
        {
            gravity = MinGravity;
        } 
        //If the character is falling (not jumping), revert gravity to normal.
        if (IsFalling())
        {
            gravity = NomalGravity;
        }

        //Ensure the character doesn't fall faster than their terminalVelocity.
        if (IsFalling() && VelocityY < terminalVelocity)
        {
            VelocityY = terminalVelocity;
        }
    }
    //Change the value of yVelocity, allowing the character to jump when MoveChar() is called
    void Jump()
    {
        if (playerControl.isGrounded)
        {
            SoundManager.Instance.PlaySound2D("EFFECT_Click_Mechanical");
            VelocityY = jumpForce;
            startedJump = true;
            isJumpingFirst = true;
            canDoubleJump = true;
            IsJumping = true;
        }

    }
    void DoubleJump()
    {
        if (isJumpingFirst)
        {
            Debug.Log("더블 점프!");
            SoundManager.Instance.PlaySound2D("EFFECT_Click_Mechanical");
            VelocityY = jumpForce * 1.6f;
            startedJump = true;
            isJumpingFirst = false;
            canDoubleJump = false;
            IsJumping = true;
        }
    }
     
    float DampSpeed(float originalVelocity)
    {
        //Double the character's deceleration when there is no input and the character is grounded.
        if (!CheckForInput() && playerControl.isGrounded)
            acceleration = 10;
        else
            acceleration = 2.5f;

        //Get the magnitude of the stick's tilt * by maxSpeed to get the desired speed.
        float desiredSpeed = MaxSpeed * movement.magnitude;

        float new_ratio = 0.9f * Time.deltaTime * acceleration;
        float old_ratio = 1 - new_ratio;

        float newSpeed = (originalVelocity * old_ratio) + (desiredSpeed * new_ratio);

        newSpeed = Mathf.Clamp(newSpeed, -MaxSpeed * deltaSpeed, MaxSpeed * deltaSpeed);

        return newSpeed;
    }

    void LookAtCam()
    {
        Quaternion targetRotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);
        transform.rotation = targetRotation;
       

    }

    void zoomInCam()
    {
        if (Input.GetMouseButtonDown(1) && !isZoomIn)
        {
            targetFOV = 8f;
            freeLookCamera.m_XAxis.m_MaxSpeed /= 2f;
            freeLookCamera.m_YAxis.m_MaxSpeed /= 2f;

            isZoomIn = true;
            PlayerState.PlayerIsZooming = true;
        }
        else if (Input.GetMouseButtonDown(1) && isZoomIn)
        {
            targetFOV = 40f;
            freeLookCamera.m_YAxis.m_MaxSpeed *= 2f;

            freeLookCamera.m_XAxis.m_MaxSpeed *= 2f;

            isZoomIn = false;
            PlayerState.PlayerIsZooming = false;

        }
        if (freeLookCamera.m_Lens.FieldOfView != targetFOV)
        {
            freeLookCamera.m_Lens.FieldOfView = Mathf.Lerp(freeLookCamera.m_Lens.FieldOfView, targetFOV,  Time.deltaTime * ((freeLookCamera.m_Lens.FieldOfView < targetFOV) ? 100f:10f));

        }
    }

    void Restart()
    {
        transform.position = originalPos;
        Debug.Log("����!");
    }

    void InputKeyListEvent()
    {
        // Check for key down events
        if (Input.GetKeyDown(KeyCode.W))
        {
            pressedKeysZ.Add(KeyCode.W);

        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            pressedKeysZ.Add(KeyCode.S);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            pressedKeysX.Add(KeyCode.A);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            pressedKeysX.Add(KeyCode.D);
        }

        // Check for key up events
        if (Input.GetKeyUp(KeyCode.W))
        {
            pressedKeysZ.Remove(KeyCode.W);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            pressedKeysZ.Remove(KeyCode.S);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            pressedKeysX.Remove(KeyCode.A);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            pressedKeysX.Remove(KeyCode.D);
        }

    }

    void PlayerGroundMove()
    {
        //���� �Ķ����
        float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        if (moveDir.sqrMagnitude > 1f)
        {
            moveDir = moveDir.normalized;
        }
        Velocity = DampSpeed(Velocity);
        playerControl.Move(((moveDir * Time.deltaTime) * Velocity) * deltaSpeed);
    }

    void DashStart()
    {
        PlayerState.PlayerIsDashing = true;

        Vector3 moveDir = Quaternion.Euler(0f, cam.eulerAngles.y, 0f) * Vector3.forward;

        if (moveDir.sqrMagnitude > 1f)
        {
            moveDir = moveDir.normalized;
        }
        dashDir = moveDir;
        startDash = true;
        dashTimeRemaining = dashDuration;
        if (!isDashing)
        { 
            isDashing = true;
        }
    }
    void Dash()
    {
        if(startDash)
        {
            startDash = false;
        }
       
        if (dashTimeRemaining > 0)
        { 

            Vector3 move = dashDir * dashSpeed * Time.deltaTime;
            playerControl.Move(move);

  
            dashTimeRemaining -= Time.deltaTime;
        }
        else
        {
            DashEnd();
        }
    }
    void DashEnd()
    {

        isDashing = false;
        PlayerState.PlayerIsDashing = false ;
    }

    void AnimPlay()
    {

        anim.SetInteger("z", (int)movement.z);
        anim.SetInteger("x", (int)movement.x);

    }

    public bool isSettingScreen()
    {
        GameObject SettingScreen = GameObject.Find("Canv_Options");

        if(SettingScreen != null && SettingScreen.activeSelf == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ChangePause()
    {
        ChangePauseState.Invoke();
    }

    public bool GetController()
    {
        return isControllerActive;
    }

    public void SetController(bool value)
    {
        isControllerActive = value;
    }

    public void DisablePlayer()
    {
        gameObject.SetActive(false);
    }
}

