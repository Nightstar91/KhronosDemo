using System;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.Events;

public class FMODTriggerEvent : MonoBehaviour
{
    [Header("FMOD Event")]
    public EventReference soundEvent;

    [Header("Trigger Settings")]
    public bool playOnce = true;

    [Header("FMOD Parameter")]
    [Tooltip("The name of the FMOD parameter to set.")]
    public string parameterName = "LevelSelect";
    [Tooltip("The value to set the parameter to when triggered.")]
    public float parameterValue = 0f;

    private EventInstance instance;
    private bool hasPlayed = false;
    private bool eventFinishedFlag = false;

    public UnityEvent openDialogueDoor;
    public event Action OnEventFinished;
    public SubtitleController subs;

    private void Start()
    {
        subs = GameObject.Find("SubtitleUI").GetComponent<SubtitleController>();
        subs.instance = instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed && playOnce) return;

        if (other.CompareTag("Player"))
        {
            PlayEvent();
            hasPlayed = true;
        }
    }

    private void PlayEvent()
    {
        instance = RuntimeManager.CreateInstance(soundEvent);

        // Set the FMOD parameter before starting
        if (!string.IsNullOrEmpty(parameterName))
        {
            instance.setParameterByName(parameterName, parameterValue);
        }

        instance.start();
    }

    private void Update()
    {
        if (instance.isValid())
        {
            PLAYBACK_STATE state;
            instance.getPlaybackState(out state);

            if (state == PLAYBACK_STATE.STOPPED && !eventFinishedFlag)
            {
                eventFinishedFlag = true;
                //Debug.Log($"FMOD Event finished: {soundEvent.Path}"); // Shows which event finished
                openDialogueDoor.Invoke();

                OnEventFinished?.Invoke();
            }
        }
    }

    private void OnDestroy()
    {
        if (instance.isValid())
        {
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instance.release();
        }
    }
}