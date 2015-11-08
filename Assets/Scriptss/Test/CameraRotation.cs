using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour
{
    public float AngularSpeed = 10;
    public float MaxYAngle = 290;
    public float MinYAngle = 70;

    private Transform _transform;

    void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleRotationInput();
    }

    private void HandleRotationInput()
    {
        Vector3 rotation = _transform.localEulerAngles + new Vector3(-Input.GetAxis("Mouse Y"), 0f, 0f) * AngularSpeed;

        if (rotation.x < MaxYAngle && rotation.x > 180)
	    {
            rotation.x = MaxYAngle;
	    }
        else if (rotation.x > MinYAngle && rotation.x < 180)
        {
            rotation.x = MinYAngle;
        }

        _transform.localEulerAngles = rotation;
    }
}
