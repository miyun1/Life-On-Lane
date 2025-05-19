using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    public Transform target;

    public float distance = 10f;
    public float minDistance = 5f;
    public float maxDistance = 20f;
    public float height = 8f;
    public float zoomSpeed = 5f;
    public float fixedYaw = 0f;
    public float fixedPitch = 45f;

    void LateUpdate()
    {
        if (target == null) return;

        // Camera zoom with scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // Calculate camera position with fixed rotation
        Quaternion rotation = Quaternion.Euler(fixedPitch, fixedYaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        Vector3 cameraPosition = target.position + Vector3.up * height + offset;

        transform.position = cameraPosition;
        transform.LookAt(target.position + Vector3.up * height * 0.5f);
    }
}
