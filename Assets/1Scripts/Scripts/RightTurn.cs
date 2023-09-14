using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class RightTurn : MonoBehaviour
{
    public Transform playerCamera; // 회전시킬 카메라
    public float rotationSpeed = 1.0f; // 회전 속도

    private Vector2 rotationInput; // 조이스틱 입력

    // Input System에서 호출되는 메서드
    public void OnRotate(InputAction.CallbackContext context)
    {
        rotationInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        // 오른쪽 조이스틱 입력을 기반으로 카메라 회전
        float rotationAmount = rotationInput.x * rotationSpeed * Time.deltaTime;
        playerCamera.Rotate(Vector3.up, rotationAmount);
    }
}