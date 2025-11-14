using UnityEngine;
using FMODUnity;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager Instance;
    private float scrollCooldown = 0.06f;
    private float lastScrollTime = 0f;

    private EventReference buttonClickEvent = new EventReference
    {
        Guid = FMOD.GUID.Parse("1acae913-b2b5-427d-ba11-70d33efc2ce3")
    };

    private EventReference buttonHoverEvent = new EventReference
    {
        Guid = FMOD.GUID.Parse("80b4c251-19c9-41f4-8624-d1f8ad75dd63")
    };

    private EventReference buttonReturnEvent = new EventReference
    {
        Guid = FMOD.GUID.Parse("a60f9688-caa1-4a57-ac69-cda880759008")
    };

    private EventReference sliderScrollEvent = new EventReference
    {
        Guid = FMOD.GUID.Parse("544403b6-7275-49e6-895c-9b53e31c9657")
    };

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            // Optional: keep between scenes
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayClick()
    {
        RuntimeManager.PlayOneShot(buttonClickEvent);
    }

    public void PlayHover()
    {
        RuntimeManager.PlayOneShot(buttonHoverEvent);
    }

    public void PlayReturn()
    {
        RuntimeManager.PlayOneShot(buttonReturnEvent);
    }

    public void PlayScroll()
    {
        if (Time.unscaledTime - lastScrollTime < scrollCooldown)
            return;

        lastScrollTime = Time.unscaledTime;
        RuntimeManager.PlayOneShot(sliderScrollEvent);
    }
}