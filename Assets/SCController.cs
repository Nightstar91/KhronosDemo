using UnityEngine;

public class SCController : MonoBehaviour
{
    [Header("References")]
    public Transform pivot;
    public Transform playerCamera;

    [Header("Settings")]
    public float rotationSpeed = 2f;

    void Update()
    {
        if (!pivot || !playerCamera) return;

        Vector3 direction = playerCamera.position - pivot.position;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        // IMPORTANT: preserve current orientation's "up"
        Vector3 upDirection = pivot.up;

        // Full 3D look rotation, but anchored to current up
        Quaternion targetRotation = Quaternion.LookRotation(direction, upDirection);

        // Smooth rotation
        pivot.rotation = Quaternion.Slerp(
            pivot.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }
}
