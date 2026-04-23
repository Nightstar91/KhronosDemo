using UnityEngine;

public class SCController : MonoBehaviour
{
    public Transform player;
    public Transform head; // assign this child in Inspector
    public float rotationSpeed = 5f;

    void LateUpdate()
    {
        Vector3 direction = player.position - head.position;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Axis correction (swap Unity's assumption to your model's)
            targetRotation *= Quaternion.Euler(0f, -90f, 0f);

            head.rotation = Quaternion.Slerp(
                head.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}
