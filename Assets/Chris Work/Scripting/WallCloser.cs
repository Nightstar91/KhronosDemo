using UnityEngine;

public class WallCloser : MonoBehaviour
{
    public GameObject wall; // Drag wall object here in the Inspector
    public Vector3 closedPosition; // Set this to where the wall should be when closed
    public float closeSpeed = 5f;
    private bool shouldClose = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering is the player
        if (other.CompareTag("Player"))
        {
            shouldClose = true;
        }
    }

    void Update()
    {
        if (shouldClose)
        {
            // Smoothly move the wall to the closed position
            wall.transform.position = Vector3.MoveTowards(
                wall.transform.position,
                closedPosition,
                closeSpeed * Time.deltaTime
            );
        }
    }
}