using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;

    float sensitivityX = 15F;
    float sensitivityY = 15F;

    //float minimumX = -340F;
    //float maximumX = 340F;

    float minimumY = -60F;
    float maximumY = 60F;

    private static float rotationX = 0F;
    float rotationY = 0F;

    public static float getRotationY()
    {
        return rotationX;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) Cursor.lockState = CursorLockMode.None;
        if (Input.GetKeyDown(KeyCode.LeftControl)) Cursor.lockState = CursorLockMode.Locked;

        float zoom = Input.GetAxis("Mouse ScrollWheel") * 20f;
        cam.fieldOfView += zoom;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 60f, 120f);

        // Read the mouse input axis
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = ClampAngle(rotationY, minimumY, maximumY);

        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
    }
}
