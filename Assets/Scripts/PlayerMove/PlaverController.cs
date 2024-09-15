using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaverController : MonoBehaviour
{

    public Rigidbody playerRigid;
    public Transform cam;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private Vector3 movement = Vector3.zero;
    private Vector3 dir = Vector3.zero;
    private List<KeyCode> pressedKeysX = new List<KeyCode>();
    private List<KeyCode> pressedKeysZ = new List<KeyCode>();
    float speed = 6f;
    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.forward*10f,Color.red);
        if (Input.anyKey)
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

                if(pressedKeysX.Count == 0)
                {
                    movement.x = 0;
                }
                else if(pressedKeysZ.Count == 0)
                {
                    movement.z = 0;
                }
                //방향 파라미터
                float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

                Vector3 moveDir =  Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                playerRigid.position += (moveDir.normalized * speed * Time.deltaTime);
            }

        }
        else
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
        }
    }

}

