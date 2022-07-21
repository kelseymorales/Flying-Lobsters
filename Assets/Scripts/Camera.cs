using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] int iSenseHori; // Sensitivity horizontally
    [SerializeField] int iSenseVert; // Sensitivity vertically

    // Set field that camera can operate in (in a range min to max)
    [SerializeField] int iLockVertMin;
    [SerializeField] int iLockVertMax;

    [SerializeField] bool isYAxisInverted; // toggle for inverted camera controls

    float fXAxisRotation = 0.0f;

    // Called at Start
    void Start()
    {
        // Lock cursor to center of screen & make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // get input
        float mouseX = Input.GetAxis("Mouse X") * iSenseHori * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * iSenseVert * Time.deltaTime;

        // inverted controls check/handling
        if (isYAxisInverted)
        {
            fXAxisRotation += mouseY;
        }
        else
        {
            fXAxisRotation -= mouseY;
        }

        // Clamp the angle the camera can rotate to
        fXAxisRotation = Mathf.Clamp(fXAxisRotation, iLockVertMin, iLockVertMax);

        // Rotate camera on the x-axis
        transform.localRotation = Quaternion.Euler(fXAxisRotation, 0, 0);

        // Rotate the parent (player) transform to follow camera
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
