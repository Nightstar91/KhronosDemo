using UnityEngine;

public class ForwardArrow : MonoBehaviour
{
    // THIS CODE WAS SOURCE FROM CLAUDE AI SONNET V4.6
    // Prompt: How can i tell in the editor what direction is an empty game object is facing?

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);

        // Optional: small sphere at the tip so it's easy to spot
        Gizmos.DrawSphere(transform.position + transform.forward * 2f, 0.1f);
    }
}