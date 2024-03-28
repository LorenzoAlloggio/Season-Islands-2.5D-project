using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTest : MonoBehaviour
{
    public Transform player;
    public float sensitivityX = 1f;
    public float sensitivityY = 1f;
    public float distanceFromPlayer = 5f;
    public float minVerticalAngle = -80f;
    public float maxVerticalAngle = 80f;
    public float smoothness = 5f; // Adjust this to control the smoothness of camera rotation

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        // Rotate the player horizontally
        player.Rotate(Vector3.up * mouseX);

        // Rotate the camera vertically
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        // Rotate the camera around the y-axis
        rotationY += mouseX;

        // Smoothly interpolate between current rotation and desired rotation
        Quaternion currentRotation = transform.rotation;
        Quaternion desiredRotation = Quaternion.Euler(rotationX, rotationY, 0f);
        transform.rotation = Quaternion.Lerp(currentRotation, desiredRotation, smoothness * Time.deltaTime);

        // Set camera position relative to the player
        Vector3 desiredPosition = player.position - transform.forward * distanceFromPlayer;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothness * Time.deltaTime);
    }
}


// Oud Systeem
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CamTest : MonoBehaviour
//{
//    public Transform player;
//    public float sensitivityX = 1f;
//    public float sensitivityY = 1f;
//    public float distanceFromPlayer = 5f;
//    public float minVerticalAngle = -80f;
//    public float maxVerticalAngle = 80f;

//    private float rotationX = 0f;
//    private float rotationY = 0f;

//    void Start()
//    {
//        Cursor.lockState = CursorLockMode.Locked;
//    }

//    void Update()
//    {
//        float mouseX = Input.GetAxis("Mouse X") * sensitivityX;
//        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

//        // Rotate the player horizontally
//        player.Rotate(Vector3.up * mouseX);

//        // Rotate the camera vertically
//        rotationX -= mouseY;
//        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

//        // Rotate the camera around the y-axis
//        rotationY += mouseX;

//        Quaternion cameraRotation = Quaternion.Euler(rotationX, rotationY, 0f);
//        transform.rotation = cameraRotation;

//        // Set camera position relative to the player
//        Vector3 desiredPosition = player.position - transform.forward * distanceFromPlayer;
//        transform.position = desiredPosition;
//    }
//}