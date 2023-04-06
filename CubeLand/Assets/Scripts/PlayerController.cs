using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform camTransform;

    public float sensitivity = 4.0f; // Sensitivity set by player

    private float rotX = 0.0f;
    private float rotY = 0.0f;

    private float minimumX = -360f;
    private float maximumX = 360f;

    private float minimumY = -85f;
    private float maximumY = 85f;

    private Quaternion originalRotation;
    public float smoothSpeed = 0.35f;

    [HideInInspector]
    public float sensitivityAmt = 4.0f; // Actual sensitivity

    // Start is called before the first frame update
    void Start()
    {
        camTransform = Camera.main.transform;  // storing the transform of the gameObject
        Cursor.visible = false; // hiding the cursor
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true; // Restricting the rotation due to external physics if any RigidBody is attached
        }
        //Setting the initial sensitivity to that set by the player
        sensitivityAmt = sensitivity;
        //TODO : syncing rotation
        originalRotation = camTransform.localRotation;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //速度：每秒移动5个单位长度
    public float moveSpeed = 5;
    //角速度：每秒旋转135度
    public float angularSpeed = 135;

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        //获取竖直、水平轴的值，范围在-1到1
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        //按照矢量移动一段距离
        transform.Translate(camTransform.forward * v * Time.deltaTime * moveSpeed);
        transform.Translate(camTransform.right * h * Time.deltaTime * moveSpeed);

        //calculating the rotation w.r.t the X and Y axis from the input
        rotX += Input.GetAxisRaw("Mouse X") * sensitivityAmt * Time.timeScale;
        rotY += Input.GetAxisRaw("Mouse Y") * sensitivityAmt * Time.timeScale;
        //Limiting the value of the rotation so that it doesn't cross a certain angle
        rotX = limitAngle(rotX, minimumX, maximumX);
        rotY = limitAngle(rotY, minimumY, maximumY);
        //Applying the rotation
        Quaternion xQuaternion = Quaternion.AngleAxis(rotX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotY, -Vector3.right);
        camTransform.rotation = Quaternion.Slerp(camTransform.rotation, originalRotation * xQuaternion * yQuaternion, smoothSpeed * Time.smoothDeltaTime * 60 / Time.timeScale);
    }
    public static float limitAngle(float angle, float min, float max)   // To limit the angle moved by the player
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }
}

