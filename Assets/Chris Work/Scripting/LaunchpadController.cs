using UnityEngine;
using System.Collections;

public class LaunchpadController : MonoBehaviour
{
    [Tooltip("Force applied to the player")]
    public float launchForce = 15f;

    [Tooltip("Direction of the launch. If Use Local Direction is enabled, this is in local space.")]
    public Vector3 launchDirection = new Vector3(1f, 0.5f, 1f);

    [Tooltip("Treat launchDirection as local to this GameObject when true")]
    public bool useLocalDirection = false;

    [Tooltip("ForceMode used when applying force to a Rigidbody")]
    public ForceMode forceMode = ForceMode.VelocityChange;

    // If your player uses a CharacterController, this provides a simple short impulse.
    public float characterControllerImpulseDuration = 0.35f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Vector3 dir = (useLocalDirection ? transform.TransformDirection(launchDirection) : launchDirection).normalized;

        // Prefer attachedRigidbody which finds the Rigidbody on the root if the collider is on a child
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            // Clear vertical velocity if you want a predictable launch
            // rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(dir * launchForce, forceMode);
            Debug.Log($"Launchpad: Added force {dir * launchForce} to Rigidbody on '{other.name}' (ForceMode={forceMode}).");
            return;
        }

        // If player uses CharacterController (non-rigidbody), apply a short movement coroutine
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null)
        {
            StartCoroutine(LaunchCharacterController(cc, dir, launchForce, characterControllerImpulseDuration));
            Debug.Log($"Launchpad: Launched CharacterController on '{other.name}'.");
            return;
        }

        Debug.LogWarning($"Launchpad on '{name}' triggered by '{other.name}', but no Rigidbody or CharacterController found. Ensure the player GameObject has a Rigidbody (non-kinematic) or a CharacterController and is tagged 'Player'.");
    }

    private IEnumerator LaunchCharacterController(CharacterController cc, Vector3 direction, float force, float duration)
    {
        // Simple impulse simulation — moves the CharacterController for 'duration' seconds,
        // affected by gravity for more natural arc.
        float elapsed = 0f;
        Vector3 velocity = direction * force;
        while (elapsed < duration)
        {
            // Move applies in world-space here (CharacterController.Move uses delta position)
            cc.Move(velocity * Time.deltaTime);
            // apply gravity over time to the velocity to create an arc
            velocity += Physics.gravity * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    // Optional: draw an arrow in the editor showing the launch direction
    private void OnDrawGizmosSelected()
    {
        Vector3 dir = (useLocalDirection ? transform.TransformDirection(launchDirection) : launchDirection).normalized;
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position + Vector3.up * 0.1f, dir);
    }
}
