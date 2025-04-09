using UnityEngine;
public class CameraRotation : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform playerCam;

    float xRotation;
    float yRotation;

    bool isOpen;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isOpen = false;
    }
    private void Update()
    {
        if (!isOpen && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)))
        {
            isOpen = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (isOpen && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)))
        {
            isOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (!isOpen)
        {
            float mouseX = Input.GetAxis("Mouse X") * Time.fixedDeltaTime * sensX;
            float mouseY = Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * sensY;

            //Find current look rotation
            Vector3 rot = playerCam.transform.localRotation.eulerAngles;
            float desiredX = rot.y + mouseX;

            //Rotate, and also make sure we dont over- or under-rotate.
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //Perform the rotations
            playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
            orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
        }

    }
}