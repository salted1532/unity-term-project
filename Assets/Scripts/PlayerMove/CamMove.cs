using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{


    public Transform objToFollew;
    public float followSpeed = 10f;
    [SerializeField]
    float sens = 100f;
    float clampAngle = 70f;

    private float rotX, rotY;

    public Transform camTransform;
    public Vector3 dirNomalized;
    public Vector3 finalDir;

    [SerializeField]
    float minDist, maxDist;

    float finalDist;
    [SerializeField]
    float smoothness = 10f;


    private void Awake()
    {

    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ResetMoveInfo();
    }
    private void Update()
    {
        rotX += -(Input.GetAxis("Mouse Y")) * sens * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sens * Time.deltaTime;

        rotX = Mathf.Clamp(rotX,-clampAngle,clampAngle);

        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;

    }
    private void FixedUpdate()
    {


    }
    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, objToFollew.position , followSpeed * Time.deltaTime);
        finalDir = transform.TransformPoint(dirNomalized * maxDist);

        RaycastHit hit;
        Debug.DrawLine(objToFollew.position + finalDir, objToFollew.position, Color.red);

        if (Physics.Linecast(objToFollew.position + finalDir, objToFollew.position,out hit) && hit.collider.CompareTag("Ground"))
        {
            Debug.Log("º® °¨Áö!");
            finalDist = Mathf.Clamp(hit.distance,minDist,maxDist);
        }
        else
        {
            finalDist = maxDist;
        }
        camTransform.localPosition = Vector3.Lerp(camTransform.localPosition,dirNomalized * finalDist,Time.deltaTime * smoothness);
    }
    void ResetMoveInfo()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        dirNomalized = camTransform.localPosition.normalized;
        finalDist = camTransform.localPosition.magnitude;
    }
}
