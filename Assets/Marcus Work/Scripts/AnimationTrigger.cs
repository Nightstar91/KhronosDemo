using UnityEngine;
using FMODUnity;
using FMOD.Studio;

/// <summary>
/// Attach to any GameObject.
/// Assign FMOD events in the Inspector,
/// then call these public methods from Unity Animation Events.
/// </summary>
public class AnimationTrigger : MonoBehaviour
{
    [Header("FMOD Events")]
    [SerializeField] private EventReference event1;
    [SerializeField] private EventReference event2;
    [SerializeField] private EventReference event3;

    /// <summary>
    /// Plays Event 1 at this object's position
    /// </summary>
    public void PlayEvent1()
    {
        PlayOneShot(event1);
    }

    /// <summary>
    /// Plays Event 2 at this object's position
    /// </summary>
    public void PlayEvent2()
    {
        PlayOneShot(event2);
    }

    /// <summary>
    /// Plays Event 3 at this object's position
    /// </summary>
    public void PlayEvent3()
    {
        PlayOneShot(event3);
    }

    /// <summary>
    /// Shared FMOD play logic
    /// </summary>
    private void PlayOneShot(EventReference soundEvent)
    {
        if (soundEvent.IsNull)
        {
            Debug.LogWarning($"No FMOD Event assigned on {gameObject.name}");
            return;
        }

        RuntimeManager.PlayOneShot(soundEvent, transform.position);
    }
}