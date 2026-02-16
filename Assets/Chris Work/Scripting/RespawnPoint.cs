using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    // Reference to the RespawnPoint GameObject in the scene
    public Transform respawnPoint;

    // The Y-value threshold below which the player will respawn
    public float fallThreshold = -10f;

    private Vector3 startPosition; // Stores the player's initial position on Start

    void Start()
    {
        // If a specific respawn point is not assigned, use the player's starting position
        if (respawnPoint == null)
        {
            startPosition = transform.position;
        }
    }

    void Update()
    {
        // Check if the player's y position is below the fall threshold
        if (transform.position.y <= fallThreshold)
        {
            RespawnPlayer();
        }
    }

    void RespawnPlayer()
    {
        // Teleport the player back to the respawn point's position
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }
        else
        {
            transform.position = startPosition;
        }

        // Optional: Reset player velocity if using Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // You can add other logic here, like deducting health or playing a sound
        Debug.Log("Player Respawned!");
    }
}