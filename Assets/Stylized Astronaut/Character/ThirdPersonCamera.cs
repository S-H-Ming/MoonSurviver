using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    private const float Y_ANGLE_MIN = -50.0f;
    private const float Y_ANGLE_MAX = 50.0f;

    public Transform lookAt;
    public Transform camTransform;
    public float distance = 5.0f;

    private float maxDistance = 8f, minDistance = 1f;
    private int layerMask;
    private float camSphereRadius = .5f;
    private float camLerpSpeed = .05f;

    private float currentX = 0.0f;
    private float currentY = 45.0f;
    private float mouseSensitivity = .75f;
    // private float sensitivityX = 20.0f;
    // private float sensitivityY = 20.0f;

    private void Start()
    {
        camTransform = transform;
        layerMask = LayerMask.NameToLayer("Ignore Raycast");
        layerMask = ~layerMask;
    }

    private void Update()
    {
        currentX += Input.GetAxis("Mouse X") * mouseSensitivity;
        currentY += Input.GetAxis("Mouse Y") * mouseSensitivity;

        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // Adjust camera distance for camera collision
        RaycastHit hit;
        if (Physics.SphereCast(lookAt.position, camSphereRadius, rotation * dir, out hit, maxDistance, layerMask))
        {
            float targetDist = Mathf.Clamp(hit.distance, minDistance, maxDistance); 
            distance = Mathf.Lerp(distance, targetDist, camLerpSpeed);
        }
        else
        {
            distance = Mathf.Lerp(distance, maxDistance, camLerpSpeed);
        }

        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
    }
}
