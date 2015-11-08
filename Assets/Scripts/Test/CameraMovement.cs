using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class CameraMovement : MonoBehaviour
{
    public float AngularSpeed = 10;
    public float Speed = 100;

    private Transform _transform;
    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        float xAxisValue = Input.GetAxis("Horizontal");
        float yAxisValue = Input.GetAxis("Vertical");
        
        _rb.velocity = new Vector3(xAxisValue, _rb.velocity.y, yAxisValue) * Speed * Time.fixedDeltaTime;

        _rb.angularVelocity = new Vector3(0f, Input.GetAxis("Mouse X"), 0f) * AngularSpeed;
    }
}
