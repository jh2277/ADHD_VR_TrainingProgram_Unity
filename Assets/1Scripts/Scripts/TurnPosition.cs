using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TurnPosition : MonoBehaviour
{
    public float speed = 1;
    public float turnSpeed = 60;

    public InputActionProperty turnInputSource;
    public Rigidbody rb;

    public Transform directionSource;
    public Transform turnSource;
    private Vector2 inputMoveAxis;
    private float inputTurnAxis;

    // Update is called once per frame
    void Update()
    {
        inputTurnAxis = turnInputSource.action.ReadValue<Vector2>().x;
    }


    private void FixedUpdate()
    {

        Quaternion yaw = Quaternion.Euler(0, directionSource.eulerAngles.y, 0);
        Vector3 direction = yaw * new Vector3(inputMoveAxis.x, 0, inputMoveAxis.y);

        Vector3 targetMovePosition = rb.position + direction * Time.fixedDeltaTime * speed;


        Vector3 axis = Vector3.up;
        float angle = turnSpeed * Time.fixedDeltaTime * inputTurnAxis;

        Quaternion q = Quaternion.AngleAxis(angle, axis);

        rb.MoveRotation(rb.rotation * q);

        Vector3 newPosition = q * (targetMovePosition - turnSource.position) + turnSource.position;

        rb.MovePosition(newPosition);
    }
}



