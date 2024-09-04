using UnityEngine;
using System;

public class MoveCamera : MonoBehaviour
{
    private float cameraRotationX;
    private float cameraRotationY;

    // Assignables have to be added in the inspector if not taken in script
    [Header("Assignables")]
    [SerializeField]
    private Transform orientation; // Direction of where player is looking (Important)
    [SerializeField]
    private Transform playerRenderer; // Visual for player to rotate along the camera

    // enum to choose rotation axis in the Unity editor
    public enum RotationAxes
    {
        MouseXAndY = 0,     // yaw and pitch
        MouseX = 1,         // yaw only
        MouseY = 2          // pitch only
    }
    // rotation axis
    public RotationAxes axes = RotationAxes.MouseX;

    // rotation sensitivity
    public float sensitivityHor = 9.0f;
    public float sensitivityVert = 9.0f;

    // min and max pitch angles
    public float minimumVert = -45.0f;
    public float maximumVert = 45.0f;

    // for the pitch
    private float verticalRot = 0;

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
            return; // Return immediately after unlocking to avoid camera movement
        }
        
        // change in pitch
        verticalRot -= Input.GetAxis("Mouse Y") * sensitivityVert;
        verticalRot = Mathf.Clamp(verticalRot, minimumVert, maximumVert);

        // change in yaw
        float delta = Input.GetAxis("Mouse X") * sensitivityHor;
        float horizontalRot = transform.localEulerAngles.y + delta;

        // set pitch and yaw
        transform.localEulerAngles = new Vector3(verticalRot, horizontalRot, 0f);

        orientation.transform.localEulerAngles = new Vector3(0f, horizontalRot, 0f);
        playerRenderer.rotation = orientation.rotation;
    }

}
