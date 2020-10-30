using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    [Header("Player Movement")]
    public float SPEED = 60f;
    public float JUMP_SPEED = 8f;
    public float GRAVITY = 10f;

    [Header("Camera")]
    public float sensitivity = 3f;
    public float maxYAngle = 80f;
    public float minYAngle = -40f;
    public GameObject cameraTarget;
    
    CharacterController controller;
    Vector3 moveDirection = Vector3.zero;
    Vector2 currentRotation = Vector2.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        RotateCamera();
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (controller.isGrounded)
        {
            // calculate camera relative direction to move:
            Vector3 m_CamForward = Vector3.Scale(cameraTarget.transform.forward, new Vector3(1, 0, 1)).normalized;
            moveDirection = Input.GetAxis("Horizontal") * m_CamForward + Input.GetAxis("Vertical") * cameraTarget.transform.right;

            // moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            // moveDirection = Vector3.Scale(moveDirection, cameraTarget.transform.forward);
            moveDirection *= SPEED;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = JUMP_SPEED;
            }
        }

        moveDirection.y -= GRAVITY * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);
    }

    private void RotateCamera() 
    {
        currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
        currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
        currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
        currentRotation.y = Mathf.Clamp(currentRotation.y, minYAngle, maxYAngle);
        transform.rotation = Quaternion.Euler(0, -currentRotation.x, 0);
        cameraTarget.transform.rotation = Quaternion.Euler(currentRotation.y, 0, 0);
    }
}
