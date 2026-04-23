using UnityEngine;

public class SCController : MonoBehaviour
{
    public Transform pivot;
    public Transform cameraHead;
    public Transform playerCamera;

    private Quaternion initialOffset;

    public float rotationSpeed = 2f;

    void Start()
    {
        // Store difference between pivot and head at start
        initialOffset = Quaternion.Inverse(pivot.rotation) * cameraHead.rotation;
    }

    void Update()
    {
        if (!pivot || !cameraHead || !playerCamera) return;

        Vector3 direction = playerCamera.position - pivot.position;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, pivot.up);

        pivot.rotation = Quaternion.Slerp(
            pivot.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );

        // 🔑 KEY FIX: apply offset so it rotates around pivot correctly
        cameraHead.SetPositionAndRotation(cameraHead.position, pivot.rotation);
    }
}
