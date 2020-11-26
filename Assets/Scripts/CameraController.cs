using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerManager player;
    public GameObject rightArm;
    public GameObject rightForeArm;
    public float sensitivity = 500f;
    public float clampAngle = 85f;

    private float verticalRotation;
    private float horizontalRotation;
    private Quaternion armRotation;

    private void Start()
    {
        verticalRotation = transform.localEulerAngles.x;
        horizontalRotation = transform.localEulerAngles.y;
        armRotation = Quaternion.Euler(-3.372f, 18.024f, -75.964f);
    }

    private void Update()
    {
        // Debug.Log(Input.GetAxis("Mouse X"));
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorMode();
        }
        if (Cursor.lockState == CursorLockMode.Locked)
            Look();

        Debug.DrawRay(transform.position, transform.forward * 2, Color.red);
    }

    private void LateUpdate()
    {
        rightArm.transform.localRotation = armRotation;
        rightForeArm.transform.localRotation = Quaternion.Euler(24.405f, -22.576f, -85.684f);

        
    }

    private void Look()
    {
        float _mouseVertical = -Input.GetAxis("Mouse Y");
        float _mouseHorizontal = Input.GetAxis("Mouse X");

        horizontalRotation = player.transform.rotation.eulerAngles.y;
        armRotation = rightArm.transform.localRotation;

        verticalRotation += _mouseVertical * sensitivity * Time.deltaTime;
        horizontalRotation += _mouseHorizontal * sensitivity * Time.deltaTime;

        if (verticalRotation > 90)
        {
            verticalRotation = 90;
        } else if (verticalRotation < -90)
        {
            verticalRotation = -90;
        }
        else
        {
            rightArm.transform.RotateAround(rightArm.transform.position, rightArm.transform.right, -_mouseVertical * sensitivity * Time.deltaTime);
            armRotation = rightArm.transform.localRotation;

        }
        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        
        player.transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
        
    }

    private void ToggleCursorMode()
    {
        if (player.respawning)
        {
            return;
        }
        Cursor.visible = !Cursor.visible;

        if(Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ResetRotation()
    {
        verticalRotation = transform.localEulerAngles.x;
        transform.localRotation = Quaternion.Euler(0f, transform.rotation.y, transform.rotation.z);
    }

    public void RespawnPlayer(Quaternion _rotation)
    {
        horizontalRotation = transform.localEulerAngles.y;
        player.transform.rotation = _rotation;
    }
    
}
