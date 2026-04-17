using UnityEngine;
using System.Collections;

public class LaunchpadController : MonoBehaviour
{
    public float launchForce = 20f;
    public Vector3 launchDirection = new Vector3(0f, 1f, 1f);
    public bool useLocalDirection = true;
    public float disableControlDuration = 1.0f; // How long the arc lasts

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        CharacterController cc = other.GetComponent<CharacterController>();
        FPSController fps = other.GetComponent<FPSController>();

        if (cc != null && fps != null)
        {
            Vector3 dir = (useLocalDirection ? transform.TransformDirection(launchDirection) : launchDirection).normalized;
            StartCoroutine(HandleCharacterArc(cc, fps, dir * launchForce));
        }
    }

    private IEnumerator HandleCharacterArc(CharacterController cc, FPSController fps, Vector3 velocity)
    {
        fps.enabled = false;

        // Increase this multiplier to make the "fall" faster/snappier
        float gravityMultiplier = 3.0f;
        float elapsed = 0f;

        while (elapsed < disableControlDuration)
        {
            // Move the player
            cc.Move(velocity * Time.deltaTime);

            // Apply heavier gravity so the fast launch actually curves
            velocity += Physics.gravity * gravityMultiplier * Time.deltaTime;

            // Ground check to end the launch early if they land
            if (cc.isGrounded && elapsed > 0.1f) break;

            elapsed += Time.deltaTime;
            yield return null;
        }

        fps.enabled = true;
    }
}