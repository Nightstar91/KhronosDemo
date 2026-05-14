using UnityEngine;

public class TriggerPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float speed = 3f;

    private bool isPlayerOnPlatform = false;

    void Start()
    {
        // Automatically set current position as start if left default
        if (startPosition == Vector3.zero) startPosition = transform.position;
    }

    void Update()
    {
        // Select target destination based on player presence
        Vector3 destination = isPlayerOnPlatform ? targetPosition : startPosition;

        // Smoothly move towards destination
        transform.position = Vector3.MoveTowards(
            transform.position,
            destination,
            speed * Time.deltaTime
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPlatform = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPlatform = false;
        }
    }
}