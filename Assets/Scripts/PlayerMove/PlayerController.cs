
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public class PlayerController : MonoBehaviour
{
    public PlayerWeaponMgr playerWeaponMgr;
    public PlayerScanner playerScanner;
    public float MaxHP = 500;
    //cur == Current ������ ��� ��
    float CurHP;

    public CharacterController playerControl;
    public Transform cam;

    Vector3 originalPos;
     
    private Vector3 movement = Vector3.zero;
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

    bool canDoubleJump;
    [SerializeField]
    bool DoubleJumpLock;

    [SerializeField]
    int layerMask = 3;

    float MaxSwapDelay;
    float SwapDelayDeltaTime;

    private void Start()
    {
        originalPos = transform.position;
        LookAtCam();
    }
    private  void Update()
    {
        SwapWeaponListener();
        if (isDashing)
        {
            Dash();
        }
        else
        {
            ControlPlayer();
        }



    }
    private void FixedUpdate()
    {
        PlayerState.PlayerCurPos = transform.position;
    }
    private void LateUpdate()
    {
        LookAtCam();
    }

    void SwapWeaponListener()
    {
        SwapDelayDeltaTime += Time.deltaTime;
        if (MaxSwapDelay < SwapDelayDeltaTime)
        {
            float wheelInput = Input.GetAxis("Mouse ScrollWheel");
            PlayerInventory.SwapWeapon(wheelInput);
            SwapDelayDeltaTime = 0;

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
        


            if (!isCallJump && !canDoubleJump && !isJumpingFirst && Input.GetButtonDown("Jump"))
            {
                Jump();
            }
            else if (!DoubleJumpLock && canDoubleJump && isJumpingFirst && Input.GetButtonDown("Jump"))    //doubleJump
            {
                DoubleJump();
            }
            if (isCallJump && playerControl.isGrounded)                            //callbackJump
            {
                Jump();
                isCallJump = false;

            }
            PlayerMove();

            if (transform.position.y < -1.3)
            {
                Restart();
                return;
            }


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
            VelocityY = -gravity * Time.deltaTime;
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
        }

    }
    void DoubleJump()
    {
        if (isJumpingFirst)
        {
            SoundManager.Instance.PlaySound2D("EFFECT_Click_Mechanical");
            VelocityY = jumpForce * 1.6f;
            startedJump = true;
            isJumpingFirst = false;
            canDoubleJump = false;
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
        transform.rotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);

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
        Debug.Log("�뽬 ����");

        isDashing = false;
        PlayerState.PlayerIsDashing = false ;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (playerControl.isGrounded && isJumpingFirst)
        {
            isJumpingFirst = false;

        }
        if(playerControl.isGrounded && canDoubleJump)
        {
            canDoubleJump = false;
        }

    }

}

